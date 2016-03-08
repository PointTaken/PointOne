using Microsoft.AspNet.SignalR;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PT.PointOne.WebAPI
{
    public static class DistributR
    {
        public static void Distribute(dynamic obj)
        {
            var hubC = GlobalHost.ConnectionManager.GetHubContext<Chat>();
            if (hubC != null)
            {
                hubC.Clients.All.hello(obj);
            }
        }
    }
}