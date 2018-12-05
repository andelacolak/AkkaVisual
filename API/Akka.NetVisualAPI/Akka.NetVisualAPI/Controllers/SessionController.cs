using Akka.NetVisualAPI.Helpers;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;

namespace Akka.NetVisualAPI.Controllers
{
    public class SessionController : ApiController
    {
        [HttpGet]
        [Route("")]
        public HttpResponseMessage Index([FromUri] string userId)
        {
            UserHelper.User = userId;
            var response = new HttpResponseMessage();
            var path = HostingEnvironment.ApplicationPhysicalPath + "index.html";
            response.Content = new StringContent(File.ReadAllText(path));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
