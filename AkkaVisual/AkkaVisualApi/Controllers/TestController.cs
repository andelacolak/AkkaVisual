using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AkkaVisualApi.HubConfig;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json.Linq;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AkkaVisualApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private IHubContext<AkkaVisualHub> _hub;

        public TestController(IHubContext<AkkaVisualHub> hub)
        {
            _hub = hub;
        }

        // GET: api/<TestController>
        [HttpGet]
        public IActionResult Get()
        {
            _hub.Clients.All.SendAsync("transferchartdata", new List<string>() {"test1", "test2" });
            return Ok(new { Message = "Request Completed" });
        }

        [HttpPost]
        public HttpStatusCode SaveVectorClock([FromBody] JsonElement vectorClock)
        {
            _hub.Clients.All.SendAsync("transferchartdata", new List<string>() { "test1", "test2" });

            return HttpStatusCode.OK;
        }
    }
}
