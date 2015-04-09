using System;
using Newtonsoft.Json;

namespace Business.Utilities.Extensions
{
    public static class JsonObjectStringSerialization
    {
        #region Json.net object Serialization

        public static string ToJsonString(this object value)
        {
            return JsonConvert.SerializeObject(value);
        }

        public static string ToJsonString(this object value, bool indented)
        {
            return JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None);
        }

        public static string ToJsonString(this object value, params JsonConverter[] converters)
        {
            var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
            return JsonConvert.SerializeObject(value, null, settings);
        }

        public static string ToJsonString(this object value, bool indented, params JsonConverter[] converters)
        {
            var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
            return JsonConvert.SerializeObject(value, null, indented ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJsonString(this object value, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, null, settings);
        }

        public static string ToJsonString(this object value, Type type, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, type, settings);
        }

        public static string ToJsonString(this object value, bool indented, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, null, indented ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJsonString(this object value, Type type, bool indented, JsonSerializerSettings settings)
        {
            return JsonConvert.SerializeObject(value, type, indented ? Formatting.Indented : Formatting.None, settings);
        }

        #endregion Json.net object Serialization
    }
}