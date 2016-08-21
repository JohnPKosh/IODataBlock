using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Business.Common.Extensions
{
    public static class BsonStreamDeserialization
    {
        public static T BsonDeserialize<T>(this Stream stream, JsonSerializerSettings settings = null) where T : class
        {
            T returnvalue;
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            using (var reader = new BsonReader(stream))
            {
                var serializer = JsonSerializer.CreateDefault(settings);
                returnvalue = serializer.Deserialize<T>(reader);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            return returnvalue;
        }

        public static T BsonDeserialize<T>(this Stream stream, params JsonConverter[] converters) where T : class
        {
            T returnvalue;
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            using (var reader = new BsonReader(stream))
            {
                var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
                var serializer = JsonSerializer.CreateDefault(settings);
                returnvalue = serializer.Deserialize<T>(reader);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            return returnvalue;
        }

        public static T BsonDeserialize<T>(this MemoryStream stream, JsonSerializerSettings settings = null) where T : class
        {
            return ((Stream)stream).BsonDeserialize<T>(settings);
        }

        public static T BsonDeserialize<T>(this MemoryStream stream, params JsonConverter[] converters) where T : class
        {
            return ((Stream)stream).BsonDeserialize<T>(converters);
        }

        public static T BsonDeserialize<T>(this FileStream stream, JsonSerializerSettings settings = null) where T : class
        {
            return ((Stream)stream).BsonDeserialize<T>(settings);
        }

        public static T BsonDeserialize<T>(this FileStream stream, params JsonConverter[] converters) where T : class
        {
            return ((Stream)stream).BsonDeserialize<T>(converters);
        }

        public static T BsonDeserializeBytes<T>(this byte[] data, JsonSerializerSettings settings = null) where T : class
        {
            using (var ms = new MemoryStream(data))
            {
                return ms.BsonDeserialize<T>(settings);
            }
        }

        public static T BsonDeserializeBytes<T>(this byte[] data, params JsonConverter[] converters) where T : class
        {
            using (var ms = new MemoryStream(data))
            {
                return ms.BsonDeserialize<T>(converters);
            }
        }

        /* Not exactly sure the usefulness of below methods but we will just leave for now. */

        public static T BsonDeserializeBase64String<T>(this string value, JsonSerializerSettings settings = null) where T : class
        {
            return Convert.FromBase64String(value).BsonDeserializeBytes<T>(settings);
        }

        public static T BsonDeserializeBase64String<T>(this string value, params JsonConverter[] converters) where T : class
        {
            return Convert.FromBase64String(value).BsonDeserializeBytes<T>(converters);
        }
    }
}