using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace VCLogger.VectorClock
{
    [Serializable]
    public class VCClock : Dictionary<string, int>
    {
        public VCClock() : base() { }

        public VCClock(SerializationInfo info, StreamingContext context) : base(info, context) { }

    }
}
