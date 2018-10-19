using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akka.NetVisualAPI.Models
{
    public class VectorClock
    {
        public string Sender { get; private set; }
        public string Receiver { get; private set; }
        public string Message { get; private set; }
        public Dictionary<string, int> Clock { get; private set; }

        public VectorClock() { }

        public VectorClock(string sender, string receiver, string message, Dictionary<string, int> clock)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
            Clock = clock;
        }
    }
}