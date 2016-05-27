using System.Collections.Generic;

namespace Data.DbClient.Fluent.Select
{
    internal interface IWhereClause
    {
    }

    internal abstract class FilterClause
    {
        public string FieldName { get; set; }
        public Comparison ComparisonOperator { get; set; }
        public LogicOperator LogicOperator { get; set; }
        public object Value { get; set; }

        protected FilterClause(string field, Comparison compareOperator, object compareValue, LogicOperator logicOperator = LogicOperator.Or)
        {
            LogicOperator = logicOperator;
            FieldName = field;
            ComparisonOperator = compareOperator;
            Value = compareValue;
        }
    }

    internal class HavingClause : FilterClause
    {
        public HavingClause(string field, Comparison compareOperator, object compareValue, LogicOperator logicOperator = LogicOperator.Or)
            : base(field, compareOperator, compareValue, logicOperator)
        {
        }
    }

    internal class WhereClause : FilterClause, IWhereClause
    {
        public WhereClause(string field, Comparison compareOperator, object compareValue, LogicOperator logicOperator = LogicOperator.Or)
            : base(field, compareOperator, compareValue, logicOperator)
        {
        }
    }

    internal class GroupWhereClause : IWhereClause
    {
        public List<WhereClause> WhereClauses { get; set; }
        public LogicOperator LogicOperator { get; set; }

        public GroupWhereClause(List<WhereClause> whereClauses, LogicOperator logicOperator = LogicOperator.Or)
        {
            LogicOperator = logicOperator;
            WhereClauses = whereClauses;
        }
    }

    internal class JoinClause
    {
        public JoinType JoinType { get; set; }
        public string FromTable { get; set; }
        public string FromColumn { get; set; }
        public Comparison ComparisonOperator { get; set; }
        public string ToTable { get; set; }
        public string ToColumn { get; set; }

        public JoinClause(JoinType join, string toTableName, string toColumnName, Comparison comparison, string fromTableName, string fromColumnName)
        {
            JoinType = join;
            FromTable = fromTableName;
            FromColumn = fromColumnName;
            ComparisonOperator = comparison;
            ToTable = toTableName;
            ToColumn = toColumnName;
        }
    }

    internal class OrderClause
    {
        public string Column { get; set; }
        public Order Sorting { get; set; }

        public OrderClause(string column, Order sorting = Order.Ascending)
        {
            Column = column;
            Sorting = sorting;
        }
    }

    internal class GroupByClause
    {
        public string Column { get; set; }

        public GroupByClause(string column)
        {
            Column = column;
        }
    }

    internal class TopClause
    {
        public int Quantity { get; set; }

        public TopClause(int quantity)
        {
            Quantity = quantity;
        }
    }
}