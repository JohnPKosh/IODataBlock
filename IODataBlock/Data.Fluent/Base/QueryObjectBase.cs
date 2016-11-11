using System;
using System.Collections.Generic;
using System.Linq;
using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Extensions;
using Data.Fluent.Interfaces;
using Data.Fluent.Model;
using Data.Fluent.Model.Schema;
using Newtonsoft.Json;

namespace Data.Fluent.Base
{
    public class QueryObjectBase : ObjectBase<QueryObjectBase>, IQueryObject
    {
        #region *** Properties ***

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        public SqlLanguageType LanguageType { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<SelectColumn> SelectColumns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TopValue { get; set; }

        [JsonProperty(Required = Required.DisallowNull)]
        public FromTable FromTable { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<Join> Joins { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<WhereFilter> WhereFilters { get; set; }

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
            if(SelectColumns == null)SelectColumns = new List<SelectColumn>();
            SelectColumns.Add(new SelectColumn() {Value = "*", ValueType = SchemaValueType.Preformatted});
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
            return Select(columns.Select(x => new SelectColumn() {Value = x, ValueType = valueType }));
        }

        public IQueryObject Select(IEnumerable<SelectColumn> columns, bool clearExisting = false)
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

        public IQueryObject From(FromTable table)
        {
            FromTable = table;
            return this;
        }

        #endregion

        #region *** Join Fluent Methods ***

        public IQueryObject Join(Join join)
        {
            if (Joins == null) Joins = new List<Join>();
            Joins.Add(join);
            return this;
        }

        public IQueryObject Join(JoinType joinType, JoinTable toTableName, JoinColumn toColumnName, ComparisonOperatorType comparisonOperator, JoinTable fromTableName, JoinColumn fromColumnName)
        {
            return Join(new Join()
            {
                Type = joinType,
                ToColumn = toColumnName,
                ToTable = toTableName,
                ComparisonOperator = comparisonOperator,
                FromColumn = fromColumnName,
                FromTable = fromTableName
            });
        }

        #endregion

        #region *** Where Fluent Methods ***

        public IQueryObject Where(FilterColumn columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value,
            LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            return Where(new WhereFilter()
            {
                Column = columNameOrScalarFunction,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = logicalOperatorType
            });
        }

        public IQueryObject WhereAnd(FilterColumn columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value)
        {
            return Where(new WhereFilter()
            {
                Column = columNameOrScalarFunction,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = LogicalOperatorType.And
            });
        }

        public IQueryObject WhereOr(FilterColumn columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value)
        {
            return Where(new WhereFilter()
            {
                Column = columNameOrScalarFunction,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = LogicalOperatorType.Or
            });
        }

        public IQueryObject Where(WhereFilter whereFilter)
        {
            if (WhereFilters == null) WhereFilters = new List<WhereFilter>();
            WhereFilters.Add(whereFilter);
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
            if (WhereFilters != null) builder = WhereFilters.Aggregate(builder, (current, w) => current.Where(w.Column.AsString(), w.ComparisonOperator, w.ComparisonValue, w.LogicalOperatorType));
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
