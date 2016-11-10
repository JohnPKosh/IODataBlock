using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Extensions;
using Data.Fluent.Interfaces;
using Data.Fluent.Model;
using Newtonsoft.Json;

namespace Data.Fluent.Base
{
    public class QueryObjectBase : ObjectBase<QueryObjectBase>, IQueryObject
    {
        #region *** Properties ***

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public SqlLanguageType LanguageType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SchemaObject> SelectColumns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TopValue { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
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

        #endregion

        #region *** SELECT Fluent Methods ***

        public IQueryObject SelectAll()
        {
            if(SelectColumns == null)SelectColumns = new List<SchemaObject>();
            SelectColumns.Add(new SchemaObject() {Value = "*", ValueType = SchemaValueType.Preformatted});
            return this;
        }

        public IQueryObject Top(int quantity)
        {
            TopValue = quantity;
            return this;
        }

        public IQueryObject Select(params string[] columns)
        {
            return Select(columns, SchemaValueType.Preformatted, false);
        }

        public IQueryObject Select(string columnsString, SchemaValueType valueType = SchemaValueType.Preformatted, bool clearExisting = false)
        {
            return Select(columnsString.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), valueType, clearExisting);
        }

        public IQueryObject Select(IEnumerable<string> columns, SchemaValueType valueType = SchemaValueType.Preformatted, bool clearExisting = false)
        {
            return Select(columns.Select(x => new SchemaObject() {Value = x, ValueType = valueType }));
        }

        public IQueryObject Select(IEnumerable<SchemaObject> columns, bool clearExisting = false)
        {
            if (clearExisting) SelectColumns.Clear();
            foreach (var column in columns)
            {
                SelectColumns.Add(column);
            }
            return this;
        }

        #endregion

        #region *** FROM Fluent Methods ***

        public IQueryObject From(string table, SchemaValueType valueType = SchemaValueType.Preformatted)
        {
            return From(new SchemaObject() {Value = table, ValueType = valueType});
        }
        public IQueryObject From(SchemaObject table)
        {
            FromTable = table;
            return this;
        }

        #endregion

        #region *** Utility Methods ***

        public string GetQuery(IQueryBuilder builder)
        {
            builder = builder.FromTable(FromTable.AsString());
            builder = SelectColumns != null ? builder.SelectColumns(GetSelectedColumnsStringList(SelectColumns)) : builder.SelectAllColumns();
            if (TopValue.HasValue) builder = builder.Top(TopValue.Value);
            if (Joins != null) builder = Joins.Aggregate(builder, (current, j) => current.Join(j.Type, j.ToTable.AsString(), j.ToColumn.AsString(), j.ComparisonOperator, j.FromTable.AsString(), j.FromColumn.AsString()));
            if (WhereFilters != null) builder = WhereFilters.Aggregate(builder, (current, w) => current.Where(w.SchemaObject.AsString(), w.ComparisonOperator, w.ComparisonValue, w.LogicalOperatorType));
            if (GroupBy != null) builder = builder.GroupBy(GroupBy.Select(x => x.AsString()));
            if (HavingClauses != null) builder = HavingClauses.Aggregate(builder, (current, h) => current.Having(h.ColumNameOrAggregateFunction.AsString(), h.ComparisonOperator, h.ComparisonValue, h.LogicalOperatorType));
            if (OrderByClauses != null) builder = OrderByClauses.Aggregate(builder, (current, o) => current.OrderBy(o.Column.AsString(), o.SortDirection));
            if (Skip.HasValue) builder = builder.Skip(Skip.Value);
            if (Take.HasValue) builder = builder.Take(Take.Value);

            return builder.BuildQuery();
        }

        private static IEnumerable<string> GetSelectedColumnsStringList(IEnumerable<SchemaObject> columns, string quotedPrefix = "", string quotedSuffix = "")
        {
            return columns.Select(x => x.AsString(quotedPrefix, quotedSuffix));
        }

        #endregion


    }
}
