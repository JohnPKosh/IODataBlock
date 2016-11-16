using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Business.Common.System;
using Data.Fluent.Enums;
using Data.Fluent.Extensions;
using Data.Fluent.Interfaces;
using Data.Fluent.Model;
using Data.Fluent.Model.Schema;
using Data.Fluent.Select;
using Newtonsoft.Json;

namespace Data.Fluent.Base
{
    public class QueryObjectBase : ObjectBase<QueryObjectBase>, IQueryObject
    {
        public QueryObjectBase()
        {
            LanguageType = SqlLanguageType.SqlServer;
            SelectColumns = new List<SelectColumn>();
            Joins = new List<Join>();
            WhereFilters = new List<IWhereFilter>();
            GroupByColumns = new List<GroupByColumn>();
            HavingFilters = new List<HavingFilter>();
            OrderByClauses = new List<OrderBy>();
        }

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
        public List<IWhereFilter> WhereFilters { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<GroupByColumn> GroupByColumns { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<HavingFilter> HavingFilters { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public List<OrderBy> OrderByClauses { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? SkipValue { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public int? TakeValue { get; set; }

        #endregion

        #region *** SELECT Fluent Methods ***

        public IQueryObject SelectAll()
        {
            if(SelectColumns == null)SelectColumns = new List<SelectColumn>();
            SelectColumns.Add(new SelectColumn() {Value = "*", ValueType = SchemaValueType.NamedObject});
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

        public IQueryObject Where(FilterColumn column, ComparisonOperatorType comparisonOperator, object value,
            LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            return Where(new WhereFilter()
            {
                Column = column,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = logicalOperatorType
            });
        }

        public IQueryObject WhereAnd(FilterColumn column, ComparisonOperatorType comparisonOperator, object value)
        {
            return Where(new WhereFilter()
            {
                Column = column,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = LogicalOperatorType.And
            });
        }

        public IQueryObject WhereOr(FilterColumn column, ComparisonOperatorType comparisonOperator, object value)
        {
            return Where(new WhereFilter()
            {
                Column = column,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = LogicalOperatorType.Or
            });
        }

        public IQueryObject Where(IWhereFilter whereFilter)
        {
            if (WhereFilters == null) WhereFilters = new List<IWhereFilter>();
            WhereFilters.Add(whereFilter);
            return this;
        }

        #endregion

        #region *** Group By Fluent Methods ***

        public IQueryObject GroupBy(params string[] columns)
        {
            return GroupBy(columns, SchemaValueType.Preformatted, false);
        }

        public IQueryObject GroupBy(string columnsString, SchemaValueType valueType = SchemaValueType.Preformatted, bool clearExisting = false)
        {
            return GroupBy(columnsString.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), valueType, clearExisting);
        }

        public IQueryObject GroupBy(IEnumerable<string> columns, SchemaValueType valueType = SchemaValueType.Preformatted, bool clearExisting = false)
        {
            return GroupBy(columns.Select(x => new GroupByColumn() { Value = x, ValueType = valueType }));
        }

        public IQueryObject GroupBy(IEnumerable<GroupByColumn> columns, bool clearExisting = false)
        {
            if (clearExisting) GroupByColumns.Clear();
            foreach (var column in columns)
            {
                GroupByColumns.Add(column);
            }
            return this;
        }

        #endregion

        #region *** Having Fluent Methods ***

        public IQueryObject Having(FilterColumn column, ComparisonOperatorType comparisonOperator, object value, 
            LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            return Having(new HavingFilter()
            {
                Column = column,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = logicalOperatorType
            });
        }

        public IQueryObject HavingAnd(FilterColumn column, ComparisonOperatorType comparisonOperator, object value)
        {
            return Having(new HavingFilter()
            {
                Column = column,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = LogicalOperatorType.And
            });
        }

        public IQueryObject HavingOr(FilterColumn column, ComparisonOperatorType comparisonOperator, object value)
        {
            return Having(new HavingFilter()
            {
                Column = column,
                ComparisonOperator = comparisonOperator,
                ComparisonValue = value,
                LogicalOperatorType = LogicalOperatorType.Or
            });
        }

        public IQueryObject Having(HavingFilter havingFilter)
        {
            if (HavingFilters == null) HavingFilters = new List<HavingFilter>();
            HavingFilters.Add(havingFilter);
            return this;
        }

        #endregion

        #region *** Order By Fluent Methods ***

        public IQueryObject OrderBy(OrderBy orderBy)
        {
            OrderByClauses.Add(orderBy);
            return this;
        }

        public IQueryObject OrderBy(string column, OrderType sortDirection = OrderType.Ascending, string prefixOrSchema = null,
            SchemaValueType valueType = SchemaValueType.NamedObject)
        {
            return OrderBy(new OrderBy(column, sortDirection, prefixOrSchema, valueType));
        }

        #endregion


        #region *** SKIP/TAKE (LIMIT/OFFSET) Fluent Methods ***

        public IQueryObject Take(int take)
        {
            TakeValue = take;
            return this;
        }

        public IQueryObject Skip(int skip)
        {
            SkipValue = skip;
            return this;
        }

        #endregion

        #region *** Utility Methods ***

        //public string GetQuery(IQueryBuilder builder)
        //{
        //    builder = builder.FromTable(FromTable.AsString());
        //    builder = SelectColumns != null ? builder.SelectColumns(GetSelectedColumnsStringList(SelectColumns)) : builder.SelectAllColumns();
        //    if (TopValue.HasValue) builder = builder.Top(TopValue.Value);
        //    if (Joins != null) builder = Joins.Aggregate(builder, (current, j) => current.Join(j.Type, j.ToTable.AsString(), j.ToColumn.AsString(), j.ComparisonOperator, j.FromTable.AsString(), j.FromColumn.AsString()));
        //    if (WhereFilters != null) builder = WhereFilters.Aggregate(builder, (current, w) => current.Where(w.Column.AsString(), w.ComparisonOperator, w.ComparisonValue, w.LogicalOperatorType));
        //    if (GroupByColumns != null) builder = builder.GroupBy(GroupByColumns.Select(x => x.AsString()));
        //    if (HavingFilters != null) builder = HavingFilters.Aggregate(builder, (current, h) => current.Having(h.Column.AsString(), h.ComparisonOperator, h.ComparisonValue, h.LogicalOperatorType));
        //    if (OrderByClauses != null) builder = OrderByClauses.Aggregate(builder, (current, o) => current.OrderBy(o.Column.AsString(), o.SortDirection));
        //    if (SkipValue.HasValue) builder = builder.Skip(SkipValue.Value);
        //    if (TakeValue.HasValue) builder = builder.Take(TakeValue.Value);

        //    return builder.BuildQuery();
        //}

        private static IEnumerable<string> GetSelectedColumnsStringList(IEnumerable<SchemaObject> columns, string quotedPrefix = "", string quotedSuffix = "")
        {
            return columns.Select(x => x.AsString(quotedPrefix, quotedSuffix));
        }

        #endregion





        #region IQueryBuilder Methods

        public string BuildQuery()
        {
            var query = CompileSelectSegment(); /* SELECT */
            query += CompileWhereSegment(); /* Append WHERE */
            query += CompileGroupBySegment();  /* Append GROUP BY */
            query += CompileHavingSegment(); /* Append HAVING */
            query += CompileOrderBySegment(); /* Append ORDER BY */
            /* TODO: Implement specific SQL language methods here - (JKOSH) */
            if (LanguageType == SqlLanguageType.SqlServer)
            {
                query += CompileOffsetSegment(); /* Append OFFSET BY (SKIP) */
                query += CompileLimitSegment(); /* Append LIMIT BY (TAKE) */
            }
            else
            {
                query += CompileLimitSegment(); /* Append LIMIT BY */
                query += CompileOffsetSegment(); /* Append OFFSET BY */
            }

            return query;
        }


        #endregion IQueryBuilder Methods

        #region Private Methods

        #region Build Query Methods

        private string CompileSelectSegment()
        {
            if (SelectColumns == null || !SelectColumns.Any()) SelectAll();
            if (SkipValue.HasValue && SkipValue.Value > 0)
            {
                TopValue = 0;
                if (!OrderByClauses.Any())
                {
                    OrderByClauses.Add(new OrderBy("1", valueType:SchemaValueType.Preformatted));
                }
            }
            var query = $"SELECT{(TopValue.HasValue && TopValue.Value > 0 ? $" TOP {TopValue.Value} " : " ")}{GetSelectedColumnString()}{CompileFromSegment()}";
            return ApplyJoins(query);
        }

        private string GetSelectedColumnString()
        {
            return string.Join("\r\n\t,", SelectColumns.Select(x=>x.AsString(LanguageType)));
        }

        private string CompileFromSegment()
        {
            return $"\r\nFROM {FromTable.AsString(LanguageType)}";
        }

        private string ApplyJoins(string query)
        {
            return Joins.Aggregate(query, (current, joinClause) =>
            current + $" {GetJoinType(joinClause.Type)} {joinClause.ToTable.AsString(LanguageType)} \r\nON {RemoveTableSchema(GetTableAlias(joinClause.FromTable.AsString(LanguageType)))}.{joinClause.FromColumn.AsString(LanguageType)} {GetComparisonOperator(joinClause.ComparisonOperator)} {GetTableAlias(joinClause.ToTable.AsString(LanguageType))}.{joinClause.ToColumn.AsString(LanguageType)}");
        }

        private static string RemoveTableSchema(string tableName)
        {
            return tableName.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase) > 0 ? tableName.Substring(tableName.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase) + ".".Length) : tableName;
        }

        private static string RemoveTableAlias(string tableName)
        {
            return tableName.IndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase) > 0 ? tableName.Substring(0, tableName.LastIndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase)) : tableName;
        }

        private static string GetTableAlias(string tableName)
        {
            return tableName.IndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase) > 0 ? tableName.Substring(tableName.LastIndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase) + " AS ".Length) : tableName;
        }

        private string CompileWhereSegment()
        {
            var rv = WhereFilters.Count > 0 ? "\r\nWHERE " : "";
            foreach (var whereClause in WhereFilters)
            {
                if (whereClause is WhereFilter)
                {
                    rv += $"{GetFilterStatement(new List<WhereFilter> { whereClause as WhereFilter }, !whereClause.Equals(WhereFilters.Last()))}";
                }
                else if (whereClause is GroupWhereClause)
                {
                    rv += $"({GetFilterStatement((whereClause as GroupWhereFilter).WhereClauses)})";
                    if (!whereClause.Equals(WhereFilters.Last()))
                    {
                        // Build Logicoperator
                        rv += $"{GetLogicOperator((whereClause as GroupWhereClause).LogicalOperatorType)} ";
                    }
                }
            }
            return rv;
        }

        private string CompileGroupBySegment()
        {
            var query = GroupByColumns != null && GroupByColumns.Count > 0 ? "\r\nGROUP BY " : "";
            if (GroupByColumns == null) return query;
            foreach (var groupByClause in GroupByColumns)
            {
                query += $"{groupByClause.AsString(LanguageType)}";
                if (!groupByClause.Equals(GroupByColumns.Last()))
                {
                    query += "\r\n\t, ";
                }
            }
            return query;
        }

        private string CompileHavingSegment()
        {
            if (HavingFilters == null || !HavingFilters.Any()) return string.Empty;
            var query = HavingFilters.Count > 0 ? "\r\nHAVING " : "";
            return HavingFilters.Aggregate(query, (current, havingClause) => current + GetFilterStatement(new List<HavingFilter> { havingClause }, !havingClause.Equals(HavingFilters.Last())));
        }

        private string CompileOrderBySegment()
        {
            if (OrderByClauses == null || !OrderByClauses.Any()) return string.Empty;
            var query = OrderByClauses.Count > 0 ? "\r\nORDER BY " : "";
            /* TODO: Fix the logic here for correct formatting - (JKOSH)  */
            foreach (var sortClause in OrderByClauses)
            {
                query += $"{sortClause.Column.AsString(LanguageType)} {GetSortingType(sortClause.SortDirection)}";
                if (!sortClause.Equals(OrderByClauses.Last()))
                {
                    query += ", ";
                }
            }
            return query;
        }

        /// <summary>
        /// Generates the LIMIT or TAKE clause. e.g. FETCH NEXT 10 ROWS ONLY
        /// </summary>
        /// <returns>string</returns>
        private string CompileLimitSegment()
        {
            switch (LanguageType)
            {
                case SqlLanguageType.SqlServer:
                    return $"{(TakeValue.HasValue && TakeValue.Value > 0 ? $"\r\nFETCH NEXT {TakeValue.Value} ROWS ONLY " : "")}";
                case SqlLanguageType.PostgreSql:
                    return $"{(TakeValue.HasValue && TakeValue.Value > 0 ? $" LIMIT {TakeValue.Value} " : "")}";
                default:
                    return $"{(TakeValue.HasValue && TakeValue.Value > 0 ? $" LIMIT {TakeValue.Value} " : "")}";
                    /* TODO: Implement and Test all specific language types - (JKOSH) */
            }
        }

        /// <summary>
        /// Generates the OFFSET or SKIP clause. e.g. OFFSET 2 ROWS
        /// </summary>
        /// <returns>string</returns>
        private string CompileOffsetSegment()
        {
            switch (LanguageType)
            {
                case SqlLanguageType.SqlServer:
                    return TakeValue.HasValue && TakeValue.Value > 0 ? $"{(SkipValue.HasValue && SkipValue.Value > 0 ? $"\r\nOFFSET {SkipValue.Value} ROWS " : "\r\nOFFSET 0 ROWS ")}" : $"{(SkipValue.HasValue && SkipValue.Value > 0 ? $"\r\nOFFSET {SkipValue.Value} ROWS " : "")}";
                case SqlLanguageType.PostgreSql:
                    return $"{(SkipValue.HasValue && SkipValue.Value > 0 ? $" OFFSET {SkipValue.Value} " : "")}";
                default:
                    return $"{(SkipValue.HasValue && SkipValue.Value > 0 ? $" OFFSET {SkipValue.Value} " : "")}";
                    /* TODO: Implement and Test all specific language types - (JKOSH) */
            }
        }

        #endregion Build Query Methods

        #region Utility Methods

        private static string FormatSqlValue(object value)
        {
            if (value == null)
            {
                return "NULL";
            }

            switch (value.GetType().Name)
            {
                case "String":
                    if (StringValueIsAggregate((string) value)) return (string) value;
                    return "'" + ((string)value).Replace("'", "''") + "'";

                case "Boolean":
                    return (bool)value ? "1" : "0";

                case "DateTime":
                    //return $"CONVERT (DATETIME,'{((DateTime)value).ToString("yyyy-dd-MM HH:mm:ss")}')";
                    return $"'{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}'"; // TODO: review this!!!!!
                case "SqlLiteral":
                    // ReSharper disable once PossibleNullReferenceException
                    return (value as SqlLiteral).Expression;

                case "SqlParameter":
                    // ReSharper disable once PossibleNullReferenceException
                    return (value as SqlParameter).ParameterName;

                default:
                    return value.ToString();
            }
        }

        private static bool StringValueIsAggregate(string value)
        {
            return value.ToUpper().StartsWith("AVG(") ||
                   value.ToUpper().StartsWith("MIN(") ||
                   value.ToUpper().StartsWith("CHECKSUM_AGG(") ||
                   value.ToUpper().StartsWith("SUM(") ||
                   value.ToUpper().StartsWith("COUNT(") ||
                   value.ToUpper().StartsWith("STDEV(") ||
                   value.ToUpper().StartsWith("COUNT_BIG(") ||
                   value.ToUpper().StartsWith("STDEVP(") ||
                   value.ToUpper().StartsWith("GROUPING(") ||
                   value.ToUpper().StartsWith("VAR(") ||
                   value.ToUpper().StartsWith("GROUPING_ID(") ||
                   value.ToUpper().StartsWith("VARP(") ||
                   value.ToUpper().StartsWith("MAX(");
        }

        private string GetFilterStatement<T>(List<T> filterClauses, bool ignoreCheckLastClause = false)
            where T : FilterBase
        {
            var output = "";
            foreach (var clause in filterClauses)
            {
                output = output + GetComparisonClause(clause);
                if (ignoreCheckLastClause || !clause.Equals(filterClauses.Last()))
                {
                    output += $" {GetLogicOperator(clause.LogicalOperatorType)} ";
                }
            }
            return output;
        }

        private string GetComparisonClause(FilterBase filterClause)
        {

            if (filterClause.ComparisonOperator == ComparisonOperatorType.In || filterClause.ComparisonOperator == ComparisonOperatorType.NotIn)
            {
                return $"{filterClause.Column.AsString(LanguageType)} {GetComparisonOperator(filterClause.ComparisonOperator, filterClause.ComparisonValue == null)} ({FormatSqlValue(filterClause.ComparisonValue)})";
            }
            else
            {
                return $"{filterClause.Column.AsString(LanguageType)} {GetComparisonOperator(filterClause.ComparisonOperator, filterClause.ComparisonValue == null)} {FormatSqlValue(filterClause.ComparisonValue)}";
            }
        }

        private static string GetJoinType(JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.InnerJoin:
                    return "\r\nINNER JOIN";

                case JoinType.LeftJoin:
                    return "\r\nLEFT OUTER JOIN";

                case JoinType.RightJoin:
                    return "\r\nRIGHT OUTER JOIN";

                case JoinType.OuterJoin:
                    return "\r\nFULL OUTER JOIN";

                default:
                    return "";
            }
        }

        private static string GetLogicOperator(LogicalOperatorType logicalOperatorType)
        {
            switch (logicalOperatorType)
            {
                case LogicalOperatorType.And:
                    return "\r\nAND";

                case LogicalOperatorType.Or:
                    return "\r\nOR";

                default:
                    return "";
            }
        }

        private static string GetSortingType(OrderType sorting)
        {
            switch (sorting)
            {
                case OrderType.Ascending:
                    return "ASC";

                case OrderType.Descending:
                    return "DESC";

                default:
                    return "";
            }
        }

        private static string GetComparisonOperator(ComparisonOperatorType comparisonOperator, bool isNull = false)
        {
            if (isNull)
            {
                switch (comparisonOperator)
                {
                    case ComparisonOperatorType.Equals:
                        return "IS";

                    case ComparisonOperatorType.NotEquals:
                        return "IS NOT";
                }
            }
            switch (comparisonOperator)
            {
                case ComparisonOperatorType.Equals:
                    return "=";

                case ComparisonOperatorType.NotEquals:
                    return "!=";

                case ComparisonOperatorType.GreaterThan:
                    return ">";

                case ComparisonOperatorType.GreaterOrEquals:
                    return ">=";

                case ComparisonOperatorType.LessThan:
                    return "<";

                case ComparisonOperatorType.LessOrEquals:
                    return "<=";

                case ComparisonOperatorType.Like:
                    return "LIKE";

                case ComparisonOperatorType.NotLike:
                    return "NOT LIKE";

                case ComparisonOperatorType.In:
                    return "IN";

                case ComparisonOperatorType.NotIn:
                    return "NOT IN";

                default:
                    return "";
            }
        }

        #endregion Utility Methods

        #endregion Private Methods


    }
}
