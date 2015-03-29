using System;
using System.IO;
using Newtonsoft.Json;

namespace Business.Utilities.Extensions
{
    public static class JsonStreamDeserialization
    {
        public static T JsonDeserialize<T>(this Stream stream, JsonSerializerSettings settings = null) where T : class
        {
            T returnvalue;
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            var sr = new StreamReader(stream);
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var serializer = JsonSerializer.CreateDefault(settings);
                returnvalue = serializer.Deserialize<T>(reader);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            return returnvalue;
        }

        public static T JsonDeserialize<T>(this Stream stream, params JsonConverter[] converters) where T : class
        {
            T returnvalue;
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // set Stream to beginning.
            var sr = new StreamReader(stream);
            using (JsonReader reader = new JsonTextReader(sr))
            {
                var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
                var serializer = JsonSerializer.CreateDefault(settings);
                returnvalue = serializer.Deserialize<T>(reader);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            return returnvalue;
        }

        public static T JsonDeserialize<T>(this MemoryStream stream, JsonSerializerSettings settings = null) where T : class
        {
            return ((Stream)stream).JsonDeserialize<T>(settings);
        }

        public static T JsonDeserialize<T>(this MemoryStream stream, params JsonConverter[] converters) where T : class
        {
            return ((Stream)stream).JsonDeserialize<T>(converters);
        }

        public static T JsonDeserialize<T>(this FileStream stream, JsonSerializerSettings settings = null) where T : class
        {
            return ((Stream)stream).JsonDeserialize<T>(settings);
        }

        public static T JsonDeserialize<T>(this FileStream stream, params JsonConverter[] converters) where T : class
        {
            return ((Stream)stream).JsonDeserialize<T>(converters);
        }

        public static T JsonDeserializeBytes<T>(this Byte[] data, JsonSerializerSettings settings = null) where T : class
        {
            using (var ms = new MemoryStream(data))
            {
                return ms.JsonDeserialize<T>(settings);
            }
        }

        public static T JsonDeserializeBytes<T>(this Byte[] data, params JsonConverter[] converters) where T : class
        {
            using (var ms = new MemoryStream(data))
            {
                return ms.JsonDeserialize<T>(converters);
            }
        }

        /* Not exactly sure the usefulness of below methods but we will just leave for now. */

        public static T JsonDeserializeBase64String<T>(this String value, JsonSerializerSettings settings = null) where T : class
        {
            return Convert.FromBase64String(value).JsonDeserializeBytes<T>(settings);
        }

        public static T JsonDeserializeBase64String<T>(this String value, params JsonConverter[] converters) where T : class
        {
            return Convert.FromBase64String(value).JsonDeserializeBytes<T>(converters);
        }
    }
}