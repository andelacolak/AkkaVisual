using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

namespace API.Controllers
{
    [ApiController]
    public class VisualController : ControllerBase
    {
        public IHubContext<VisualHub> _hubContext { get; private set; }

        public VisualController(IHubContext<VisualHub> hubContext)
        {
            _hubContext = hubContext;
        }

        [HttpPost]
        [Route("api/vector_clock/save")]
        public HttpStatusCode SaveVectorClock([FromBody] JObject vectorClock)
        {
            var test = vectorClock.ToString();
            JObject json = JObject.Parse(vectorClock.ToString());
            UpdateClient(json).Wait();

            return HttpStatusCode.OK;
        }

        private async Task UpdateClient(JObject json)
        {
            //var connections = DBHelper.GetConnectionIds(json["user"].ToString());
            //await _hubContext.Clients.Clients(connections).SendAsync("VCMessage", json);
            await _hubContext.Clients.All.SendAsync("VCMessage", json);
        }
    }
}