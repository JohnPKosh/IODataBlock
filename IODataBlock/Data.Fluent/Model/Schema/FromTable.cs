using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model.Schema
{
    public class FromTable : ObjectBase<FromTable>, ISchemaObject
    {
        public FromTable(string value = null, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject)
        {
            Value = value;
            PrefixOrSchema = prefixOrSchema;
            Alias = alias;
            ValueType = valueType;
        }

        public string Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrefixOrSchema { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SchemaValueType ValueType { get; set; }

        public static implicit operator FromTable(string value)
        {
            return new FromTable(value, null, null, SchemaValueType.Preformatted);
        }

        public static implicit operator string(FromTable value)
        {
            return value.Value;
        }
    }
}
