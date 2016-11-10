using Data.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Interfaces
{
    public interface ISchemaObject
    {
        string Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string PrefixOrSchema { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        SchemaValueType ValueType { get; set; }

        string ToJson(bool indented = false);
        void PopulateFromJson(string value);
        void PopulateFromJson(string value, JsonSerializerSettings settings);
    }
}