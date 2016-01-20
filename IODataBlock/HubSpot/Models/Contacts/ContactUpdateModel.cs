using System.Collections.Generic;
using Business.Common.Configuration;
using HubSpot.Models.Properties;
using Newtonsoft.Json;

namespace HubSpot.Models.Contacts
{
    public class ContactUpdateModel : ModelBase<ContactUpdateModel>
    {

        public ContactUpdateModel()
        {
            // TODO: determine if string hapikey needs added to class signature.
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
            var propertyManager = new PropertyManager(_hapiKey);
            ManagedProperties = propertyManager.Properties;
        }

        private readonly string _hapiKey;
        internal List<PropertyTypeModel> ManagedProperties;
            
        [JsonProperty("properties")]
        public HashSet<PropertyUpdateValue> Properties { get; set; }

        #region Conversion Operators

        //static public implicit operator ContactModel(ContactUpdateModel value)
        //{
        //    var rv = new ContactModel {Properties = new Dictionary<string, object>()};
        //    foreach (var p in value.Properties)
        //    {
        //        if (value.ManagedProperties.All(x => x.name != p.Key)) continue;
        //        var proptyp = value.ManagedProperties.First(x => x.name == p.Key);
        //        switch (proptyp.type)
        //        {
        //            case "datetime":
        //                DateTime? ts = new UnixMsTimestamp(p.Value);
        //                rv.Properties.Add(p.Key, ts);
        //                break;
        //            case "bool":
        //                if (string.IsNullOrWhiteSpace(p.Value))
        //                {
        //                    rv.Properties.Add(p.Key, new bool?());
        //                }
        //                else if (p.Value == "true")
        //                {
        //                    rv.Properties.Add(p.Key, new bool?(true));
        //                }
        //                else
        //                {
        //                    rv.Properties.Add(p.Key, new bool?(false));
        //                }
        //                break;
        //            default:
        //                rv.Properties.Add(p.Key, p.Value);
        //                break;
        //        }
        //    }
        //    rv.ManagedProperties = value.ManagedProperties;
        //    return rv;
        //}

        #endregion Conversion Operators
    }
}
