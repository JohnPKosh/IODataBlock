using System.Collections.Generic;
using Newtonsoft.Json;

namespace HubSpot.Models
{
    public class ContactListDto
    {
        public List<ContactDto> contacts { get; set; }

        [JsonProperty("has-more")]
        public bool has_more { get; set; }

        [JsonProperty("vid-offset")]
        public int vid_offset { get; set; }
    }
}