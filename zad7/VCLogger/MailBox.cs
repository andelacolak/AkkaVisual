using Akka.Actor;
using Akka.Configuration;
using Akka.Configuration.Hocon;
using Akka.Dispatch;
using Akka.Dispatch.MessageQueues;
using Newtonsoft.Json.Linq;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using VCLogger.VCFolder;

namespace VCLogger
{
    class VisualMailboxType : MailboxType, IProducesMessageQueue<VisualMailbox>
    {
        public VisualMailboxType(Settings settings, Config config) : base(settings, config)
        {
        }

        public override IMessageQueue Create(IActorRef owner, ActorSystem system)
        {
            var m = new UnboundedMailbox();
            return new VisualMailbox(m.Create(owner, system), owner, system);
        }
    }

    class VisualMailbox : IMessageQueue, IUnboundedMessageQueueSemantics, IMultipleConsumerSemantics
    {
        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private VectorClock _clock_sender = new VectorClock();
        private VectorClock _clock_receiver = new VectorClock();

        private IMessageQueue _messageQueue;

        public VisualMailbox(IMessageQueue messageQueue, IActorRef owner, ActorSystem system)
        {
            _messageQueue = messageQueue;
        }

        public int Count { get { return _messageQueue.Count; } }

        public bool HasMessages { get { return _messageQueue.HasMessages; } }

        public void CleanUp(IActorRef owner, IMessageQueue deadletters)
        {
            _messageQueue.CleanUp(owner, deadletters);
        }

        public void Enqueue(IActorRef receiver, Envelope envelope)
        {
            VCMessage message = GetMessage(envelope);
            VCActor senderActor = new VCActor(envelope.Sender.Path.ToString(), GetActorType(envelope.Sender));
            VCActor receiverActor = new VCActor(receiver.Path.ToString(), GetActorType(receiver));

            OnSend(receiverActor, senderActor, message);

            OnReceive(receiverActor, senderActor, message);

            SendToAPI();

            _messageQueue.Enqueue(receiver, envelope);
        }

        public bool TryDequeue(out Envelope envelope)
        {
            return _messageQueue.TryDequeue(out envelope);
        }

        private string GetUser()
        {
            var config = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            var configStr = config.AkkaConfig.Root.ToString();
            return configStr.Substring(configStr.IndexOf("user") + 7).Split()[0];
        }

        private VCMessage GetMessage(Envelope envelope)
        {
            VCMessage message = new VCMessage(envelope.Message.GetType().Name);
            var props = envelope.Message.GetType().GetProperties();

            foreach (var prop in props)
            {
                try
                {
                    var value = envelope.Message.GetType().GetProperty(prop.Name).GetValue(envelope.Message);
                    if (value == null) { throw new Exception(); }
                    message.AddProp(prop.Name, value.ToString());
                }
                catch { message.AddProp(prop.Name, "");}
            }

            return message;
        }

        private string GetActorType(IActorRef actor)
        {
            object props;

            if (IsField(actor))
                props = actor.GetType().GetField("Props", bindingFlags).GetValue(actor);
            else if (IsProp(actor))
                props = actor.GetType().GetProperty("Props", bindingFlags).GetValue(actor);
            else return null;

            var type = props.GetType().GetProperty("TypeName", bindingFlags).GetValue(props);
            return type.ToString().Split(',')[0];
        }

        private bool IsField(IActorRef actor)
        {
            return actor.GetType().GetField("Props", bindingFlags) != null;
        }

        private bool IsProp(IActorRef actor)
        {
            return actor.GetType().GetProperty("Props", bindingFlags) != null;
        }

        private void OnSend(VCActor receiver, VCActor sender, VCMessage message)
        {
            _clock_sender = VectorClockHelper.GetVectorClock(sender.Path.ToString());

            _clock_sender.Update(
               sender,
               receiver,
               message,
               GetUser()
               );

            _clock_sender.Tick(sender.Path.ToString());

            VectorClockHelper.Update(sender.Path.ToString(), _clock_sender);
        }

        private void OnReceive(VCActor receiver, VCActor sender, VCMessage message)
        {
            _clock_receiver = VectorClockHelper.GetVectorClock(receiver.Path.ToString());

            _clock_receiver.Update(
               sender,
               receiver,
               message,
               GetUser()
               );

            _clock_receiver.Merge(_clock_sender);

            _clock_receiver.Tick(receiver.Path.ToString());

            VectorClockHelper.Update(receiver.Path.ToString(), _clock_receiver);
        }

        private void SendToAPI()
        {
            var uri = "http://localhost:5000/api/vector_clock/save";
            var byteArray = Encoding.Default.GetBytes(_clock_sender.ToString());
            var request = WebRequest.Create(uri);
            request.Credentials = CredentialCache.DefaultCredentials;
            ((HttpWebRequest)request).UserAgent = "Akka.NET Visualiser";
            ((HttpWebRequest)request).Accept = "application/json";
            request.Method = "POST";
            request.ContentLength = byteArray.Length;
            request.ContentType = "application/json";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(byteArray, 0, byteArray.Length);
            dataStream.Close();
        }
    }
}
