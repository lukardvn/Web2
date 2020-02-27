using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp.Hubs
{
    [HubName("Vehicle")]
    public class VehicleHub : Hub
    {
        public VehicleHub()
        {
        }

        public static IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<VehicleHub>();
    }
}