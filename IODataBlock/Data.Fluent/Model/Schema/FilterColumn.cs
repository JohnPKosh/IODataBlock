using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model.Schema
{
    public class FilterColumn : ObjectBase<FilterColumn>, ISchemaObject
    {
        public FilterColumn(string value = null, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject)
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

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SchemaObjectType ObjectType { get; set; }

        public static implicit operator FilterColumn(string value)
        {
            return new FilterColumn(value, null, null, SchemaValueType.Preformatted);
        }

        public static implicit operator string(FilterColumn value)
        {
            return value.Value;
        }
    }
}
