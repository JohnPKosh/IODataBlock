using System;
using System.IO;
using Newtonsoft.Json;

namespace Data.DbClient.Extensions
{
    public static class DbClientJsonExtensions
    {
        public static void JsonDbQuery(this Stream stream, string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonSerializerSettings settings = null, params object[] parameters)
        {
            var provider = String.IsNullOrWhiteSpace(providerName) ? "System.Data.SqlClient" : providerName;
            using (var db = Database.OpenConnectionString(connectionString, provider))
            {
                if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
                var sw = new StreamWriter(stream);
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, db.QueryToJObjects(commandText, commandTimeout, parameters));
                }
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            }
        }

        public static void JsonDbQuery(this Stream stream, string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonConverter[] converters = null, params object[] parameters)
        {
            var provider = String.IsNullOrWhiteSpace(providerName) ? "System.Data.SqlClient" : providerName;
            using (var db = Database.OpenConnectionString(connectionString, provider))
            {
                if (stream.CanWrite) stream.SetLength(0);  // set length back to 0 on serialization.
                var sw = new StreamWriter(stream);
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    var settings = (converters != null && converters.Length > 0) ? new JsonSerializerSettings { Converters = converters } : null;
                    var serializer = JsonSerializer.CreateDefault(settings);
                    serializer.Serialize(writer, db.QueryToJObjects(commandText, commandTimeout, parameters));
                }
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            }
        }

        public static Stream JsonDbQueryToStream(string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonSerializerSettings settings = null, params object[] parameters)
        {
            Stream ms = new MemoryStream();
            ms.JsonDbQuery(commandText, connectionString, providerName, commandTimeout, settings: settings, parameters: parameters);
            return ms;
        }

        public static Stream JsonDbQueryToStream(string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonConverter[] converters = null, params object[] parameters)
        {
            Stream ms = new MemoryStream();
            ms.JsonDbQuery(commandText, connectionString, providerName, commandTimeout, converters: converters, parameters: parameters);
            return ms;
        }

        public static MemoryStream JsonDbQueryToMemoryStream(string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonSerializerSettings settings = null, params object[] parameters)
        {
            var ms = new MemoryStream();
            ms.JsonDbQuery(commandText, connectionString, providerName, commandTimeout, settings: settings, parameters: parameters);
            return ms;
        }

        public static MemoryStream JsonDbQueryToMemoryStream(string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonConverter[] converters = null, params object[] parameters)
        {
            var ms = new MemoryStream();
            ms.JsonDbQuery(commandText, connectionString, providerName, commandTimeout, converters: converters, parameters: parameters);
            return ms;
        }

        public static FileStream JsonDbQueryToFileStream(string filePath, string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonSerializerSettings settings = null, params object[] parameters)
        {
            var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            fs.JsonDbQuery(commandText, connectionString, providerName, commandTimeout, settings: settings, parameters: parameters);
            return fs;
        }

        public static FileStream JsonDbQueryToFileStream(string filePath, string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonConverter[] converters = null, params object[] parameters)
        {
            var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None);
            fs.JsonDbQuery(commandText, connectionString, providerName, commandTimeout, converters: converters, parameters: parameters);
            return fs;
        }

        public static Byte[] JsonDbQueryToToBytes(string filePath, string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonSerializerSettings settings = null, params object[] parameters)
        {
            var ms = new MemoryStream();
            ms.JsonDbQuery(commandText, connectionString, providerName, commandTimeout, settings: settings, parameters: parameters);
            return ms.ToArray();
        }

        public static Byte[] JsonDbQueryToToBytes(string filePath, string commandText, string connectionString, string providerName = null, int commandTimeout = 60, JsonConverter[] converters = null, params object[] parameters)
        {
            var ms = new MemoryStream();
            ms.JsonDbQuery(commandText, connectionString, providerName, commandTimeout, converters: converters, parameters: parameters);
            return ms.ToArray();
        }
    }
}