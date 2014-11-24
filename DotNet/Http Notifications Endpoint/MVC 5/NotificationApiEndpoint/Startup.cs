using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NotificationApiEndpoint.Startup))]
namespace NotificationApiEndpoint
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
        }
    }
}
