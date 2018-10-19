using Akka.NetVisualAPI.Models;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Akka.NetVisualAPI
{
    public static class VectorClockHolder
    {
        private static VectorClock _value { get; set; }

        public static VectorClock GetVectorClock()
        {
            return _value;
        }

        public static void SetVectorClock(VectorClock v)
        {
            _value = v;
        }
    }
}