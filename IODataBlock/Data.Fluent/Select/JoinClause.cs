using Data.Fluent.Base;
using Data.Fluent.Enums;
using Data.Fluent.Model;
using Data.Fluent.Model.Schema;

namespace Data.Fluent.Select
{
    public class JoinClause
    {
        public JoinType JoinType { get; set; }
        public string FromTable { get; set; }
        public string FromColumn { get; set; }
        public ComparisonOperatorType ComparisonOperator { get; set; }
        public string ToTable { get; set; }
        public string ToColumn { get; set; }

        public JoinClause(JoinType join, string toTableName, string toColumnName, ComparisonOperatorType comparisonOperator, string fromTableName, string fromColumnName)
        {
            JoinType = join;
            FromTable = fromTableName;
            FromColumn = fromColumnName;
            ComparisonOperator = comparisonOperator;
            ToTable = toTableName;
            ToColumn = toColumnName;
        }

        public static implicit operator Join(JoinClause value)
        {
            return new Join()
            {
                Type = value.JoinType,
                ComparisonOperator = value.ComparisonOperator,
                FromColumn = new JoinColumn(value.FromColumn, null, SchemaValueType.Preformatted),
                FromTable = new JoinTable(value.FromTable, null, null, SchemaValueType.Preformatted),
                ToColumn = new JoinColumn(value.ToColumn, null, SchemaValueType.Preformatted),
                ToTable = new JoinTable(value.ToTable, null, null, SchemaValueType.Preformatted)
            };
        }
    }
}