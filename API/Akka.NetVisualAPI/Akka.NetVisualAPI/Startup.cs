using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.SignalR;

[assembly: OwinStartup(typeof(Akka.NetVisualAPI.Startup))]
namespace Akka.NetVisualAPI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //Enable SignalR
            app.MapSignalR();
        }
    }
}
