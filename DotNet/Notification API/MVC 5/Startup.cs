using Microsoft.Owin;
using Owin;
using SocketLabs.NotificationApi.TestEndpoint.Mvc5;

[assembly: OwinStartup(typeof(Startup))]
namespace SocketLabs.NotificationApi.TestEndpoint.Mvc5
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
