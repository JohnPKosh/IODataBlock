using Data.Fluent.Base;
using Data.Fluent.Enums;

namespace Data.Fluent.Model.Schema
{
    public class SelectColumn : SchemaObject
    {
        public SelectColumn(string value = null, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject)
            : base(value, prefixOrSchema, alias, valueType, SchemaObjectType.Column){ }

        public static implicit operator SelectColumn(string value)
        {
            return new SelectColumn(value, null, null, SchemaValueType.Preformatted);
        }

        public static implicit operator string(SelectColumn value)
        {
            return value.Value;
        }
    }
}
