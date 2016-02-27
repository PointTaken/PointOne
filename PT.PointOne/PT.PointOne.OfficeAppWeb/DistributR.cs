using Microsoft.AspNet.SignalR;
using PT.PointOne.OfficeAppWeb.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PT.PointOne.OfficeAppWeb
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