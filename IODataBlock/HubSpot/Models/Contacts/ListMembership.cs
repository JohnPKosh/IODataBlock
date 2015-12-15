using Business.Common.System;
using Newtonsoft.Json;

namespace HubSpot.Models.Contacts
{
    public class ListMembership
    {
        [JsonProperty("static-list-id")]
        public int static_list_id { get; set; }

        [JsonProperty("internal-list-id")]
        public int internal_list_id { get; set; }

        public UnixMsTimestamp timestamp { get; set; }

        public int vid { get; set; }

        [JsonProperty("is-member")]
        public bool is_member { get; set; }
    }
}