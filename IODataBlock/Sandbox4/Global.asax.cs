﻿using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using JavaScriptEngineSwitcher.Core;
using Sandbox4.App_Start;

namespace Sandbox4
{
    // Note: For instructions on enabling IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=301868
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            JsEngineSwitcherConfig.Configure(JsEngineSwitcher.Instance);
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            /* Remove the XML formatter */
            //ConfigureApi(GlobalConfiguration.Configuration);

            /* Create custom mappings in the Mapping Config class */
            MappingConfig.RegisterMaps();
        }

        void ConfigureApi(HttpConfiguration config)
        {
            // Remove the XML formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);
        }
    }
}
