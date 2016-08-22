using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace Business.Common.Extensions
{
    public static class BsonObjectBytesSerialization
    {
        #region Json.net object Serialization

        public static byte[] ToBsonBytes(this object value)
        {
            return value.BsonSerializeToBytes(new ExpandoObjectConverter(), new StringEnumConverter());
        }

        public static byte[] ToBsonBytes(this object value, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            var settings = new JsonSerializerSettings { Converters = converterList.ToArray() };
            return value.BsonSerializeToBytes(settings);
        }

        public static byte[] ToBsonBytes(this object value, Type type, params JsonConverter[] converters)
        {
            var converterList = new List<JsonConverter>(converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            var settings = new JsonSerializerSettings { Converters = converterList.ToArray() };
            return value.BsonSerializeToBytes(type, settings);
        }

        public static byte[] ToBsonBytes(this object value, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return value.BsonSerializeToBytes(settings);
        }

        public static byte[] ToBsonBytes(this object value, Type type, JsonSerializerSettings settings)
        {
            var converterList = new List<JsonConverter>(settings.Converters)
            {
                new ExpandoObjectConverter(),
                new StringEnumConverter()
            };
            settings.Converters = converterList.ToArray();
            return value.BsonSerializeToBytes(type, settings);
        }

        #endregion Json.net object Serialization
    }
}