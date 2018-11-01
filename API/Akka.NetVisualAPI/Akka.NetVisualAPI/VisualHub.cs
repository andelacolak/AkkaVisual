using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;
using Akka.NetVisualAPI.Models;
using Microsoft.AspNet.SignalR.Hubs;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Akka.NetVisualAPI
{
    //[HubName("visual")]
    public class VisualHub : Hub
    {
        private VectorClock _previousValue;

        public async Task Send()
        {
            while (true)
            {
                if (_previousValue != VectorClockHolder.GetVectorClock())
                {
                    await Clients.All.broadcastMessage(VectorClockHolder.GetVectorClock());
                    _previousValue = VectorClockHolder.GetVectorClock();
                }
            }
        }
    }
}