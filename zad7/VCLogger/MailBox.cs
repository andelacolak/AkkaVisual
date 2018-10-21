using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch;
using Akka.Dispatch.MessageQueues;
using System.Collections.Specialized;
using System.Net;
using System.Text;

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
            OnSend(receiver, envelope);

            OnReceive(receiver, envelope);

            SendToAPI();

            _messageQueue.Enqueue(receiver, envelope);
        }

        public bool TryDequeue(out Envelope envelope)
        {
            return _messageQueue.TryDequeue(out envelope);
        }

        private void OnSend(IActorRef receiver, Envelope envelope)
        {
            _clock_sender.Update(
               envelope.Sender.Path.ToString(),
               receiver.Path.ToString(),
               envelope.Message.GetType().Name
               );

            _clock_sender.Tick(envelope.Sender.Path.ToString());
        }

        private void OnReceive(IActorRef receiver, Envelope envelope)
        {
            _clock_receiver.Update(
               envelope.Sender.Path.ToString(),
               receiver.Path.ToString(),
               envelope.Message.GetType().Name
               );
            
            _clock_receiver.Merge(_clock_sender);

            _clock_receiver.Tick(receiver.Path.ToString());
        }

        private void SendToAPI()
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["sender"] = _clock_sender.Sender;
                values["receiver"] = _clock_sender.Receiver;
                values["message"] = _clock_sender.Message;
                values["clock"] = _clock_sender.Clock.ToString();

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
