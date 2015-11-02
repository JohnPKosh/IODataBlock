using Newtonsoft.Json;

namespace HubSpot.Models.Contacts
{
    public class Version
    {
        public string value { get; set; }

        [JsonProperty("source-type")]
        public string sourceType { get; set; }

        [JsonProperty("source-id")]
        public string sourceId { get; set; }

        [JsonProperty("source-label")]
        public string sourceLabel { get; set; }

        public long timestamp { get; set; }

        public bool selected { get; set; }
    }
}