using System.Collections.Generic;
using System.Linq;
using Business.Common.System;
using Data.DbClient.Fluent.Select;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

namespace Data.DbClient.Fluent.Model
{
    public class QueryObjectBase : ObjectBase<QueryObjectBase>, IQueryObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> SelectColumns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Top { get; set; }

        public string FromTable { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Join> Joins { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Where> WhereFilters { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<string> GroupBy { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Having> HavingClauses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderBy> OrderByClauses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Skip { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Take { get; set; }

        /* TODO: Update to use IQueryBuilder after refactoring - (JKOSH)  */
        public string GetQuery(IQueryBuilder builder)
        {
            builder = builder.FromTable(FromTable);
            builder = SelectColumns != null ? builder.SelectColumns(SelectColumns) : builder.SelectAllColumns();
            if(Top.HasValue) builder = builder.Top(Top.Value);
            if(Joins != null) builder = Joins.Aggregate(builder, (current, j) => current.Join(j.Type, j.ToTable, j.ToColumn, j.ComparisonOperator, j.FromTable, j.FromColumn));
            if(WhereFilters != null) builder = WhereFilters.Aggregate(builder, (current, w) => current.Where(w.FieldName, w.ComparisonOperator, w.Value, w.LogicalOperatorType));
            if (GroupBy != null) builder = builder.GroupBy(GroupBy);
            if(HavingClauses != null)builder = HavingClauses.Aggregate(builder, (current, h) => current.Having(h.ColumNameOrAggregateFunction, h.ComparisonOperator, h.Value, h.LogicalOperatorType));
            if(OrderByClauses != null) builder = OrderByClauses.Aggregate(builder, (current, o) => current.OrderBy(o.Column, o.SortDirection));
            if (Skip.HasValue) builder = builder.Skip(Skip.Value);
            if (Take.HasValue) builder = builder.Take(Take.Value);

            return builder.BuildQuery();
        }
    }
}
