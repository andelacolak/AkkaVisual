using Akka.NetVisualAPI.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Akka.NetVisualAPI
{
    public static class VectorClockHolder
    {
        private static VectorClock _value { get; set; }
        private static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<VisualHub>();

        public static VectorClock GetVectorClock()
        {
            return _value;
        }

        public static void SetVectorClock(VectorClock v)
        {
            _value = v;
            UpdateClient().Wait();
        }

        public static async Task UpdateClient()
        {
            if (GetVectorClock() != null)
            {
                await hubContext.Clients.All.broadcastMessage(GetVectorClock());
                SetVectorClock(null);
            }
        }
    }
}