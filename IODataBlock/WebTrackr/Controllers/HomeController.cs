using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Data.DbClient;
using WebTrackr.Models.Content;

namespace WebTrackr.Controllers
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

            ViewBag.LinkedInTrendingNow = LinkedInTrendingNow();

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



        private IEnumerable<dynamic> LinkedInTrendingNow()
        {

            #region SQL

            var sql = @"
SELECT TOP 5 [Id]
      ,[LinkedInId]
      ,[LinkedInPage]
      ,[LinkedInCompanyName]
      ,[DomainName]
      ,[specialties]
      ,[streetAddress]
      ,[locality]
      ,[region]
      ,[postalCode]
      ,[countryName]
      ,[website]
      ,[industry]
      ,[type]
      ,[companySize]
      ,[founded]
      ,[followersCount]
      ,[photourl]
      ,[description]
      ,[CreatedDate]
      ,[BatchId]
FROM [TestData].[dbo].[LinkedInCompany]
ORDER BY [Id] DESC
";

            #endregion


            try
            {
                var data = Database.Query(@"Data Source=.\EXP14;Initial Catalog=TestData;Integrated Security=True;", "System.Data.SqlClient", sql, 120, "LERG%");
                if (data.Any())
                {
                    return data;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw;
            }

        }

    }
}
