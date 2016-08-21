using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Business.Common.Extensions
{
    public static class BsonStreamSerialization
    {
        public static void BsonSerialize<T>(this Stream stream, T value, JsonSerializerSettings settings = null) where T : class
        {
            if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
            using (var writer = new BsonWriter(stream))
            {
                var serializer = JsonSerializer.CreateDefault(settings);
                serializer.Serialize(writer, value, typeof(T));
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
        }

        public static void BsonSerialize<T>(this Stream stream, T value, Type type, JsonSerializerSettings settings = null) where T : class
        {
            if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
            using (var writer = new BsonWriter(stream))
            {
                var serializer = JsonSerializer.CreateDefault(settings);
                serializer.Serialize(writer, value, type);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
        }

        public static void BsonSerialize<T>(this Stream stream, T value, params JsonConverter[] converters) where T : class
        {
            if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
            using (var writer = new BsonWriter(stream))
            {
                var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
                var serializer = JsonSerializer.CreateDefault(settings);
                serializer.Serialize(writer, value, typeof(T));
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
        }

        public static void BsonSerialize<T>(this Stream stream, T value, Type type, params JsonConverter[] converters) where T : class
        {
            if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
            using (var writer = new BsonWriter(stream))
            {
                var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
                var serializer = JsonSerializer.CreateDefault(settings);
                serializer.Serialize(writer, value, type);
            }
            if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
        }

        public static Stream BsonSerializeToStream<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            Stream ms = new MemoryStream();
            ms.BsonSerialize(value, settings);
            return ms;
        }

        public static Stream BsonSerializeToStream<T>(this T value, params JsonConverter[] converters) where T : class
        {
            Stream ms = new MemoryStream();
            ms.BsonSerialize(value, converters);
            return ms;
        }

        public static MemoryStream BsonSerializeToMemoryStream<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            var ms = new MemoryStream();
            ms.BsonSerialize(value, settings);
            return ms;
        }

        public static MemoryStream BsonSerializeToMemoryStream<T>(this T value, params JsonConverter[] converters) where T : class
        {
            var ms = new MemoryStream();
            ms.BsonSerialize(value, converters);
            return ms;
        }

        public static FileStream BsonSerializeToFileStream<T>(this T value, string filePath, JsonSerializerSettings settings = null) where T : class
        {
            var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            fs.BsonSerialize(value, settings);
            return fs;
        }

        public static FileStream BsonSerializeToFileStream<T>(this T value, string filePath, params JsonConverter[] converters) where T : class
        {
            var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            fs.BsonSerialize(value, converters);
            return fs;
        }

        public static byte[] BsonSerializeToBytes<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            var ms = new MemoryStream();
            ms.BsonSerialize(value, settings);
            return ms.ToArray();
        }

        public static byte[] BsonSerializeToBytes<T>(this T value, Type type, JsonSerializerSettings settings = null) where T : class
        {
            var ms = new MemoryStream();
            ms.BsonSerialize(value, type, settings);
            return ms.ToArray();
        }

        public static byte[] BsonSerializeToBytes<T>(this T value, params JsonConverter[] converters) where T : class
        {
            var ms = new MemoryStream();
            ms.BsonSerialize(value, converters);
            return ms.ToArray();
        }


        public static byte[] BsonSerializeToBytes<T>(this T value, Type type, params JsonConverter[] converters) where T : class
        {
            var ms = new MemoryStream();
            ms.BsonSerialize(value, type, converters);
            return ms.ToArray();
        }

        /* Not exactly sure the usefulness of below methods but we will just leave for now. */

        public static string BsonSerializeToBase64<T>(this T value, JsonSerializerSettings settings = null) where T : class
        {
            var ms = new MemoryStream();
            ms.BsonSerialize(value, settings);
            return Convert.ToBase64String(ms.ToArray());
        }

        public static string BsonSerializeToBase64<T>(this T value, params JsonConverter[] converters) where T : class
        {
            var ms = new MemoryStream();
            ms.BsonSerialize(value, converters);
            return Convert.ToBase64String(ms.ToArray());
        }
    }
}