using System;
using System.IO;
using Newtonsoft.Json;

namespace Business.Utilities.Extensions
{
    public static class JsonStreamSerialization
    {
        public static void JsonSerialize<T>(this Stream stream, T value, JsonSerializerSettings settings = null) where T : class
        {
            if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
            var sw = new StreamWriter(stream);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var serializer = JsonSerializer.CreateDefault(settings);
                serializer.Serialize(writer,value,typeof(T));
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
        }

        public static void JsonSerialize<T>(this Stream stream, T value, params JsonConverter[] converters) where T : class
        {
            if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
            var sw = new StreamWriter(stream);
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
                var serializer = JsonSerializer.CreateDefault(settings);
                serializer.Serialize(writer, value, typeof(T));
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
        }

        public static Stream JsonSerializeToStream<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            Stream ms = new MemoryStream();
            ms.JsonSerialize(value, settings);
            return ms;
        }

        public static Stream JsonSerializeToStream<T>(this T value, params JsonConverter[] converters) where T : class
        {
            Stream ms = new MemoryStream();
            ms.JsonSerialize(value, converters);
            return ms;
        }

        public static MemoryStream JsonSerializeToMemoryStream<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            var ms = new MemoryStream();
            ms.JsonSerialize(value, settings);
            return ms;
        }

        public static MemoryStream JsonSerializeToMemoryStream<T>(this T value, params JsonConverter[] converters) where T : class
        {
            var ms = new MemoryStream();
            ms.JsonSerialize(value, converters);
            return ms;
        }

        public static Byte[] JsonSerializeToBytes<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            var ms = new MemoryStream();
            ms.JsonSerialize(value, settings);
            return ms.ToArray();
        }

        public static Byte[] JsonSerializeToBytes<T>(this T value, params JsonConverter[] converters) where T : class
        {
            var ms = new MemoryStream();
            ms.JsonSerialize(value, converters);
            return ms.ToArray();
        }

        public static String JsonSerializeToBase64<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            var ms = new MemoryStream();
            ms.JsonSerialize(value, settings);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static String JsonSerializeToBase64<T>(this T value, params JsonConverter[] converters) where T : class
        {
            var ms = new MemoryStream();
            ms.JsonSerialize(value, converters);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}