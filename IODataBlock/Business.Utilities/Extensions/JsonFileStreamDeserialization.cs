using System.IO;
using Newtonsoft.Json;

namespace Business.Utilities.Extensions
{
    public static class JsonFileStreamDeserialization
    {
        public static T JsonDeserialize<T>(this FileStream stream, JsonSerializerSettings settings = null) where T : class
        {
            return ((Stream)stream).JsonDeserialize<T>(settings);
        }

        public static T JsonDeserialize<T>(this FileStream stream, params JsonConverter[] converters) where T : class
        {
            return ((Stream)stream).JsonDeserialize<T>(converters);
        }
    }
}