using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Data.Fluent.Model;
using Data.Fluent.Model.Schema;

namespace Data.Fluent.Select
{
    public class WhereClause : Filter, IWhereClause
    {
        public WhereClause(string field, ComparisonOperatorType compareOperator, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
            : base(field, compareOperator, compareValue, logicalOperatorType)
        {
        }

        public static implicit operator WhereFilter(WhereClause value)
        {
            return new WhereFilter()
            {
                Column = new FilterColumn(value.Column, null, SchemaValueType.Preformatted),
                ComparisonOperator = value.ComparisonOperator,
                LogicalOperatorType = value.LogicalOperatorType,
                ComparisonValue = value.ComparisonValue
            };
        }
    }
}