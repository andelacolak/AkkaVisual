using System;
using System.Collections.Generic;
using System.Text;

namespace AkkaVisual
{
    public class Message
    {
        public string Name { get; private set; }
        public Dictionary<string, string> Props { get; private set; }

        public Message(string name = null)
        {
            Name = name;
            Props = new Dictionary<string, string>();
        }

        public void AddProp(string key, string value)
        {
            Props.Add(key, value);
        }
    }
}
