using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(PT.PointOne.SingalRChat.Startup))]
namespace PT.PointOne.SingalRChat
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // Any connection or hub wire up and configuration should go here
            app.MapSignalR();
        }
    }
}