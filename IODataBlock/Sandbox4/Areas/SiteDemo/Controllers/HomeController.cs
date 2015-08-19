using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Sandbox4.Models.Content;

namespace Sandbox4.Areas.SiteDemo.Controllers
{
    public class HomeController : Controller
    {
        private string GetContent(string controller, string action, string section, string contentId, string areaName = "")
        {
            //var myid = User.Identity.GetUserId();
            return ContentManager.Data.GetUnauthenticatedContent(controller, action, section, contentId, areaName, "Content Goes Here!");
        }

        public ActionResult Index()
        {
            var action = (string)RouteData.Values["action"];
            ViewBag.JumbotronTitle = GetContent("Home", "Index", "Jumbotron", "Title");
            ViewBag.JumbotronSubtitle = GetContent("Home", "Index", "Jumbotron", "Subtitle");
            return View();
        }

        //[Authorize]
        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            ViewBag.JumbotronTitle = GetContent("Home", "About", "Jumbotron", "Title");
            ViewBag.JumbotronSubtitle = GetContent("Home", "About", "Jumbotron", "Subtitle");

            return View();
        }

        public ActionResult Contact()
        {
            ContentManager.Reload();

            ViewBag.Message = "Your contact page.";

            ViewBag.JumbotronTitle = GetContent("Home", "Contact", "Jumbotron", "Title");
            ViewBag.JumbotronSubtitle = GetContent("Home", "Contact", "Jumbotron", "Subtitle");

            return View();
        }
    }
}
