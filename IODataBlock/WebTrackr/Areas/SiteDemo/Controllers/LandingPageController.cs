using System.Web.Mvc;

namespace WebTrackr.Areas.SiteDemo.Controllers
{
    public class LandingPageController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
