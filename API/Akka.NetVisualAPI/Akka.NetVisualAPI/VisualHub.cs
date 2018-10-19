using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Akka.NetVisualAPI.Models;
using Microsoft.AspNet.SignalR.Hubs;

namespace Akka.NetVisualAPI
{
    //[HubName("visual")]
    public class VisualHub : Hub
    {
        public void Send()
        {
            // Call the sendMessage method to push to clients.
            Clients.All.broadcastMessage(VectorClockHolder.GetVectorClock());
        }
    }
}