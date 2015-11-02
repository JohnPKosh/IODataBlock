using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.System;
using HubSpot.Models.Contacts;
using Newtonsoft.Json;

namespace HubSpot.Models.Base
{
    public class ContactUpdateValue : IContactUpdateValue
    {
        public ContactUpdateValue(string key, string value, ContactPropertyDto propertyType = null)
        {
            this.Key = key;
            this.Value = value;
            this.PropertyType = propertyType;
        }

        public ContactUpdateValue(string key, object value)
        {
            this.Key = key;
            if (value == null) return;
            if (value is DateTime) this.Value = new UnixMsTimestamp((DateTime)value).ToString();
            else this.Value = value.ToString();
        }

        [JsonProperty("property")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonIgnore]
        public ContactPropertyDto PropertyType { get; set; }

        #region Conversion Operators

        static public implicit operator KeyValuePair<string,string>(ContactUpdateValue item)
        {
            return new KeyValuePair<string, string>(item.Key, item.Value);
        }

        static public implicit operator ContactUpdateValue(KeyValuePair<string, string> value)
        {
            return new ContactUpdateValue(value.Key, value.Value);
        }

        static public implicit operator ContactUpdateValue(KeyValuePair<string, object> value)
        {
            return new ContactUpdateValue(value.Key, value.Value);
        }

        #endregion Conversion Operators
    }
}
