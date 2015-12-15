using System;
using System.Collections.Generic;
using Business.Common.System;
using Newtonsoft.Json;

namespace HubSpot.Models.Properties
{
    public class PropertyUpdateValue : IPropertyUpdateValue
    {
        public PropertyUpdateValue(string key, string value, PropertyTypeModel propertyType = null)
        {
            this.Key = key;
            this.Value = value;
            this.PropertyType = propertyType;
        }

        public PropertyUpdateValue(string key, object value)
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
        public PropertyTypeModel PropertyType { get; set; }

        #region Conversion Operators

        static public implicit operator KeyValuePair<string,string>(PropertyUpdateValue item)
        {
            return new KeyValuePair<string, string>(item.Key, item.Value);
        }

        static public implicit operator PropertyUpdateValue(KeyValuePair<string, string> value)
        {
            return new PropertyUpdateValue(value.Key, value.Value);
        }

        static public implicit operator PropertyUpdateValue(KeyValuePair<string, object> value)
        {
            return new PropertyUpdateValue(value.Key, value.Value);
        }

        #endregion Conversion Operators
    }
}
