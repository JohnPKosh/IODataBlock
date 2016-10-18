using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Interfaces;
using Data.DbClient.Fluent.Model;

namespace Data.DbClient.Fluent.Select
{
    public class WhereClause : FilterClause, IWhereClause
    {
        public WhereClause(string field, ComparisonOperatorType compareOperator, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
            : base(field, compareOperator, compareValue, logicalOperatorType)
        {
        }

        public static implicit operator Where(WhereClause value)
        {
            return new Where()
            {
                SchemaObject = new SchemaObject(value.FieldName, null, null, SchemaValueType.Preformatted),
                ComparisonOperator = value.ComparisonOperator,
                LogicalOperatorType = value.LogicalOperatorType,
                ComparisonValue = value.Value
            };
        }
    }
}