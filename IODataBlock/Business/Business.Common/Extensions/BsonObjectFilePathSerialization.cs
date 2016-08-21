using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Business.Common.Extensions
{
    public static class BsonObjectFilePathSerialization
    {
        #region WriteBsonToFilePath

        public static void WriteBsonToFilePath(this object value, string filePath, JsonSerializerSettings settings = null)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, value);
                }
            }
        }

        public static void WriteBsonToFilePath(this object value, string filePath, params JsonConverter[] converters)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, value);
                }
            }
        }

        public static void WriteBsonToFilePath(this object value, string filePath, Type type, JsonSerializerSettings settings = null)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, value, type);
                }
            }
        }

        public static void WriteBsonToFilePath(this object value, string filePath, Type type, params JsonConverter[] converters)
        {
            using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, value, type);
                }
            }
        }

        #endregion WriteBsonToFilePath
    }
}