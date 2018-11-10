using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCLogger
{
    class VectorClockHelper
    {
        public static Dictionary<string, VectorClock> VectorClockList = new Dictionary<string, VectorClock>();

        public static void Update(string actorPath, VectorClock vectorClock)
        {
            if (VectorClockList.ContainsKey(actorPath))
            {
                VectorClockList[actorPath] = vectorClock;
            }
            else
            {
                VectorClockList.Add(actorPath, vectorClock);
            }
        }

        public static VectorClock GetVectorClock(string actorPath)
        {
            return VectorClockList.ContainsKey(actorPath) ? VectorClockList[actorPath] : new VectorClock();
        }
    }
}
