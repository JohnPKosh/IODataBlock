using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.System;
using HubSpot.Models.Properties;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HubSpot.Models.Contacts
{
    public class ContactModel : ContactModelBase<ContactModel>
    {
        #region Class Initialization

        public ContactModel()
        {
            // TODO: determine if string hapikey needs added to class signature.
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
            var propertyManager = new PropertyManager(_hapiKey);
            ManagedProperties = propertyManager.Properties;
        }

        #endregion

        #region Private Fields and Properties

        private readonly string _hapiKey;
        internal List<PropertyTypeModel> ManagedProperties;

        #endregion

        #region Public Properties

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

        [JsonProperty("properties")]
        public JObject Properties { get; set; }
        
        [JsonProperty("form-submissions")]
        public List<JObject> form_submissions { get; set; }

        [JsonProperty("list-memberships")]
        public List<ListMembership> list_memberships { get; set; }

        [JsonProperty("identity-profiles")]
        public List<IdentityProfile> identity_profiles { get; set; }

        [JsonProperty("merge-audits")]
        public List<JObject> merge_audits { get; set; }

        #endregion


        #region Conversion Operators

        static public implicit operator ContactViewModel(ContactModel value)
        {
            var rv = new ContactViewModel { Properties = new HashSet<PropertyValue>(),
                vid = value.vid,
                addedAt = value.addedAt,
                canonical_vid = value.canonical_vid,
                form_submissions = value.form_submissions,
                identity_profiles = value.identity_profiles,
                is_contact = value.is_contact,
                list_memberships = value.list_memberships,
                merge_audits = value.merge_audits,
                merged_vids = value.merged_vids,
                portal_id = value.portal_id,
                profile_token = value.profile_token,
                profile_url = value.profile_url,
                ManagedProperties = value.ManagedProperties
            };
            foreach (var p in value.ManagedProperties)
            {
                JToken token;
                if (value.Properties.TryGetValue(p.name, StringComparison.InvariantCulture, out token))
                {
                    var versions = (JArray)token["versions"];
                    if (versions != null)
                    {
                        var ver = versions.ToObject<List<PropertyVersion>>();
                        rv.Properties.Add(new PropertyValue(p.name, token.Value<string>("value"), new HashSet<PropertyVersion>(ver), p));
                    }
                    else
                    {
                        rv.Properties.Add(new PropertyValue(p.name, token.Value<string>("value"), null, p));
                    }
                }
                else
                {
                    rv.Properties.Add(new PropertyValue(p.name, null, null, p));
                }
            }
            return rv;
        }

        //static public implicit operator ContactUpdateModel(ContactModel value)
        //{
        //    var rv = new ContactUpdateModel { Properties = new HashSet<ContactUpdateValue>() };
        //    foreach (var p in value.Properties)
        //    {
        //        var prop = value.ManagedProperties.FirstOrDefault(x => x.name == p.Key);
        //        if (prop == null) continue;
        //        rv.Properties.Add(new ContactUpdateValue(p.Key, p.Value.Value<string>("value"), prop));

        //        //if (value.ManagedProperties.All(x => x.name != p.Key)) continue;
        //        //rv.Properties.Add(new ContactUpdateValue(p.Key, p.Value.Value<string>("value"), value.ManagedProperties.First(x=>x.name == p.Key)));
        //    }
        //    rv.ManagedProperties = value.ManagedProperties;
        //    return rv;
        //}

        //static public implicit operator ContactUpdateModel(ContactModel value)
        //{
        //    var rv = new ContactUpdateModel { Properties = new HashSet<ContactUpdateValue>() };
        //    foreach (var p in value.Properties)
        //    {
        //        if (value.ManagedProperties.All(x => x.name != p.Key)) continue;
        //        rv.Properties.Add(new ContactUpdateValue(p.Key, p.Value.value));

        //        //var proptyp = value.ManagedProperties.First(x => x.name == p.Key);
        //        //switch (proptyp.type)
        //        //{
        //        //    case "datetime":
        //        //        long? ts = new UnixMsTimestamp((DateTime?)p.Value);
        //        //        rv.Properties.Add(new ContactUpdateValue(p.Key, ts.ToString()));
        //        //        break;
        //        //    case "bool":
        //        //        if (!((bool?)p.Value).HasValue)
        //        //        {
        //        //            rv.Properties.Add(new ContactUpdateValue(p.Key, null));
        //        //        }
        //        //        else if (((bool?)p.Value).Value)
        //        //        {
        //        //            rv.Properties.Add(new ContactUpdateValue(p.Key, "true"));
        //        //        }
        //        //        else
        //        //        {
        //        //            rv.Properties.Add(new ContactUpdateValue(p.Key, "true"));
        //        //        }
        //        //        break;
        //        //    default:
        //        //        rv.Properties.Add(new ContactUpdateValue(p.Key, p.Value as string));
        //        //        break;
        //        //}
        //    }
        //    rv.ManagedProperties = value.ManagedProperties;
        //    return rv;
        //}

        #endregion Conversion Operators
    }
}
