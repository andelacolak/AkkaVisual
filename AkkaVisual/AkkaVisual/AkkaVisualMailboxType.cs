using Akka.Actor;
using Akka.Configuration;
using Akka.Dispatch;
using Akka.Dispatch.MessageQueues;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaVisual
{
    public class AkkaVisualMailboxType : MailboxType, IProducesMessageQueue<AkkaVisualMailbox>
    {
        public AkkaVisualMailboxType(Settings settings, Config config) : base(settings, config)
        {
        }

        public override IMessageQueue Create(IActorRef owner, ActorSystem system)
        {
            var m = new UnboundedMailbox();
            return new AkkaVisualMailbox(m.Create(owner, system), owner, system);
        }
    }
}
