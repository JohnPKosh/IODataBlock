using Data.Fluent.Base;
using Data.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model.Schema
{
    public class OrderByColumn : SchemaObject
    {
        public OrderByColumn(string value = null, string prefixOrSchema = null, SchemaValueType valueType = SchemaValueType.NamedObject)
        : base(value, prefixOrSchema, null, valueType, SchemaObjectType.Column) { }

        [JsonIgnore]
        private new string Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType SortDirection { get; set; }

        public static implicit operator OrderByColumn(string value)
        {
            return new OrderByColumn(value, null, SchemaValueType.Preformatted);
        }

        public static implicit operator string(OrderByColumn value)
        {
            return value.Value;
        }
    }
}
