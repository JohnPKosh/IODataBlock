using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebTrackr.Startup))]
namespace WebTrackr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
