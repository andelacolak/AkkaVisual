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
            response.Content = new StringContent(File.ReadAllText(@"C:\Users\Andela\Desktop\Anđela\pmf\Raspodijeljeni sustavi\acolak_rs_projekt\MailboxLibrary\API\Akka.NetVisualAPI\Akka.NetVisualAPI/index.html"));
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");
            return response;
        }
    }
}
