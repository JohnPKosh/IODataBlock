using Business.Common.System;
using Data.DbClient.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.DbClient.Fluent.Model
{
    public class OrderBy : ObjectBase<OrderBy>
    {
        public string Column { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public OrderType SortDirection { get; set; }
    }
}
