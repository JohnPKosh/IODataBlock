using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Data.Fluent.Model.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model
{
    public class FilterBase : ObjectBase<HavingFilter>, IWhereFilter
    {
        protected FilterBase() { }

        protected FilterBase(FilterColumn column, ComparisonOperatorType comparisonOperatorType, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            LogicalOperatorType = logicalOperatorType;
            Column = column;
            ComparisonOperator = comparisonOperatorType;
            ComparisonValue = compareValue;
        }

        public FilterColumn Column { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ComparisonOperatorType ComparisonOperator { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogicalOperatorType LogicalOperatorType { get; set; }

        public object ComparisonValue { get; set; }
    }
}