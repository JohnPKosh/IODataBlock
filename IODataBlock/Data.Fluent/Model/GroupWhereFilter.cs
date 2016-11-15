using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;
using Data.Fluent.Select;

namespace Data.Fluent.Model
{
    public class GroupWhereFilter : ObjectBase<GroupWhereFilter>, IWhereClause
    {
        public GroupWhereFilter(List<WhereFilter> whereClauses, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            LogicalOperatorType = logicalOperatorType;
            WhereClauses = whereClauses;
        }

        public List<WhereFilter> WhereClauses { get; set; }
        public LogicalOperatorType LogicalOperatorType { get; set; }

    }
}
