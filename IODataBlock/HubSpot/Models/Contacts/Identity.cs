using Business.Common.System;

namespace HubSpot.Models.Contacts
{
    public class Identity
    {
        public string type { get; set; }

        public string value { get; set; }

        public UnixMsTimestamp timestamp { get; set; }
    }
}