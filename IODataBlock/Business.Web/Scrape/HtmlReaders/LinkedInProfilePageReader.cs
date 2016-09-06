using Business.Web.Models;
using Business.Web.Scrape.Services;
using Flurl;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Web.Scrape.HtmlReaders
{
    public class LinkedInProfilePageReader : PageReaderBase<LinkedInProfileScrapeDto>
    {
        #region Class Initialization

        public LinkedInProfilePageReader(JObject value) : base(value)
        {
        }

        #endregion Class Initialization



        #region Try Read Data Methods


        protected override LinkedInProfileScrapeDto TryGetDto()
        {
            try
            {
                var rv = new LinkedInProfileScrapeDto();
                if (Document == null) return null;
                var html = Document;
                var htmlUtility = new LinkedInProfileHtml(html);

                rv.ProfileUrl = htmlUtility.GetLinkedInProfileUrl();
                rv.ProfileId = htmlUtility.GetLinkedInProfileId();
                rv.FullName = htmlUtility.GetLinkedInFullName();
                rv.Connections = htmlUtility.GetLinkedInConnections();
                rv.Title = htmlUtility.GetLinkedInTitle();
                rv.CompanyName = htmlUtility.GetLinkedInCompanyName();
                rv.CompanyId = htmlUtility.GetCurrentLinkedInCompanyId();
                rv.Industry = htmlUtility.GetLinkedInIndustry();
                rv.Location = htmlUtility.GetLinkedInLocation();
                rv.Email = htmlUtility.GetLinkedInEmail();
                rv.Im = htmlUtility.GetLinkedInIm();
                rv.Twitter = htmlUtility.GetLinkedInTwitter();
                rv.Address = htmlUtility.GetLinkedInAddress();
                rv.Phone = htmlUtility.GetLinkedInPhone();
                rv.PhotoUrl = htmlUtility.GetLinkedInProfilePhotoUrl();
                

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