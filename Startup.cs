using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProjectScheduling.Startup))]
namespace ProjectScheduling
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
