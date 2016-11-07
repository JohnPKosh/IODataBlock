using Data.Fluent.Enums;
using Data.Fluent.Model;

namespace Data.Fluent.Select
{
    public class HavingClause : Filter
    {
        public HavingClause(string field, ComparisonOperatorType compareOperator, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
            : base(field, compareOperator, compareValue, logicalOperatorType)
        {
        }

        public static implicit operator Having(HavingClause value)
        {
            return new Having()
            {
                ColumNameOrAggregateFunction = new SchemaObject(value.Name, null, null, SchemaValueType.Preformatted),
                ComparisonOperator = value.ComparisonOperator,
                LogicalOperatorType = value.LogicalOperatorType,
                ComparisonValue = value.ComparisonValue
            };
        }
    }
}