using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Business.Common.Extensions
{
    public static class JsonExpandoStringSerialization
    {
        #region Json.net ExpandoObject Serialization

        public static string ToJsonString(this ExpandoObject value)
        {
            return JsonConvert.SerializeObject(value, new ExpandoObjectConverter(), new StringEnumConverter());
        }

        public static string ToJsonString(this ExpandoObject value, bool indented)
        {
            return JsonConvert.SerializeObject(value, indented ? Formatting.Indented : Formatting.None, new ExpandoObjectConverter(), new StringEnumConverter());
        }

        public static string ToJsonString(this ExpandoObject value, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            var settings = new JsonSerializerSettings { Converters = converterList.ToArray() };
            return JsonConvert.SerializeObject(value, null, settings);
        }

        public static string ToJsonString(this ExpandoObject value, bool indented, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            var settings = new JsonSerializerSettings { Converters = converterList.ToArray() };
            return JsonConvert.SerializeObject(value, null, indented ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJsonString(this ExpandoObject value, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return JsonConvert.SerializeObject(value, null, settings);
        }

        public static string ToJsonString(this ExpandoObject value, Type type, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return JsonConvert.SerializeObject(value, type, settings);
        }

        public static string ToJsonString(this ExpandoObject value, bool indented, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return JsonConvert.SerializeObject(value, null, indented ? Formatting.Indented : Formatting.None, settings);
        }

        public static string ToJsonString(this ExpandoObject value, Type type, bool indented, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return JsonConvert.SerializeObject(value, type, indented ? Formatting.Indented : Formatting.None, settings);
        }

        #endregion Json.net ExpandoObject Serialization
    }
}