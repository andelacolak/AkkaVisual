using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using VCLogger.VCFolder;

namespace VCLogger
{
    [Serializable]
    public class VectorClock
    {
        public string Sender { get; private set; }
        public string Receiver { get; private set; }
        public VCMessage Message { get; private set; }
        public VCClock Clock { get; private set; }

        #region VectorClock Constructors
        public VectorClock()
        {
            Sender = null;
            Receiver = null;
            Message = new VCMessage();
            Clock = new VCClock();
        }

        public VectorClock(string sender, string receiver, VCMessage message, VCClock clock)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
            Clock = clock;
        }
        #endregion

        #region VectorClock Methods
        public void Tick(string name)
        {
            //if new name
            if (!Clock.Keys.Contains(name))
            {
                Clock.Add(name, 0);
            }
            Clock[name]++;
        }

        public void Update(string sender, string receiver, VCMessage message)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
        }

        public void Merge(VectorClock other)
        {
            foreach (var el in other.Clock.ToList())
            {
                if (Clock.ContainsKey(el.Key))
                {
                    Clock[el.Key] = Math.Max(el.Value, Clock[el.Key]);
                }
                else
                {
                    Clock.Add(el.Key, el.Value);
                }
            }
        }
        #endregion

        #region Deep Cloning 
        public static T DeepClone<T>(T obj)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(ms, obj);
                ms.Position = 0;

                return (T)formatter.Deserialize(ms);
            }
        }
        #endregion
    } 
}
