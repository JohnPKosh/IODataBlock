using Data.Fluent.Base;
using Data.Fluent.Enums;
using Newtonsoft.Json;

namespace Data.Fluent.Model.Schema
{
    public class GroupByColumn : SchemaObject
    {
        public GroupByColumn(string value = null, string prefixOrSchema = null, SchemaValueType valueType = SchemaValueType.NamedObject)
        : base(value, prefixOrSchema, null, valueType, SchemaObjectType.Column) { }

        [JsonIgnore]
        private new string Alias { get; set; }

        public static implicit operator GroupByColumn(string value)
        {
            return new GroupByColumn(value, null, SchemaValueType.Preformatted);
        }

        public static implicit operator string(GroupByColumn value)
        {
            return value.Value;
        }
    }
}
