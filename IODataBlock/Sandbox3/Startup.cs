using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Sandbox3.Startup))]
namespace Sandbox3
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
