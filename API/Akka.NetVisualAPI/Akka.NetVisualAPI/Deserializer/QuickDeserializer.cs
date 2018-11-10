using Akka.NetVisualAPI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Akka.NetVisualAPI.Deserializer
{
    public static class QuickDeserializer
    {
        public static VectorClock Deserialize(JObject vectorClock)
        {
            string sender = vectorClock["sender"].ToObject<string>();
            string receiver = vectorClock["receiver"].ToObject<string>();
            string messageName = vectorClock["message"].ToObject<string>();
            Dictionary<string, string> messageProps = ConvertToStringDict(vectorClock["message_props"].ToString());
            Dictionary<string, int> clock = ConvertToIntDict(vectorClock["clock"].ToString());

            return new VectorClock(sender, receiver, new VCMessage(messageName, messageProps), clock);
        }

        private static Dictionary<string, string> ConvertToStringDict(string str)
        {
            return Regex.Matches(str, @"\s*(.*?)\s*=\s*(.*?)\s*(;|$)")
               .OfType<Match>()
               .ToDictionary(m => m.Groups[1].Value, m => m.Groups[2].Value);
        }

        private static Dictionary<string, int> ConvertToIntDict(string str)
        {
            return Regex.Matches(str, @"\s*(.*?)\s*=\s*(.*?)\s*(;|$)")
               .OfType<Match>()
               .ToDictionary(m => m.Groups[1].Value, m => int.Parse(m.Groups[2].Value));
        }
    }
}