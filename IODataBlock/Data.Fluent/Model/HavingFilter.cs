using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Data.Fluent.Model.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model
{
    public class HavingFilter : FilterBase, IWhereFilter
    {
        public HavingFilter() { }
        public HavingFilter(FilterColumn column, ComparisonOperatorType comparisonOperatorType, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or) : base(column, comparisonOperatorType, compareValue, logicalOperatorType)
        {
            LogicalOperatorType = logicalOperatorType;
            Column = column;
            ComparisonOperator = comparisonOperatorType;
            ComparisonValue = compareValue;
        }
    }
}
