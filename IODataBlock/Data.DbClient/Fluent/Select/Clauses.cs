using System.Collections.Generic;
using Data.DbClient.Fluent.Enums;

namespace Data.DbClient.Fluent.Select
{
    public interface IWhereClause
    {
    }

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

    public class HavingClause : FilterClause
    {
        public HavingClause(string field, ComparisonOperatorType compareOperator, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
            : base(field, compareOperator, compareValue, logicalOperatorType)
        {
        }
    }

    public class WhereClause : FilterClause, IWhereClause
    {
        public WhereClause(string field, ComparisonOperatorType compareOperator, object compareValue, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
            : base(field, compareOperator, compareValue, logicalOperatorType)
        {
        }
    }

    public class GroupWhereClause : IWhereClause
    {
        public List<WhereClause> WhereClauses { get; set; }
        public LogicalOperatorType LogicalOperatorType { get; set; }

        public GroupWhereClause(List<WhereClause> whereClauses, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            LogicalOperatorType = logicalOperatorType;
            WhereClauses = whereClauses;
        }
    }

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
    }

    public class OrderClause
    {
        public string Column { get; set; }
        public OrderType Sorting { get; set; }

        public OrderClause(string column, OrderType sorting = OrderType.Ascending)
        {
            Column = column;
            Sorting = sorting;
        }
    }

    public class GroupByClause
    {
        public string Column { get; set; }

        public GroupByClause(string column)
        {
            Column = column;
        }
    }

    public class TopClause
    {
        public int Quantity { get; set; }

        public TopClause(int quantity)
        {
            Quantity = quantity;
        }
    }

    public class LimitClause
    {
        public int Take { get; set; }

        public LimitClause(int take)
        {
            Take = take;
        }
    }

    public class OffsetClause
    {
        public int Skip { get; set; }

        public OffsetClause(int skip)
        {
            Skip = skip;
        }
    }
}