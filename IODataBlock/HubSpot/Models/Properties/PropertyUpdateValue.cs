using Business.Common.System;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HubSpot.Models.Properties
{
    public class PropertyUpdateValue : IPropertyUpdateValue
    {
        public PropertyUpdateValue(string key, string value, PropertyTypeModel propertyType = null)
        {
            Key = key;
            Value = value;
            PropertyType = propertyType;
        }

        public PropertyUpdateValue(string key, object value)
        {
            Key = key;
            if (value == null) return;
            if (value is DateTime) Value = new UnixMsTimestamp((DateTime)value).ToString();
            else Value = value.ToString();
        }

        [JsonProperty("property")]
        public string Key { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonIgnore]
        public PropertyTypeModel PropertyType { get; set; }

        #region Conversion Operators

        static public implicit operator KeyValuePair<string, string>(PropertyUpdateValue item)
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