using Business.Common.System;
using Data.Fluent.Base;
using Data.Fluent.Enums;
using Data.Fluent.Model.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model
{
    public class WhereFilter : ObjectBase<WhereFilter>
    {
        public FilterColumn Column { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ComparisonOperatorType ComparisonOperator { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogicalOperatorType LogicalOperatorType { get; set; }

        public object ComparisonValue { get; set; }
    }
}
