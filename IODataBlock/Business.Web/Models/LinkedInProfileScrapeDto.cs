using System.Collections.Generic;

namespace Business.Web.Models
{
    public class LinkedInProfileScrapeDto
    {
        public long? LinkedInProfileId { get; set; }
        public string LinkedInPage { get; set; }
        public string LinkedInFullName { get; set; }
        public int LinkedInConnections { get; set; }
        public string LinkedInTitle { get; set; }
        public string LinkedInCompanyName { get; set; }
        public long? LinkedInCompanyId { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Im { get; set; }
        public string Twitter { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string LinkedInPhotoUrl { get; set; }
    }
}