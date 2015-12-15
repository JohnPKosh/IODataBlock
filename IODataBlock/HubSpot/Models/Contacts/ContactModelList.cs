using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Models.Contacts
{
    public class ContactModelList
    {
        [JsonProperty("contacts")]
        public List<ContactModel> contacts { get; set; }

        [JsonProperty("has-more")]
        public bool has_more { get; set; }

        [JsonProperty("vid-offset")]
        public int vid_offset { get; set; }

        [JsonProperty("time-offset")]
        public long? time_offset { get; set; }

        [JsonProperty("total")]
        public int total { get; set; }

        [JsonProperty("query")]
        public string query { get; set; }
    }
}
