using Business.Common.System.App;
using Flurl;
using Newtonsoft.Json.Linq;
using SelfHostWebApi.Utility;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace SelfHostWebApi.api
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { AppBag.Data.Value.BaseAddress, EnvironmentUtilities.GetComputerName() };
        }

        // GET api/values/5
        public int Get(int id)
        {
            return id;
        }

        // POST api/values
        //public void Post([FromBody]string value)
        //{
        //}

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        public string PostBody(JObject value) /* Captures: location, document, links elements of Post */
        {
            var html = value.GetValue("document").Value<string>();
            var links = value.GetValue("links").Value<JArray>().Select(x => x.Value<string>());
            var HtmlUtility = new HtmlDocumentUtility(html);
            //var elements =HtmlUtility.AllHrefUrlsWhere(x => x.HasAttributes && x.GetAttributeValue("href", string.Empty).Contains("linkedin.com")).ToList();
            var elements = links;
            var companyUrls = new HashSet<string>();
            var peopleToInvite = new HashSet<string>();
            var companyid = string.Empty;
            var followUrl = string.Empty;

            var twitterPhotoUrl = HtmlUtility.GetTwitterCompanyPhotoUrl();
            var twitterCompanyName = HtmlUtility.GetTwitterCompanyName();
            var twitterFollowersCount = HtmlUtility.GetTwitterFollowersCount();
            var twitterCompanyId = HtmlUtility.GetTwitterCompanyId();
            var twitterCompanyDescription = HtmlUtility.GetTwitterCompanyDescription();
            var twitterCompanyAbout = HtmlUtility.GetTwitterCompanyAboutSection();

            foreach (var e in elements)
            {
                // do something
                var url = new Url(e);

                if (e.Contains("linkedin.com/people/invite"))
                {
                    peopleToInvite.Add(e);
                }

                if (e.Contains("linkedin.com/company/follow/submit?") && url.QueryParams.ContainsKey("id"))
                {
                    followUrl = e;
                    companyid = url.QueryParams["id"].ToString();
                    companyUrls.Add("https://www.linkedin.com/company/" + companyid);
                }

                if (e.Contains("linkedin.com/company/") && (e.Contains("trk=company_logo") || e.Contains("trk=top_nav_home")))
                {
                    url.QueryParams.Clear();
                    companyUrls.Add(url.ToString());
                }

                //https://www.linkedin.com/company/avecinia-wellness-center?trk=company_logo
                //https://www.linkedin.com/company/avecinia-wellness-center?trk=top_nav_home
                //https://www.linkedin.com/company/caribbean-passion-cuisine?trk=extra_biz_viewers_viewed
                //https://www.linkedin.com/company/1491319
                //https://www.linkedin.com/company/follow/submit?id=1491319&fl=start&ft=0_Y3kiyf9HWAmYDU6VAEIvRVAObMxlNM_IGCvo1b5lkvTF_k9D5fA1WjmaCTx-TCZL&csrfToken=ajax%3A8554679308426727204&trk=co_followed-start
                //https://www.linkedin.com/profile/view?id=AA4AAACQwrMBDWy_MXk3zV2ZlQqGe4XoXXwHxU8&authType=name&authToken=nb6X&csrfToken=ajax%3A8554679308426727204&trk=nav_utilities_pymk_name
                //https://www.linkedin.com/people/invite?from=profile&key=308846476&firstName=Lee&lastName=Vang&connectionParam=member_desktop_profile_miniprofile&csrfToken=ajax%3A8554679308426727204&goback=&trk=miniprofile-primary-connect-button
                //https://www.linkedin.com/people/invite?from=profile&key=135521993&firstName=Mandy&lastName=Hess&connectionParam=member_desktop_profile_miniprofile&csrfToken=ajax%3A8554679308426727204&goback=&trk=miniprofile-primary-connect-button
                //https://www.linkedin.com/vsearch/p?f_CC=1498699&trk=rr_connectedness
                //https://www.linkedin.com/people/invite?from=profile&key=308846476
                //https://www.linkedin.com/people/invite?from=profile&key=104287327&firstName=Elias&lastName=Reyes&connectionParam=member_desktop_profile_miniprofile&csrfToken=ajax%3A8554679308426727204&goback=&trk=miniprofile-primary-connect-button
            }
            return string.Join("<br/>", companyUrls);
        }
    }
}