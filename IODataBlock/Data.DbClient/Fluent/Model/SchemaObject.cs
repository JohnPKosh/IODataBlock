using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.System;
using Data.DbClient.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.DbClient.Fluent.Model
{
    public class SchemaObject : ObjectBase<SchemaObject>
    {
        public SchemaObject(string value = null, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject)
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
    }
}
