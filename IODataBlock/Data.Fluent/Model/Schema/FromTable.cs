using Data.Fluent.Base;
using Data.Fluent.Enums;

namespace Data.Fluent.Model.Schema
{
    public class FromTable : SchemaObject
    {
        public FromTable(string value = null, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject)
        : base(value, prefixOrSchema, alias, valueType, SchemaObjectType.Table) { }

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
