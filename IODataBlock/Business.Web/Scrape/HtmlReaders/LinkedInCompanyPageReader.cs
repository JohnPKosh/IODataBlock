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
                if (InputDto == null) return TryScrapeDto();

                rv.FollowUrl = InputDto.GetValue("FollowUrl").Value<string>();
                rv.PhotoUrl = InputDto.GetValue("PhotoUrl").Value<string>();
                rv.LinkedInCompanyName = InputDto.GetValue("LinkedInCompanyName").Value<string>();
                //rv.FollowersText = InputDto.GetValue("FollowersText").Value<string>();
                //int followers;
                //if (!string.IsNullOrWhiteSpace(rv.FollowersText) && int.TryParse(rv.FollowersText.Replace("followers", string.Empty).Replace(",", string.Empty).Trim(), out followers))
                //{
                //    rv.FollowersCount = followers;
                //}
                //else
                //{
                //    rv.FollowersCount = -1;
                //}
                rv.FollowersCount = InputDto.GetValue("FollowersCount").Value<int?>();

                var companyidstring = InputDto.GetValue("LinkedInCompanyId").Value<string>();
                long companyid;
                if (!string.IsNullOrWhiteSpace(companyidstring) && long.TryParse(companyidstring, out companyid))
                {
                    rv.LinkedInCompanyId = companyid;
                }
                else
                {
                    rv.LinkedInCompanyId = -1;
                }

                rv.CompanyDescription = InputDto.GetValue("CompanyDescription").Value<string>();
                rv.DomainName = InputDto.GetValue("DomainName").Value<string>();
                rv.Specialties = InputDto.GetValue("Specialties").Value<string>();
                rv.StreetAddress = InputDto.GetValue("StreetAddress").Value<string>();
                rv.Locality = InputDto.GetValue("Locality").Value<string>();
                rv.Region = InputDto.GetValue("Region").Value<string>();
                rv.PostalCode = InputDto.GetValue("PostalCode").Value<string>();
                rv.CountryName = InputDto.GetValue("CountryName").Value<string>();
                rv.Website = InputDto.GetValue("Website").Value<string>();
                rv.Industry = InputDto.GetValue("Industry").Value<string>();
                rv.CompanyType = InputDto.GetValue("CompanyType").Value<string>();
                rv.CompanySize = InputDto.GetValue("CompanySize").Value<string>();
                rv.Founded = InputDto.GetValue("Founded").Value<string>();
                rv.LinkedInCompanyUrl = InputDto.GetValue("LinkedInCompanyUrl").Value<string>();
                return rv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override LinkedInCompanyScrapeDto TryScrapeDto()
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
                rv.LinkedInCompanyName = htmlUtility.GetLinkedInCompanyName();
                //rv.FollowersText = htmlUtility.GetLinkedInFollowersCount();
                //int followers;
                //if (!string.IsNullOrWhiteSpace(rv.FollowersText) && int.TryParse(rv.FollowersText.Replace("followers", string.Empty).Replace(",", string.Empty).Trim(), out followers))
                //{
                //    rv.FollowersCount = followers;
                //}
                //else
                //{
                //    rv.FollowersCount = -1;
                //}
                rv.FollowersCount = InputDto.GetValue("CompanyName").Value<int?>();

                var companyidstring = htmlUtility.GetLinkedInCompanyId();
                long companyid;
                if (!string.IsNullOrWhiteSpace(companyidstring) && long.TryParse(companyidstring, out companyid))
                {
                    rv.LinkedInCompanyId = companyid;
                }
                else
                {
                    rv.LinkedInCompanyId = -1;
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
                    rv.CompanyType = about.type;
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
                rv.LinkedInCompanyUrl = locurl;
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