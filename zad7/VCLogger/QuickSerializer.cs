using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VCLogger
{
    public static class QuickSerializer
    {
        public static NameValueCollection Serialize(VectorClock vectorClock)
        {
            var values = new NameValueCollection();

            values["sender"] = vectorClock.Sender;
            values["receiver"] = vectorClock.Receiver;
            values["message"] = vectorClock.Message.Name;
            values["message_props"] = DictToString( vectorClock.Message.Props );
            values["clock"] = DictToString( vectorClock.Clock );

            return values;
        }

        private static string DictToString(Dictionary<string, string> dict )
        {
            return string.Join(";", dict.Select(kv => kv.Key.ToString() + " = " + kv.Value.ToString()).ToArray());
        }

        private static string DictToString(Dictionary<string, int> dict)
        {
            return string.Join(";", dict.Select(kv => kv.Key.ToString() + " = " + kv.Value.ToString()).ToArray());
        }
    }
}
