using Business.Web.Models;
using Business.Web.Scrape.Services;
using Flurl;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Web.Scrape.HtmlReaders
{
    public class LinkedInCompanyPageReader : PageReaderBase<LinkedInCompanyScrapeDto>
    {
        #region Class Initialization

        public LinkedInCompanyPageReader(JObject value) : base(value)
        {
        }

        #endregion Class Initialization



        #region Try Read Data Methods

        protected override LinkedInCompanyScrapeDto TryGetDto()
        {
            try
            {
                var rv = new LinkedInCompanyScrapeDto();
                if (Document == null) return null;
                var html = Document;
                var htmlUtility = new LinkedInCompanyHtml(html);
                var links = htmlUtility.AllHrefUrlsWhere(x => x.HasAttributes && x.GetAttributeValue("href", string.Empty).Contains("linkedin.com")).ToList();
                rv.CompanyUrls = new HashSet<string>();
                rv.PeopleToInvite = new HashSet<string>();
                rv.FollowUrl = string.Empty;
                rv.PhotoUrl = htmlUtility.GetLinkedInCompanyPhotoUrl();
                rv.CompanyName = htmlUtility.GetLinkedInCompanyName();
                rv.FollowersText = htmlUtility.GetLinkedInFollowersCount();
                int followers;
                if (!string.IsNullOrWhiteSpace(rv.FollowersText) && int.TryParse(rv.FollowersText.Replace("followers", string.Empty).Replace(",", string.Empty).Trim(), out followers))
                {
                    rv.Followers = followers;
                }
                else
                {
                    rv.Followers = -1;
                }

                var companyidstring = htmlUtility.GetLinkedInCompanyId();
                double companyid;
                if (!string.IsNullOrWhiteSpace(companyidstring) && double.TryParse(companyidstring, out companyid))
                {
                    rv.CompanyId = companyid;
                }
                else
                {
                    rv.CompanyId = -1;
                }

                rv.CompanyDescription = htmlUtility.GetLinkedInCompanyDescription();
                var about = htmlUtility.GetLinkedInCompanyAboutSection();
                if (about != null)
                {
                    rv.DomainName = about.website;
                    rv.Specialties = about.specialties;
                    rv.StreetAddress = about.streetAddress;
                    rv.Locality = about.locality;
                    rv.Region = about.region;
                    rv.PostalCode = about.postalCode;
                    rv.CountryName = about.countryName;
                    rv.Website = about.website;
                    rv.Industry = about.industry;
                    rv.Type = about.type;
                    rv.CompanySize = about.companySize;
                    rv.Founded = about.founded;
                }

                foreach (var link in links)
                {
                    var url = new Url(link);
                    if (link.Contains("linkedin.com/people/invite"))
                    {
                        rv.PeopleToInvite.Add(link);
                    }
                    if (link.Contains("linkedin.com/company/follow/submit?") && url.QueryParams.ContainsKey("id"))
                    {
                        rv.FollowUrl = link;
                        rv.CompanyUrls.Add("https://linkedin.com/company/" + url.QueryParams["id"]);
                    }
                    if (!link.Contains("linkedin.com/company/") || (!link.Contains("trk=company_logo") && !link.Contains("trk=top_nav_home"))) continue;
                    url.QueryParams.Clear();
                    rv.CompanyUrls.Add(url.ToString());
                }
                /* TODO set LocationUrl here temporarily - needs review */
                var locurl = rv.CompanyUrls.OrderByDescending(x => x).FirstOrDefault();
                if (string.IsNullOrWhiteSpace(locurl)) locurl = Location;
                rv.LocationUrl = locurl;
                return rv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion Try Read Data Methods
    }
}