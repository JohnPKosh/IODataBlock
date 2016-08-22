using System.Web.Mvc;

namespace WebTrackr.Controllers
{
    public class LandingPageController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

    }
}
