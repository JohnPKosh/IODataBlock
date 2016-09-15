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


                rv.LinkedInPage = InputDto.GetValue("LinkedInPage").Value<string>();
                rv.LinkedInProfileId = InputDto.GetValue("LinkedInProfileId").Value<long?>();
                rv.LinkedInFullName = InputDto.GetValue("LinkedInFullName").Value<string>();
                //rv.Connections = InputDto.GetValue("Connections").Value<int>();
                var connections = -1;
                int.TryParse(
                    InputDto.GetValue("LinkedInConnections")
                        .Value<string>()
                        .Replace(",", string.Empty)
                        .Replace("followers", string.Empty)
                        .Replace("connections", string.Empty)
                        .Trim(), out connections);
                rv.LinkedInConnections = connections;

                rv.LinkedInTitle = InputDto.GetValue("LinkedInTitle").Value<string>();
                rv.LinkedInCompanyName = InputDto.GetValue("LinkedInCompanyName").Value<string>();
                rv.LinkedInCompanyId = InputDto.GetValue("LinkedInCompanyId").Value<long?>();
                rv.Industry = InputDto.GetValue("Industry").Value<string>();
                rv.Location = InputDto.GetValue("Location").Value<string>();
                rv.Email = InputDto.GetValue("Email").Value<string>();
                rv.Im = InputDto.GetValue("Im").Value<string>();
                rv.Twitter = InputDto.GetValue("Twitter").Value<string>();
                rv.Address = InputDto.GetValue("Address").Value<string>();
                rv.Phone = InputDto.GetValue("Phone").Value<string>();
                rv.LinkedInPhotoUrl = InputDto.GetValue("LinkedInPhotoUrl").Value<string>();


                return rv;
            }
            catch (Exception ex)
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

                rv.LinkedInPage = htmlUtility.GetLinkedInProfileUrl();
                rv.LinkedInProfileId = htmlUtility.GetLinkedInProfileId();
                rv.LinkedInFullName = htmlUtility.GetLinkedInFullName();
                rv.LinkedInConnections = htmlUtility.GetLinkedInConnections();
                rv.LinkedInTitle = htmlUtility.GetLinkedInTitle();
                rv.LinkedInCompanyName = htmlUtility.GetLinkedInCompanyName();
                rv.LinkedInCompanyId = htmlUtility.GetCurrentLinkedInCompanyId();
                rv.Industry = htmlUtility.GetLinkedInIndustry();
                rv.Location = htmlUtility.GetLinkedInLocation();
                rv.Email = htmlUtility.GetLinkedInEmail();
                rv.Im = htmlUtility.GetLinkedInIm();
                rv.Twitter = htmlUtility.GetLinkedInTwitter();
                rv.Address = htmlUtility.GetLinkedInAddress();
                rv.Phone = htmlUtility.GetLinkedInPhone();
                rv.LinkedInPhotoUrl = htmlUtility.GetLinkedInProfilePhotoUrl();


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