using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace PT.PointOne.WebAPI
{
    public class Chat : Hub
    {
        public void Hello(string msg)
        {
            Clients.All.hello(msg);
        }
    }
}