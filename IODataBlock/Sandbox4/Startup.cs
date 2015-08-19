using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sandbox4.Startup))]
namespace Sandbox4
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
