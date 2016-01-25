using System.Collections.Generic;
using System.IO;
using Business.Common.Configuration;
using Business.Common.IO;
using Business.Common.System.States;
using HubSpot.Models.Properties;
using HubSpot.Services;
using HubSpot.Services.Contacts;
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
            var jsonFilePath = Path.Combine(IOUtility.AppDataFolderPath, @"ContactPropertyList.json");
            var propertyManager = new PropertyManager(new ContactPropertyService(_hapiKey), new JsonFileLoader(new FileInfo(jsonFilePath)));
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
