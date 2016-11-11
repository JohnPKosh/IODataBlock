using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Base
{
    public class SchemaObject : ObjectBase<ISchemaObject>, ISchemaObject
    {
        public SchemaObject(string value = null, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject, SchemaObjectType objectType = SchemaObjectType.Column)
        {
            Value = value;
            PrefixOrSchema = prefixOrSchema;
            Alias = alias;
            ValueType = valueType;
            ObjectType = objectType;
        }

        public string Value { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string PrefixOrSchema { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SchemaValueType ValueType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SchemaObjectType ObjectType { get; set; }

        public static implicit operator SchemaObject(string value)
        {
            return new SchemaObject(value, null, null, SchemaValueType.Preformatted);
        }

        public static implicit operator string(SchemaObject value)
        {
            return value.Value;
        }
    }
}
