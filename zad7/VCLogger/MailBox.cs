using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch;
using Akka.Dispatch.MessageQueues;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
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

            OnSend(receiver, envelope.Sender, message);
            var a = receiver.GetType().FullName;
            var b = receiver.GetType().GetProperties().Select(x => x.Name);
            OnReceive(receiver, envelope.Sender, message);

            SendToAPI();

            _messageQueue.Enqueue(receiver, envelope);
        }

        public bool TryDequeue(out Envelope envelope)
        {
            return _messageQueue.TryDequeue(out envelope);
        }

        private VCMessage GetMessage(Envelope envelope)
        {
            VCMessage message = new VCMessage(envelope.Message.GetType().Name);
            var props = envelope.Message.GetType().GetProperties();

            foreach (var prop in props)
            {
                var value = envelope.Message.GetType().GetProperty(prop.Name).GetValue(envelope.Message);
                message.AddProp(prop.Name, value.ToString());
            }

            return message;
        }

        private void OnSend(IActorRef receiver, IActorRef sender, VCMessage message)
        {
            _clock_sender = VectorClockHelper.GetVectorClock(sender.Path.ToString());

            _clock_sender.Update(
               sender.Path.ToString(),
               receiver.Path.ToString(),
               message
               );

            _clock_sender.Tick(sender.Path.ToString());

            VectorClockHelper.Update(sender.Path.ToString(), _clock_sender);
        }

        private void OnReceive(IActorRef receiver, IActorRef sender, VCMessage message)
        {
            _clock_receiver = VectorClockHelper.GetVectorClock(receiver.Path.ToString());

            _clock_receiver.Update(
               sender.Path.ToString(),
               receiver.Path.ToString(),
               message
               );
            
            _clock_receiver.Merge(_clock_sender);

            _clock_receiver.Tick(receiver.Path.ToString());

            VectorClockHelper.Update(receiver.Path.ToString(), _clock_receiver);
        }

        private void SendToAPI()
        {
            using (var client = new WebClient())
            {
                var values = QuickSerializer.Serialize(_clock_sender);

                try
                {
                    var response = client.UploadValues("http://localhost:51510/api/vector_clock/save", values);
                    var responseString = Encoding.Default.GetString(response);
                }
                catch
                {
                    System.Diagnostics.Debug.WriteLine("[ERROR] Visualisation API connection has failed.");
                }
            }
        }
    }
}
