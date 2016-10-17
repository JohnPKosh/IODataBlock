using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Model;

namespace Data.DbClient.Fluent.Select
{
    public class HavingClause : FilterClause
    {
        public HavingClause(string field, ComparisonOperatorType compareOperator, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
            : base(field, compareOperator, compareValue, logicalOperatorType)
        {
        }

        public static implicit operator Having(HavingClause value)
        {
            return new Having()
            {
                ColumNameOrAggregateFunction = new SchemaObject(value.FieldName, null, null, SchemaValueType.Preformatted),
                ComparisonOperator = value.ComparisonOperator,
                LogicalOperatorType = value.LogicalOperatorType,
                ComparisonValue = value.Value
            };
        }
    }
}