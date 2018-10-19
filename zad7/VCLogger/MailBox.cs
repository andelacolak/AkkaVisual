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
        private VectorClock _clock = new VectorClock();

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
            _clock.Update(
               envelope.Sender.Path.ToString(),
               receiver.Path.ToString(),
               envelope.Message.GetType().Name
               );

            _clock.Tick(receiver.Path.ToString());

            _clock.Merge(ReturnVectorClock(envelope.Sender));

            //delete later

           // VectorClockHelper.VectorClockList[receiver.Path.ToString()] = VectorClock.DeepClone(_clock);

           // var temp = VectorClockHelper.VectorClockList;

            SendToAPI();

            _messageQueue.Enqueue(receiver, envelope);
        }

        public bool TryDequeue(out Envelope envelope)
        {
            return _messageQueue.TryDequeue(out envelope);
        }

        private VectorClock ReturnVectorClock(IActorRef actor)
        {
            return VectorClockHelper.VectorClockList.ContainsKey(actor.Path.ToString()) ?
                VectorClockHelper.VectorClockList[actor.Path.ToString()] : new VectorClock();
        }

        private void SendToAPI()
        {
            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values["sender"] = _clock.Sender;
                values["receiver"] = _clock.Receiver;
                values["message"] = _clock.Message;
                values["clock"] = _clock.Clock.ToString();

                var response = client.UploadValues("http://localhost:51510/api/vector_clock/save", values);

                var responseString = Encoding.Default.GetString(response);
            }
        }
    }
}
