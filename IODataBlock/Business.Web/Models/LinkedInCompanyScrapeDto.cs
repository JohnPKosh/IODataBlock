using System.Collections.Generic;

namespace Business.Web.Models
{
    public class LinkedInCompanyScrapeDto
    {
        public string LocationUrl { get; set; }
        public long CompanyId { get; set; }
        public string CompanyName { get; set; }
        public string DomainName { get; set; }
        public string Specialties { get; set; }
        public string StreetAddress { get; set; }
        public string Locality { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string CountryName { get; set; }
        public string Website { get; set; }
        public string Industry { get; set; }
        public string Type { get; set; }
        public string CompanySize { get; set; }
        public string Founded { get; set; }
        public string FollowersText { get; set; }
        public int Followers { get; set; }
        public string PhotoUrl { get; set; }
        public string CompanyDescription { get; set; }
        public HashSet<string> CompanyUrls { get; set; }
        public HashSet<string> PeopleToInvite { get; set; }
        public string FollowUrl { get; set; }
    }
}