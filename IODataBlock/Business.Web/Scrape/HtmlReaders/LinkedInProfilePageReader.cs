using Business.Web.Models;
using Business.Web.Scrape.Services;
using Flurl;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Business.Web.System;

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

                rv.LinkedInPage = InputDto.GetValueOrDefault<string>("LinkedInPage");
                rv.LinkedInProfileId = InputDto.GetValueOrDefault<long?>("LinkedInProfileId");
                rv.LinkedInFullName = InputDto.GetValueOrDefault<string>("LinkedInFullName");
                var connections = -1;
                var connectionsText = InputDto.GetValueOrDefault<string>("LinkedInConnections");
                if (!string.IsNullOrWhiteSpace(connectionsText))
                {
                    int.TryParse(
                    connectionsText
                        .Replace(",", string.Empty)
                        .Replace("followers", string.Empty)
                        .Replace("connections", string.Empty)
                        .Trim(), out connections);
                }
                rv.LinkedInConnections = connections;

                rv.LinkedInTitle = InputDto.GetValueOrDefault<string>("LinkedInTitle");
                rv.LinkedInCompanyName = InputDto.GetValueOrDefault<string>("LinkedInCompanyName");
                rv.LinkedInCompanyId = InputDto.GetValueOrDefault<long?>("LinkedInCompanyId");
                rv.Industry = InputDto.GetValueOrDefault<string>("Industry");
                rv.Location = InputDto.GetValueOrDefault<string>("Location");
                rv.Email = InputDto.GetValueOrDefault<string>("Email");
                rv.Im = InputDto.GetValueOrDefault<string>("Im");
                rv.Twitter = InputDto.GetValueOrDefault<string>("Twitter");
                rv.Address = InputDto.GetValueOrDefault<string>("Address");
                rv.Phone = InputDto.GetValueOrDefault<string>("Phone");
                rv.LinkedInPhotoUrl = InputDto.GetValueOrDefault<string>("LinkedInPhotoUrl");

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