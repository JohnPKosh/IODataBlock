using System;
using System.Collections.Generic;
using Business.Common.System;
using Newtonsoft.Json;

namespace HubSpot.Models.Properties
{
    public class PropertyValue: IPropertyValue
    {
        public PropertyValue(string key, string value, HashSet<PropertyVersion> versions = null, PropertyTypeModel propertyType = null)
        {
            this.Key = key;
            this.Value = value;
            this.Versions = versions;
            this.PropertyType = propertyType;
        }

        public PropertyValue(string key, object value, HashSet<PropertyVersion> versions = null)
        {
            this.Key = key;
            this.Versions = versions;
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

        public HashSet<PropertyVersion> Versions { get; set; }

        #region Conversion Operators

        static public implicit operator KeyValuePair<string, string>(PropertyValue item)
        {
            return new KeyValuePair<string, string>(item.Key, item.Value);
        }

        //static public implicit operator PropertyValue(KeyValuePair<string, string> value)
        //{
        //    return new PropertyValue(value.Key, value.Value);
        //}

        //static public implicit operator PropertyValue(KeyValuePair<string, object> value)
        //{
        //    return new PropertyValue(value.Key, value.Value);
        //}

        #endregion Conversion Operators
    }
}
