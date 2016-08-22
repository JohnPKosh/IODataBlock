using System.Web.Mvc;

namespace WebTrackr.Controllers
{
    public class CommonViewsController : Controller
    {

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Error_One()
        {
            return View();
        }

        public ActionResult Error_Two()
        {
            return View();
        }

        public ActionResult LockScreen()
        {
            return View();
        }

    }
}
