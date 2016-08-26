using System.Collections.Generic;

namespace Business.Web.Models
{
    public class LinkedInProfileScrapeDto
    {
        public double ProfileId { get; set; }
        public string ProfileUrl { get; set; }
        public string FullName { get; set; }
        public int Connections { get; set; }
        public string Title { get; set; }
        public string CompanyName { get; set; }
        public double CompanyId { get; set; }
        public string Industry { get; set; }
        public string Location { get; set; }
        public string Email { get; set; }
        public string Im { get; set; }
        public string Twitter { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string PhotoUrl { get; set; }
    }
}