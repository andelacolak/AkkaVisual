using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace AkkaVisual
{
    public class VectorClock
    {
        public Actor Sender { get; private set; }
        public Actor Receiver { get; private set; }
        public Message Message { get; private set; }
        public Dictionary<string, int> Clock { get; private set; }
        public string User { get; private set; }

        public VectorClock()
        {
            Sender = null;
            Receiver = null;
            Message = new Message();
            Clock = new Dictionary<string, int>();
            User = null;
        }

        public void Tick(string name)
        {
            if (!Clock.ContainsKey(name))
                Clock.Add(name, 0);

            Clock[name]++;
        }

        public void Update(Actor sender, Actor receiver, Message message, string user)
        {
            Sender = sender;
            Receiver = receiver;
            Message = message;
            User = user;
        }

        public void Merge(VectorClock other)
        {
            foreach (var el in other.Clock)
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
    }
}
