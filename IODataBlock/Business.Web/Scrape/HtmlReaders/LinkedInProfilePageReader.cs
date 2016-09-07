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
                if (InputDto == null) return TryScrapeDto();


                rv.ProfileUrl = InputDto.GetValue("ProfileUrl").Value<string>();
                rv.ProfileId = InputDto.GetValue("ProfileId").Value<long?>();
                rv.FullName = InputDto.GetValue("FullName").Value<string>();
                rv.Connections = InputDto.GetValue("Connections").Value<int>();
                rv.Title = InputDto.GetValue("Title").Value<string>();
                rv.CompanyName = InputDto.GetValue("CompanyName").Value<string>();
                rv.CompanyId = InputDto.GetValue("CompanyId").Value<long?>();
                rv.Industry = InputDto.GetValue("Industry").Value<string>();
                rv.Location = InputDto.GetValue("Location").Value<string>();
                rv.Email = InputDto.GetValue("Email").Value<string>();
                rv.Im = InputDto.GetValue("Im").Value<string>();
                rv.Twitter = InputDto.GetValue("Twitter").Value<string>();
                rv.Address = InputDto.GetValue("Address").Value<string>();
                rv.Phone = InputDto.GetValue("Phone").Value<string>();
                rv.PhotoUrl = InputDto.GetValue("PhotoUrl").Value<string>();


                return rv;
            }
            catch (Exception)
            {
                return null;
            }
        }

        protected override LinkedInProfileScrapeDto TryScrapeDto()
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