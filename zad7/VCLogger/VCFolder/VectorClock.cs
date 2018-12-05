using Akka.Actor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using VCLogger.VCFolder;

namespace VCLogger
{
    [Serializable]
    public class VectorClock
    {
        public VCActor Sender { get; private set; }
        public VCActor Receiver { get; private set; }
        public VCMessage Message { get; private set; }
        public VCClock Clock { get; private set; }
        public string User { get; private set; }

        #region VectorClock Constructors
        public VectorClock()
        {
            Sender = null;
            Receiver = null;
            Message = new VCMessage();
            Clock = new VCClock();
            User = null;
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

        public void Update(VCActor sender, VCActor receiver, VCMessage message, string user)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
            User = user;
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

        public override string ToString()
        {
            return string.Format("{{\"sender\": {0}, \"receiver\": {1}, \"message\": {{{2}}}, \"clock\": {{ \"{3}\" }}, \"user\": \"{4}\"}}", 
                Sender.ToString(),
                Receiver.ToString(),
                Message.ToString(),
                Clock.ToString(),
                User);
        }
    }
}
