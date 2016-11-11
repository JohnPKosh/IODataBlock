using Business.Common.System;
using Data.Fluent.Base;
using Data.Fluent.Enums;
using Data.Fluent.Model;
using Data.Fluent.Model.Schema;

namespace Data.Fluent.Select
{
    public abstract class Filter : ObjectBase<Filter>
    {

        protected Filter(FilterColumn column, ComparisonOperatorType comparisonOperatorType, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            LogicalOperatorType = logicalOperatorType;
            Column = column;
            ComparisonOperator = comparisonOperatorType;
            ComparisonValue = compareValue;
        }

        public FilterColumn Column { get; set; }
        public ComparisonOperatorType ComparisonOperator { get; set; }
        public LogicalOperatorType LogicalOperatorType { get; set; }
        public object ComparisonValue { get; set; }
        
    }
}