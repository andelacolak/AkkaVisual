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

        public override string ToString()
        {
            return string.Format("\"name\": \"{0}\", \"props\": {{ \"{1}\" }}", Name, SerializeProps());
        }

        private string SerializeProps()
        {
            return Props.Count() > 0 ? string.Join("\", \"", Props.Select(kv => kv.Key + "\": \"" +
                PrepToEncode(kv.Value.ToString())).ToArray()) : "data\": \"NONE";
        }

        private string PrepToEncode(string prop)
        {
            return prop.Replace("[", "").Replace("]", "").Replace("=", ":");
        }
    }
}
