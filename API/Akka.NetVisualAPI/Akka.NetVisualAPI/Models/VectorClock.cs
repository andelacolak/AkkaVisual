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
        public VCMessage Message { get; private set; }
        public Dictionary<string, int> Clock { get; private set; }

        public VectorClock() { }

        public VectorClock(string sender, string receiver, VCMessage message, Dictionary<string, int> clock)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
            Clock = clock;
        }
    }
}