using System;
using System.Collections.Generic;
using Business.Common.Extensions;
using Business.Common.System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HubSpot.Models
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

        [JsonProperty("form-submissions")]
        public List<object> form_submissions { get; set; }

        [JsonProperty("list-memberships")]
        public List<ListMembership> list_memberships { get; set; }

        [JsonProperty("identity-profiles")]
        public List<IdentityProfile> identity_profiles { get; set; }

        [JsonProperty("merge-audits")]
        public List<object> merge_audits { get; set; }
    }

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

    public class Createdate
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    //public class HsAnalyticsFirstReferrer
    //{
    //    public string value { get; set; }

    //    public List<Version> versions { get; set; }
    //}

    public class HsEmailOptout
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HubspotOwnerId
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Annualrevenue
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Mobilephone
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsNumPageViews
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class State
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Fax
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Zip
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsLifecyclestageSubscriberDate
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsAveragePageViews
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Lastname
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Twitterhandle
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Phone
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class NumConversionEvents
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsNumEventCompletions
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Salutation
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Followercount
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Firstname
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class City
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsSocialNumBroadcastClicks
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Industry
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsLastTimestamp
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsNumVisits
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Twitterbio
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsSocialLinkedinClicks
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsSocialLastEngagement
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsTwitterid
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsSource
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Company
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Email
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsEmailOptout636817
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsFirstUrl
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Website
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Address
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsEmailOptout636825
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Jobtitle
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsFirstVisitTimestamp
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsFirstTimestamp
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Numemployees
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Lastmodifieddate
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class BlogDefaultHubspotBlogSubscription
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsSocialGooglePlusClicks
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsLastReferrer
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Message
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsSocialFacebookClicks
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HubspotOwnerAssigneddate
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Twitterprofilephoto
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsSourceData2
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsSocialTwitterClicks
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class HsAnalyticsSourceData1
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Lifecyclestage
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    //netsuite_internal_id

    public class NetSuiteInternalId
    {
        public string value { get; set; }

        public List<Version> versions { get; set; }
    }

    public class Properties
    {
        //associatedcompanyid

        public StringItem associatedcompanyid { get; set; }

        public StringItem hs_lead_status { get; set; }

        public StringItem country { get; set; }

        public StringItem hs_analytics_last_url { get; set; }

        public StringItem num_unique_conversion_events { get; set; }

        public StringItem hs_analytics_revenue { get; set; }

        public StringItem createdate { get; set; }

        public StringItem hs_analytics_first_referrer { get; set; }

        public HsEmailOptout hs_email_optout { get; set; }

        public HubspotOwnerId hubspot_owner_id { get; set; }

        public Annualrevenue annualrevenue { get; set; }

        public Mobilephone mobilephone { get; set; }

        public HsAnalyticsNumPageViews hs_analytics_num_page_views { get; set; }

        public State state { get; set; }

        public Fax fax { get; set; }

        public Zip zip { get; set; }

        public HsLifecyclestageSubscriberDate hs_lifecyclestage_subscriber_date { get; set; }

        public HsAnalyticsAveragePageViews hs_analytics_average_page_views { get; set; }

        public Lastname lastname { get; set; }

        public Twitterhandle twitterhandle { get; set; }

        public Phone phone { get; set; }

        public NumConversionEvents num_conversion_events { get; set; }

        public HsAnalyticsNumEventCompletions hs_analytics_num_event_completions { get; set; }

        public Salutation salutation { get; set; }

        public Followercount followercount { get; set; }

        public Firstname firstname { get; set; }

        public City city { get; set; }

        public HsSocialNumBroadcastClicks hs_social_num_broadcast_clicks { get; set; }

        public Industry industry { get; set; }

        public HsAnalyticsLastTimestamp hs_analytics_last_timestamp { get; set; }

        public HsAnalyticsNumVisits hs_analytics_num_visits { get; set; }

        public Twitterbio twitterbio { get; set; }

        public HsSocialLinkedinClicks hs_social_linkedin_clicks { get; set; }

        public HsSocialLastEngagement hs_social_last_engagement { get; set; }

        public HsTwitterid hs_twitterid { get; set; }

        public HsAnalyticsSource hs_analytics_source { get; set; }

        public Company company { get; set; }

        public Email email { get; set; }

        public HsEmailOptout636817 hs_email_optout_636817 { get; set; }

        public HsAnalyticsFirstUrl hs_analytics_first_url { get; set; }

        public Website website { get; set; }

        public Address address { get; set; }

        public HsEmailOptout636825 hs_email_optout_636825 { get; set; }

        public Jobtitle jobtitle { get; set; }

        public HsAnalyticsFirstVisitTimestamp hs_analytics_first_visit_timestamp { get; set; }

        public HsAnalyticsFirstTimestamp hs_analytics_first_timestamp { get; set; }

        public Numemployees numemployees { get; set; }

        public Lastmodifieddate lastmodifieddate { get; set; }

        public BlogDefaultHubspotBlogSubscription blog_default_hubspot_blog_subscription { get; set; }

        public HsSocialGooglePlusClicks hs_social_google_plus_clicks { get; set; }

        public HsAnalyticsLastReferrer hs_analytics_last_referrer { get; set; }

        public Message message { get; set; }

        public HsSocialFacebookClicks hs_social_facebook_clicks { get; set; }

        public HubspotOwnerAssigneddate hubspot_owner_assigneddate { get; set; }

        public Twitterprofilephoto twitterprofilephoto { get; set; }

        public HsAnalyticsSourceData2 hs_analytics_source_data_2 { get; set; }

        public HsSocialTwitterClicks hs_social_twitter_clicks { get; set; }

        public HsAnalyticsSourceData1 hs_analytics_source_data_1 { get; set; }

        public Lifecyclestage lifecyclestage { get; set; }

        public NetSuiteInternalId netsuite_internal_id { get; set; }
    }

    public class ListMembership
    {
        [JsonProperty("static-list-id")]
        public int static_list_id { get; set; }

        [JsonProperty("internal-list-id")]
        public int internal_list_id { get; set; }

        public long timestamp { get; set; }

        public int vid { get; set; }

        [JsonProperty("is-member")]
        public bool is_member { get; set; }
    }

    public class Identity
    {
        public string type { get; set; }

        public string value { get; set; }

        public object timestamp { get; set; }
    }

    public class IdentityProfile
    {
        public int vid { get; set; }

        [JsonProperty("saved-at-timestamp")]
        public long saved_at_timestamp { get; set; }

        [JsonProperty("deleted-changed-timestamp")]
        public int deleted_changed_timestamp { get; set; }

        public List<Identity> identities { get; set; }
    }
}