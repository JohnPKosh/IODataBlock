using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Business.Common.System;
using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Extensions;
using Data.DbClient.Fluent.Select;
using Microsoft.SqlServer.Server;
using Newtonsoft.Json;

namespace Data.DbClient.Fluent.Model
{
    public class QueryObjectBase : ObjectBase<QueryObjectBase>, IQueryObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SchemaObject> SelectColumns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Top { get; set; }

        public SchemaObject FromTable { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Join> Joins { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Where> WhereFilters { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SchemaObject> GroupBy { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Having> HavingClauses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderBy> OrderByClauses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Skip { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? Take { get; set; }

        public string GetQuery(IQueryBuilder builder)
        {
            builder = builder.FromTable(FromTable.AsString());
            builder = SelectColumns != null ? builder.SelectColumns(GetSelectedColumnsStringList(SelectColumns)) : builder.SelectAllColumns();
            if(Top.HasValue) builder = builder.Top(Top.Value);
            if(Joins != null) builder = Joins.Aggregate(builder, (current, j) => current.Join(j.Type, j.ToTable.AsString(), j.ToColumn.AsString(), j.ComparisonOperator, j.FromTable.AsString(), j.FromColumn.AsString()));
            if(WhereFilters != null) builder = WhereFilters.Aggregate(builder, (current, w) => current.Where(w.SchemaObject.AsString(), w.ComparisonOperator, w.ComparisonValue, w.LogicalOperatorType));
            if (GroupBy != null) builder = builder.GroupBy(GroupBy.Select(x=> x.AsString()));
            if(HavingClauses != null)builder = HavingClauses.Aggregate(builder, (current, h) => current.Having(h.ColumNameOrAggregateFunction.AsString(), h.ComparisonOperator, h.ComparisonValue, h.LogicalOperatorType));
            if(OrderByClauses != null) builder = OrderByClauses.Aggregate(builder, (current, o) => current.OrderBy(o.Column.AsString(), o.SortDirection));
            if (Skip.HasValue) builder = builder.Skip(Skip.Value);
            if (Take.HasValue) builder = builder.Take(Take.Value);

            return builder.BuildQuery();
        }

        private static IEnumerable<string> GetSelectedColumnsStringList(IEnumerable<SchemaObject> columns, string quotedPrefix = "", string quotedSuffix = "")
        {
            return columns.Select(x => x.AsString(quotedPrefix, quotedSuffix));
        }


    }
}
