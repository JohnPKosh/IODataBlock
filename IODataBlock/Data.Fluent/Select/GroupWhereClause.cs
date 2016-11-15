using System.Collections.Generic;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;

namespace Data.Fluent.Select
{
    public class GroupWhereClause : IWhereFilter
    {
        public GroupWhereClause(List<WhereClause> whereClauses, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            LogicalOperatorType = logicalOperatorType;
            WhereClauses = whereClauses;
        }

        public List<WhereClause> WhereClauses { get; set; }
        public LogicalOperatorType LogicalOperatorType { get; set; }

    }
}