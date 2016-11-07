using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Data.Fluent.Model;

namespace Data.Fluent.Select
{
    public class WhereClause : Filter, IWhereClause
    {
        public WhereClause(string field, ComparisonOperatorType compareOperator, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
            : base(field, compareOperator, compareValue, logicalOperatorType)
        {
        }

        public static implicit operator Where(WhereClause value)
        {
            return new Where()
            {
                SchemaObject = new SchemaObject(value.Name, null, null, SchemaValueType.Preformatted),
                ComparisonOperator = value.ComparisonOperator,
                LogicalOperatorType = value.LogicalOperatorType,
                ComparisonValue = value.ComparisonValue
            };
        }
    }
}