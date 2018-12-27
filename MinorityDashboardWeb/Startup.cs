using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(MinorityDashboard.Web.Startup))]
namespace MinorityDashboard.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
