using Owin;
using Microsoft.Owin;
using System;

[assembly: OwinStartup(typeof(PT.PointOne.WebAPI.Startup))]
namespace PT.PointOne.WebAPI
{
    

      public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();


        }
    }
}
