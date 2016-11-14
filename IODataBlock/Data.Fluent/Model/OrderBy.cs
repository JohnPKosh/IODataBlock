using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Model.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model
{
    public class OrderBy : ObjectBase<OrderBy>
    {
        public OrderBy() { }

        public OrderBy(string column, OrderType sortDirection = OrderType.Ascending, string prefixOrSchema = null, SchemaValueType valueType = SchemaValueType.NamedObject)
        {
            SortDirection = sortDirection;
            Column = new OrderByColumn(column, prefixOrSchema, valueType);
        }

        public OrderByColumn Column { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType SortDirection { get; set; }
    }
}
