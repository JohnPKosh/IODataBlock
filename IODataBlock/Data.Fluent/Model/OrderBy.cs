using Business.Common.System;
using Data.Fluent.Base;
using Data.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model
{
    public class OrderBy : ObjectBase<OrderBy>
    {
        public OrderBy() { }

        public OrderBy(string column, OrderType sortDirection = OrderType.Ascending, string prefixOrSchema = null, string alias = null, SchemaValueType valueType = SchemaValueType.NamedObject)
        {
            SortDirection = sortDirection;
            Column = new SchemaObject(column, prefixOrSchema, alias, valueType);
        }

        public SchemaObject Column { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType SortDirection { get; set; }
    }
}
