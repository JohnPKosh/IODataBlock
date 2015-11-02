using System.Collections.Generic;
using Business.Common.System;
using HubSpot.Models.Base;
using Newtonsoft.Json;

namespace HubSpot.Models.Contacts
{

    public class ContactDto
    {
        public UnixMsTimestamp addedAt { get; set; }

        public int vid { get; set; }

        [JsonProperty("canonical-vid")]
        public int canonical_vid { get; set; }

        [JsonProperty("merged-vids")]
        public List<int> merged_vids { get; set; }

        [JsonProperty("portal-id")]
        public int portal_id { get; set; }

        [JsonProperty("is-contact")]
        public bool is_contact { get; set; }

        [JsonProperty("profile-token")]
        public string profile_token { get; set; }

        [JsonProperty("profile-url")]
        public string profile_url { get; set; }

        public Properties properties { get; set; }
        //public HubSpotContactProperties properties { get; set; }

        [JsonProperty("form-submissions")]
        public List<object> form_submissions { get; set; }

        [JsonProperty("list-memberships")]
        public List<ListMembership> list_memberships { get; set; }

        [JsonProperty("identity-profiles")]
        public List<IdentityProfile> identity_profiles { get; set; }

        [JsonProperty("merge-audits")]
        public List<object> merge_audits { get; set; }
    }

    //public class Country 
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsLastUrl 
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class NumUniqueConversionEvents
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsRevenue
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Createdate
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsFirstReferrer
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsEmailOptout
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HubspotOwnerId
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Annualrevenue
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Mobilephone
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsNumPageViews
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class State
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Fax
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Zip
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsLifecyclestageSubscriberDate
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsAveragePageViews
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Lastname
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Twitterhandle
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Phone
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class NumConversionEvents
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsNumEventCompletions
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Salutation
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Followercount
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Firstname
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class City
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsSocialNumBroadcastClicks
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Industry
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsLastTimestamp
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsNumVisits
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Twitterbio
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsSocialLinkedinClicks
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsSocialLastEngagement
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsTwitterid
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsSource
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Company
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Email
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsEmailOptout636817
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsFirstUrl
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Website
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Address
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsEmailOptout636825
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Jobtitle
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsFirstVisitTimestamp
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsFirstTimestamp
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Numemployees
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Lastmodifieddate
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class BlogDefaultHubspotBlogSubscription
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsSocialGooglePlusClicks
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsLastReferrer
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Message
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsSocialFacebookClicks
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HubspotOwnerAssigneddate
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Twitterprofilephoto
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsSourceData2
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsSocialTwitterClicks
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class HsAnalyticsSourceData1
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    //public class Lifecyclestage
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    ////netsuite_internal_id

    //public class NetSuiteInternalId
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}
}