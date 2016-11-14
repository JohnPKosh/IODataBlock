using Data.Fluent.Base;
using Data.Fluent.Enums;
using Newtonsoft.Json;

namespace Data.Fluent.Model.Schema
{
    public class FilterColumn : SchemaObject
    {
        public FilterColumn(string value = null, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject)
            : base(value, prefixOrSchema, alias, valueType, SchemaObjectType.Column) { }

        [JsonIgnore]
        private new string Alias { get; set; }

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
