using Newtonsoft.Json;
using System.Collections.Generic;

namespace HubSpot.Models.Companies
{
    public class CompanyModelList
    {
        [JsonProperty("results")]
        public List<CompanyModel> results { get; set; }

        [JsonProperty("hasMore")]
        public bool has_more { get; set; }

        [JsonProperty("offset")]
        public int? offset { get; set; }

        [JsonProperty("total")]
        public int total { get; set; }
    }
}