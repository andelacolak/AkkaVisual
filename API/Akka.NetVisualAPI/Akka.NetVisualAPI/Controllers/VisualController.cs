using Akka.NetVisualAPI.Models;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace Akka.NetVisualAPI.Controllers
{
    public class VisualController : ApiController
    {
        [HttpPost]
        public HttpStatusCode SaveVectorClock([FromBody] JObject vectorClock)
        {
            string sender = vectorClock["sender"].ToObject<string>();
            string receiver = vectorClock["receiver"].ToObject<string>();
            string message = vectorClock["message"].ToObject<string>();
            Dictionary<string, int> clock = ConvertToDict(vectorClock["clock"].ToString());

            var vc = new VectorClock(sender, receiver, message, clock);
            VectorClockHolder.SetVectorClock(vc);

            return HttpStatusCode.OK;
        }

        private Dictionary<string, int> ConvertToDict(string str)
        {
            return Regex.Matches(str, @"\s*(.*?)\s*=\s*(.*?)\s*(;|$)")
               .OfType<Match>()
               .ToDictionary(m => m.Groups[1].Value, m => int.Parse(m.Groups[2].Value));
        }
    }
}
