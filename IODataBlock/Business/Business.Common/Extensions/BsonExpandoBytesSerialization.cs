using System;
using System.Collections.Generic;
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Business.Common.Extensions
{
    public static class BsonExpandoBytesSerialization
    {
        #region Json.net ExpandoObject Serialization

        public static byte[] ToBsonBytes(this ExpandoObject value)
        {
            return value.BsonSerializeToBytes(new ExpandoObjectConverter(), new StringEnumConverter());
        }

        public static byte[] ToBsonBytes(this ExpandoObject value, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            var settings = new JsonSerializerSettings { Converters = converterList.ToArray() };
            return value.BsonSerializeToBytes(settings);
        }

        public static byte[] ToBsonBytes(this ExpandoObject value, Type type, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            var settings = new JsonSerializerSettings { Converters = converterList.ToArray() };
            return value.BsonSerializeToBytes(type, settings);
        }

        public static byte[] ToBsonBytes(this ExpandoObject value, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return value.BsonSerializeToBytes(settings);
        }

        public static byte[] ToBsonBytes(this ExpandoObject value, Type type, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return value.BsonSerializeToBytes(type, settings);
        }

        #endregion Json.net ExpandoObject Serialization
    }
}