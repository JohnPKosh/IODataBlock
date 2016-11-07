using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Model;

namespace Data.Fluent.Select
{
    public abstract class Filter : ObjectBase<Filter>
    {
        protected Filter(string name, ComparisonOperatorType comparisonOperatorType, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            LogicalOperatorType = logicalOperatorType;
            Name = name;
            ComparisonOperator = comparisonOperatorType;
            ComparisonValue = compareValue;
        }
        protected Filter(SchemaObject name, ComparisonOperatorType comparisonOperatorType, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            LogicalOperatorType = logicalOperatorType;
            Name = name;
            ComparisonOperator = comparisonOperatorType;
            ComparisonValue = compareValue;
        }

        public SchemaObject Name { get; set; }
        public ComparisonOperatorType ComparisonOperator { get; set; }
        public LogicalOperatorType LogicalOperatorType { get; set; }
        public object ComparisonValue { get; set; }
        
    }
}