using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Web.Scrape.Services;
using Flurl;
using Newtonsoft.Json.Linq;

namespace Business.Web.Scrape.HtmlReaders
{
    public class TwitterCompanyPageReader
    {

        public TwitterCompanyPageReader(JObject value)
        {
            Input = value;
        }

        public JObject Input { get; }
        public string Location => TryReadUrlLocation();
        public IEnumerable<string> Links => TryReadLinkStrings();
        public string Document => TryReadDocument();
        public dynamic ScrapeData => TryScrapeData();

        #region Try Read Data Methods

        private string TryReadUrlLocation()
        {
            try
            {
                return Input.GetValue("location").Value<string>();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private IEnumerable<string> TryReadLinkStrings()
        {
            try
            {
                return Input.GetValue("links").Value<JArray>().Select(x => x.Value<string>());
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string TryReadDocument()
        {
            try
            {
                return Input.GetValue("document").Value<string>();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        private dynamic TryScrapeData()
        {
            try
            {
                dynamic rv = new ExpandoObject();
                if(Document == null)return null;
                var html = Document;
                var htmlUtility = new HtmlDocumentUtility(html);
                var links = htmlUtility.AllHrefUrlsWhere(x => x.HasAttributes && x.GetAttributeValue("href", string.Empty).Contains("linkedin.com")).ToList();

                rv.companyUrls = new HashSet<string>();
                rv.peopleToInvite = new HashSet<string>();
                rv.companyid = string.Empty;
                rv.followUrl = string.Empty;

                rv.twitterPhotoUrl = htmlUtility.GetTwitterCompanyPhotoUrl();
                rv.twitterCompanyName = htmlUtility.GetTwitterCompanyName();
                rv.twitterFollowersCount = htmlUtility.GetTwitterFollowersCount();
                rv.twitterCompanyId = htmlUtility.GetTwitterCompanyId();
                rv.twitterCompanyDescription = htmlUtility.GetTwitterCompanyDescription();
                rv.twitterCompanyAbout = htmlUtility.GetTwitterCompanyAboutSection();

                foreach (var link in links)
                {
                    var url = new Url(link);
                    if (link.Contains("linkedin.com/people/invite"))
                    {
                        rv.peopleToInvite.Add(link);
                    }
                    if (link.Contains("linkedin.com/company/follow/submit?") && url.QueryParams.ContainsKey("id"))
                    {
                        rv.followUrl = link;
                        rv.companyid = url.QueryParams["id"].ToString();
                        rv.companyUrls.Add("https://www.linkedin.com/company/" + rv.companyid);
                    }
                    if (!link.Contains("linkedin.com/company/") || (!link.Contains("trk=company_logo") && !link.Contains("trk=top_nav_home"))) continue;
                    url.QueryParams.Clear();
                    rv.companyUrls.Add(url.ToString());
                }
                return rv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

    }
}
