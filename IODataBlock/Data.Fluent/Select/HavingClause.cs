using Data.Fluent.Base;
using Data.Fluent.Enums;
using Data.Fluent.Model;
using Data.Fluent.Model.Schema;

namespace Data.Fluent.Select
{
    public class HavingClause : Filter
    {
        public HavingClause(string field, ComparisonOperatorType compareOperator, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
            : base(field, compareOperator, compareValue, logicalOperatorType)
        {
        }

        public static implicit operator HavingFilter(HavingClause value)
        {
            return new HavingFilter()
            {
                Column = new FilterColumn(value.Column, null, SchemaValueType.Preformatted),
                ComparisonOperator = value.ComparisonOperator,
                LogicalOperatorType = value.LogicalOperatorType,
                ComparisonValue = value.ComparisonValue
            };
        }
    }
}