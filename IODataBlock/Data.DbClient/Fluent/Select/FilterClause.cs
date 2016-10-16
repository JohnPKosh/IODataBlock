using Data.DbClient.Fluent.Enums;

namespace Data.DbClient.Fluent.Select
{
    public abstract class FilterClause
    {
        public string FieldName { get; set; }
        public ComparisonOperatorType ComparisonOperator { get; set; }
        public LogicalOperatorType LogicalOperatorType { get; set; }
        public object Value { get; set; }

        protected FilterClause(string field, ComparisonOperatorType comparisonOperatorType, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            LogicalOperatorType = logicalOperatorType;
            FieldName = field;
            ComparisonOperator = comparisonOperatorType;
            Value = compareValue;
        }
    }
}