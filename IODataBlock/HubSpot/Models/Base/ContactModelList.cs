using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HubSpot.Models.Contacts;
using Newtonsoft.Json;

namespace HubSpot.Models.Base
{
    public class ContactModelList
    {
        [JsonProperty("contacts")]
        public List<ContactModel> contacts { get; set; }

        [JsonProperty("has-more")]
        public bool has_more { get; set; }

        [JsonProperty("vid-offset")]
        public int vid_offset { get; set; }

        [JsonProperty("total")]
        public int total { get; set; }

        [JsonProperty("query")]
        public string query { get; set; }
    }
}
