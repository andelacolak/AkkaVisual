using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Reflection;
using System.Text;

namespace API.Controllers
{
    public class SessionController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public ContentResult Index()
        {
            var path = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var html = System.IO.File.ReadAllText(path + "../../../../wwwroot/index.html");
            return Content(html, "text/html", Encoding.UTF8);
        }
    }
}