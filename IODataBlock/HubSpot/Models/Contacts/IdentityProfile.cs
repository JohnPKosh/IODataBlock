using System.Collections.Generic;
using Business.Common.System;
using Newtonsoft.Json;

namespace HubSpot.Models.Contacts
{
    public class IdentityProfile
    {
        public int vid { get; set; }

        [JsonProperty("saved-at-timestamp")]
        public UnixMsTimestamp saved_at_timestamp { get; set; }

        [JsonProperty("deleted-changed-timestamp")]
        public UnixMsTimestamp deleted_changed_timestamp { get; set; }

        public List<Identity> identities { get; set; }
    }
}