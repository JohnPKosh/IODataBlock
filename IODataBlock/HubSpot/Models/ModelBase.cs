using Business.Common.Extensions;
using Business.Common.System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HubSpot.Models
{
    public class ModelBase<T> : IObjectBase<T>
    {
        #region Json Serialization

        public string ToJson(bool indented = false)
        {
            return JsonConvert.SerializeObject(this, indented ? Formatting.Indented : Formatting.None);
        }

        #endregion Json Serialization

        #region Json Deserialization

        public void PopulateFromJson(string value)
        {
            this.PopulateObjectFromJson(value);
        }

        public void PopulateFromJson(string value, JsonSerializerSettings settings)
        {
            this.PopulateObjectFromJson(value, settings);
        }

        public static T CreateFromJson(string value)
        {
            return ClassExtensions.CreateFromJson<T>(value);
        }

        #endregion Json Deserialization

        #region Conversion Operators

        public static implicit operator JObject(ModelBase<T> value)
        {
            return value.ToJObject();
        }

        #endregion Conversion Operators
    }
}