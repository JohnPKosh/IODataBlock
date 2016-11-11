using Business.Common.System;
using Data.Fluent.Base;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model.Schema
{
    public class JoinTable : SchemaObject
    {
        public JoinTable(string value = null, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject)
        : base(value, prefixOrSchema, alias, valueType, SchemaObjectType.Table) { }

        public static implicit operator JoinTable(string value)
        {
            return new JoinTable(value, null, null, SchemaValueType.Preformatted);
        }

        public static implicit operator string(JoinTable value)
        {
            return value.Value;
        }
    }
}
