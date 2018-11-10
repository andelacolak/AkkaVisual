using Akka.NetVisualAPI.Deserializer;
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

            var vc = QuickDeserializer.Deserialize(vectorClock);
            VectorClockHolder.SetVectorClock(vc);

            return HttpStatusCode.OK;
        }
    }
}
