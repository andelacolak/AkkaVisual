using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCLogger.VCFolder
{
    [Serializable]
    public class VCMessage
    {
        public string Name { get; private set; }
        public Dictionary<string, string> Props { get; private set; }

        public VCMessage(string name = null)
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
