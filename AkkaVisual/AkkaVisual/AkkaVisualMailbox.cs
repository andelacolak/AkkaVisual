using Akka.Actor;
using Akka.Configuration.Hocon;
using Akka.Dispatch;
using Akka.Dispatch.MessageQueues;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AkkaVisual
{
    public class AkkaVisualMailbox : IMessageQueue, IUnboundedMessageQueueSemantics, IMultipleConsumerSemantics
    {
        //TODO: move to service
        private static readonly HttpClient client = new HttpClient();

        private const BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        private VectorClock clockSender = new VectorClock();
        private VectorClock clockRecever = new VectorClock();

        private IMessageQueue _messageQueue;

        public AkkaVisualMailbox(IMessageQueue messageQueue, IActorRef owner, ActorSystem system)
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
            Message message = GetMessage(envelope);
            Actor senderActor = new Actor(envelope.Sender.Path.ToString(), GetActorType(envelope.Sender));
            Actor receiverActor = new Actor(receiver.Path.ToString(), GetActorType(receiver));

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
            return "test";
            //var config = (AkkaConfigurationSection)ConfigurationManager.GetSection("akka");
            //var configStr = config.AkkaConfig.Root.ToString();
            //return configStr.Substring(configStr.IndexOf("user") + 7).Split()[0];
        }

        private Message GetMessage(Envelope envelope)
        {
            Message message = new Message(envelope.Message.GetType().Name);
            var props = envelope.Message.GetType().GetProperties();

            foreach (var prop in props)
            {
                try
                {
                    var value = envelope.Message.GetType().GetProperty(prop.Name).GetValue(envelope.Message);
                    if (value == null) { throw new Exception(); }
                    message.AddProp(prop.Name, value.ToString());
                }
                catch { message.AddProp(prop.Name, ""); }
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

        private void OnSend(Actor receiver, Actor sender, Message message)
        {
            clockSender = VectorClockHelper.GetVectorClock(sender.Path.ToString());

            clockSender.Update(sender, receiver, message, GetUser());

            clockSender.Tick(sender.Path.ToString());

            VectorClockHelper.Update(sender.Path.ToString(), clockSender);
        }

        private void OnReceive(Actor receiver, Actor sender, Message message)
        {
            clockRecever = VectorClockHelper.GetVectorClock(receiver.Path.ToString());

            clockRecever.Update(sender, receiver, message, GetUser());

            clockRecever.Merge(clockSender);

            clockRecever.Tick(receiver.Path.ToString());

            VectorClockHelper.Update(receiver.Path.ToString(), clockRecever);
        }

        private async Task SendToAPI()
        {
            try
            {
                var json = JsonConvert.SerializeObject(clockSender);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("http://localhost:58967/api/test", content);

                var responseString = await response.Content.ReadAsStringAsync();
                //var json = JsonConvert.SerializeObject(clockSender);
                //var byteArray = Encoding.Default.GetBytes(json);
                //var request = WebRequest.Create("http://localhost:58967/api/test");
                //request.Credentials = CredentialCache.DefaultCredentials;
                //((HttpWebRequest)request).UserAgent = "Akka.NET Visualiser";
                //request.Method = "POST";
                //request.ContentLength = byteArray.Length;
                //request.ContentType = "application/x-www-form-urlencoded";
                //Stream dataStream = request.GetRequestStream();
                //dataStream.Write(byteArray, 0, byteArray.Length);
                //dataStream.Close();
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine("[ERROR] Visualisation API connection has failed.");
            }
        }
    }
}
