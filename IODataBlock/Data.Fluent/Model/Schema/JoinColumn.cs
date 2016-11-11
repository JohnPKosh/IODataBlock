using Business.Common.System;
using Data.Fluent.Base;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model.Schema
{
    public class JoinColumn : SchemaObject
    {
        public JoinColumn(string value = null, string prefixOrSchema = null, SchemaValueType valueType = SchemaValueType.NamedObject)
        : base(value, prefixOrSchema, null, valueType, SchemaObjectType.Column) { }

        [JsonIgnore]
        private new string Alias { get; set; }


        public static implicit operator JoinColumn(string value)
        {
            return new JoinColumn(value, null, SchemaValueType.Preformatted);
        }

        public static implicit operator string(JoinColumn value)
        {
            return value.Value;
        }
    }
}
