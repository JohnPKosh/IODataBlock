using System.Web.Mvc;

namespace Sandbox4.Areas.SiteDemo
{
    public class SiteDemoAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "SiteDemo";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "SiteDemo_default",
                "SiteDemo/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Sandbox4.Areas.SiteDemo.Controllers" }
            );
        }
    }
}