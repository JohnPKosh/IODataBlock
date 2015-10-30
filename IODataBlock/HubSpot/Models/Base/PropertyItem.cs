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
    public class PropertyItem
    {
        public PropertyItem(string key, string value)
        {
            this.Key = key;
            this.Value = value;
        }

        public PropertyItem(string key, object value)
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

        static public implicit operator KeyValuePair<string,string>(PropertyItem item)
        {
            return new KeyValuePair<string, string>(item.Key, item.Value);
        }

        static public implicit operator PropertyItem(KeyValuePair<string, string> value)
        {
            return new PropertyItem(value.Key, value.Value);
        }

        static public implicit operator PropertyItem(KeyValuePair<string, object> value)
        {
            return new PropertyItem(value.Key, value.Value);
        }

        #endregion Conversion Operators
    }
}
