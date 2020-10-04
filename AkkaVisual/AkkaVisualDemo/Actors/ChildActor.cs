using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaVisualDemo.Actors
{
    public class ChildActor : ReceiveActor
    {
        public ChildActor()
        {
            Receive<Message>(x => Console.WriteLine(x.Content));
        }
    }
}
