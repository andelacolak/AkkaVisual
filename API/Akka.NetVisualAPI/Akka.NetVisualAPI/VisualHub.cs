using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Akka.NetVisualAPI.Helpers;

namespace Akka.NetVisualAPI
{
    public class VisualHub : Hub
    {
        public override Task OnConnected()
        {
            DBHelper.AddConnection(UserHelper.User, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            DBHelper.RemoveConnection(Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }
    }
}