using Business.Common.System;
using Data.Fluent.Base;
using Data.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model
{
    public class Having : ObjectBase<Having>
    {
        public SchemaObject ColumNameOrAggregateFunction { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ComparisonOperatorType ComparisonOperator { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogicalOperatorType LogicalOperatorType { get; set; }

        public object ComparisonValue { get; set; }
    }
}
