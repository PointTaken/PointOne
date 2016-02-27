using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;


namespace PT.PointOne.WebAPI
{
    [HubName("Chat")]
    public class Chat : Hub
    {
        public void Distribute(string msg)
        {
            Clients.All.send(msg);
        }
    }
    public partial class BeerAdvocateTest : System.Web.UI.Page
    {
       
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
    }
}