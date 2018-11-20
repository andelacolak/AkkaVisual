using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VCLogger.VCFolder
{
    [Serializable]
    public class VCActor
    {
        public string Path { get; private set; }
        public string Type { get; private set; }

        public VCActor(string path, string type)
        {
            Path = path;
            Type = type == null ? "" : type;
        }

        public override string ToString()
        {
            return string.Format("{{ \"path\" : \"{0}\", \"type\" : \"{1}\" }}", Path, Type);
        }
    }
}
