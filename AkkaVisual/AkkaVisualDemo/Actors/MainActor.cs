using Akka.Actor;
using AkkaVisualDemo.Messages;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaVisualDemo.Actors
{
    public class MainActor : ReceiveActor
    {
        public IActorRef Child { get; set; }
        public MainActor()
        {
            Receive<CreateChild>(x => 
            {
                CreateChild();
                SendMessage();
            });
        }

        private void CreateChild()
        {
            Props childProps = Props.Create(() => new ChildActor());
            Child = Context.ActorOf(childProps, "child");
        }

        private void SendMessage()
        {
            Child.Tell(new Message("Hello world"));
        }
    }
}
