using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;

namespace Business.Common.Extensions
{
    public static class BsonObjectFileInfoDeserialization
    {
        public static T BsonDeserialize<T>(this FileInfo file, JsonSerializerSettings settings = null) where T : class
        {
            using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ((Stream)fs).BsonDeserialize<T>(settings);
            }
        }

        public static T BsonDeserialize<T>(this FileInfo file, params JsonConverter[] converters) where T : class
        {
            using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return ((Stream)fs).BsonDeserialize<T>(converters);
            }
        }
    }
}
