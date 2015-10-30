using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Configuration;
using Business.Common.System;
using HubSpot.Models.Contacts;
using Newtonsoft.Json;

namespace HubSpot.Models.Base
{
    public class ContactProperties : ContactPropertiesBase<ContactProperties>
    {
        public ContactProperties()
        {
            // TODO: determine if string hapikey needs added to class signature.
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
            var propertyManager = new PropertyManager(_hapiKey);
            ManagedProperties = propertyManager.Properties;
        }

        private readonly string _hapiKey;
        internal List<ContactPropertyDto> ManagedProperties;


        [JsonProperty("properties")]
        public Dictionary<string, object> Properties{ get; set; }


        #region Conversion Operators

        static public implicit operator HubSpotContactProperties(ContactProperties value)
        {
            var rv = new HubSpotContactProperties { Properties = new HashSet<PropertyItem>() };
            foreach (var p in value.Properties)
            {
                if (value.ManagedProperties.All(x => x.name != p.Key)) continue;
                var proptyp = value.ManagedProperties.First(x => x.name == p.Key);
                switch (proptyp.type)
                {
                    case "datetime":
                        long? ts = new UnixMsTimestamp((DateTime?)p.Value);
                        rv.Properties.Add(new PropertyItem(p.Key, ts.ToString()));
                        break;
                    case "bool":
                        if (!((bool?)p.Value).HasValue)
                        {
                            rv.Properties.Add(new PropertyItem(p.Key, null));
                        }
                        else if (((bool?)p.Value).Value)
                        {
                            rv.Properties.Add(new PropertyItem(p.Key, "true"));
                        }
                        else
                        {
                            rv.Properties.Add(new PropertyItem(p.Key, "true"));
                        }
                        break;
                    default:
                        rv.Properties.Add(new PropertyItem(p.Key, p.Value as string));
                        break;
                }
            }
            rv.ManagedProperties = value.ManagedProperties;
            return rv;
        }

        #endregion Conversion Operators
    }
}
