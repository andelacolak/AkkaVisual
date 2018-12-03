using Akka.NetVisualAPI.Helpers;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;

namespace Akka.NetVisualAPI.Controllers
{
    public class VisualController : ApiController
    {
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<VisualHub>();

        [HttpPost]
        public HttpStatusCode SaveVectorClock([FromBody] JObject vectorClock)
        {
            JObject json = JObject.Parse(vectorClock.First.Path);
            UpdateClient(json).Wait();
            
            return HttpStatusCode.OK;
        }

        private static async Task UpdateClient(JObject json)
        {
            var connections =  DBHelper.GetConnectionIds(json["user"].ToString());
            await hubContext.Clients.Clients(connections).broadcastMessage(json);
        }
    }
}
