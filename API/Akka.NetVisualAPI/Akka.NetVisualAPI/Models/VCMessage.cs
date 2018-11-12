using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akka.NetVisualAPI.Models
{
    public class VCMessage
    {
        public string Name { get; private set; }
        public Dictionary<string, string> Props { get; private set; }

        public VCMessage(string name, Dictionary<string, string> props)
        {
            Name = name;
            Props = props;
        }
    }
}