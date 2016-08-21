using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Business.Common.Extensions
{
    public static class BsonObjectFileInfoSerialization
    {
        #region WriteBsonToFilePath

        public static void WriteBsonToFile(this object value, FileInfo file, JsonSerializerSettings settings = null)
        {
            using (var fs = new FileStream(file.FullName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, value);
                }
            }
        }

        public static void WriteBsonToFilePath(this object value, FileInfo file, params JsonConverter[] converters)
        {
            using (var fs = new FileStream(file.FullName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var settings = converters != null && converters.Length > 0 ? new JsonSerializerSettings { Converters = converters } : null;
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, value);
                }
            }
        }

        public static void WriteBsonToFilePath(this object value, FileInfo file, Type type, JsonSerializerSettings settings = null)
        {
            using (var fs = new FileStream(file.FullName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {
                using (var writer = new BsonWriter(fs))
                {
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, value, type);
                }
            }
        }

        public static void WriteBsonToFilePath(this object value, FileInfo file, Type type, params JsonConverter[] converters)
        {
            using (var fs = new FileStream(file.FullName, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
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
