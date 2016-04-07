using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NsRestWeb.Helpers;

namespace NsRestWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {

            NsRestContactHelper helper = new NsRestContactHelper();
            var contact = helper.SearchJObjectsByEmail(@"jkosh@cloudroute.com");
            if (contact != null)
            {
                var companyItem = contact["columns"]["company"];
                var NsCompanyName = companyItem.Value<string>("name");
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}