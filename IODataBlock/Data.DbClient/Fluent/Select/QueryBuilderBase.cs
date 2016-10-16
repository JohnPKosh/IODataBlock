using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Extensions;
using Data.DbClient.Fluent.Model;

namespace Data.DbClient.Fluent.Select
{
    /* https://github.com/thoss/select-query-builder*/

    public class QueryBuilderBase : IQueryBuilder
    {
        #region Class Initialization

        public QueryBuilderBase()
        {
            //Allcolumns();
        }

        public QueryBuilderBase(SqlLanguageType languageType = SqlLanguageType.SqlServer) : this()
        {
            LanguageType = languageType;
        }

        #endregion Class Initialization

        #region Fields and Properties

        protected string SelectedTable;
        protected List<IWhereClause> WhereClauses = new List<IWhereClause>();
        protected List<string> SelectedColumns = new List<string>();
        protected List<JoinClause> JoinClauses = new List<JoinClause>();
        protected List<OrderClause> SortClauses = new List<OrderClause>();
        protected List<SchemaObject> GroupByClauses = new List<SchemaObject>();
        protected List<HavingClause> HavingClauses = new List<HavingClause>();
        protected TopClause TopClause = new TopClause(0);
        protected LimitClause LimitClause = new LimitClause(0);
        protected OffsetClause OffsetClause = new OffsetClause(0);
        protected SqlLanguageType LanguageType = SqlLanguageType.SqlServer;
        //protected string Schema = "dbo";

        #endregion Fields and Properties

        #region Fluent Methods

        #region SELECT Fluent Methods

        public QueryBuilderBase SelectAllColumns()
        {
            //SelectedColumns.Clear();
            SelectedColumns.Add("*");
            return this;
        }

        public QueryBuilderBase Top(int quantity)
        {
            TopClause.Quantity = quantity;
            return this;
        }

        public QueryBuilderBase SelectColumns(params string[] columns)
        {
            return SelectColumns(columns, false);
        }

        public QueryBuilderBase SelectColumns(IEnumerable<string> columns, bool clearExisting = false)
        {
            if (clearExisting) SelectedColumns.Clear();
            foreach (var column in columns)
            {
                SelectedColumns.Add(column);
            }
            return this;
        }

        public QueryBuilderBase SelectColumns(string columnsString, bool clearExisting = false)
        {
            return SelectColumns(columnsString.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), clearExisting);
        }

        #endregion

        #region FROM Fluent Methods

        public QueryBuilderBase FromTable(string table)
        {
            SelectedTable = table;
            return this;
        }

        public QueryBuilderBase Join(JoinClause joinClause)
        {
            JoinClauses.Add(joinClause);
            return this;
        }

        public QueryBuilderBase Join(JoinType join, string toTableName, string toColumnName, ComparisonOperatorType comparisonOperator, string fromTableName, string fromColumnName)
        {
            return Join(new JoinClause(join, toTableName, toColumnName, comparisonOperator, fromTableName, fromColumnName));
        }

        #endregion

        #region WHERE Fluent Methods

        public QueryBuilderBase Where(IWhereClause whereClause)
        {
            WhereClauses.Add(whereClause);
            return this;
        }

        public QueryBuilderBase Where(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            return Where(new WhereClause(columNameOrScalarFunction, comparisonOperator, value, logicalOperatorType));
        }

        public QueryBuilderBase WhereAnd(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value)
        {
            return Where(new WhereClause(columNameOrScalarFunction, comparisonOperator, value, LogicalOperatorType.And));
        }

        public QueryBuilderBase WhereOr(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value)
        {
            return Where(new WhereClause(columNameOrScalarFunction, comparisonOperator, value, LogicalOperatorType.Or));
        }

        #endregion

        #region GROUP BY Fluent Methods

        public QueryBuilderBase GroupBy(params string[] columns)
        {
            return GroupBy(columns, false);
        }

        public QueryBuilderBase GroupBy(IEnumerable<string> columns, bool clearExisting = false)
        {
            if (clearExisting) GroupByClauses.Clear();
            foreach (var column in columns)
            {
                GroupByClauses.Add(new SchemaObject(column, null, null, SchemaValueType.Preformatted));
            }
            return this;
        }

        public QueryBuilderBase GroupBy(string columnsString, bool clearExisting = false)
        {
            return GroupBy(columnsString.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries), clearExisting);
        }

        #endregion

        #region HAVING Fluent Methods

        public QueryBuilderBase Having(HavingClause havingClause)
        {
            HavingClauses.Add(havingClause);
            return this;
        }

        public QueryBuilderBase Having(string columNameOrAggregateFunction, ComparisonOperatorType comparisonOperator, object value, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or)
        {
            return Having(new HavingClause(columNameOrAggregateFunction, comparisonOperator, value, logicalOperatorType));
        }

        public QueryBuilderBase HavingAnd(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value)
        {
            return Having(new HavingClause(columNameOrScalarFunction, comparisonOperator, value, LogicalOperatorType.And));
        }

        public QueryBuilderBase HavingOr(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value)
        {
            return Having(new HavingClause(columNameOrScalarFunction, comparisonOperator, value, LogicalOperatorType.Or));
        }

        #endregion

        #region ORDER BY Fluent Methods

        public QueryBuilderBase OrderBy(OrderClause sortClause)
        {
            SortClauses.Add(sortClause);
            return this;
        }

        public QueryBuilderBase OrderBy(string column, OrderType sorting = OrderType.Ascending)
        {
            return OrderBy(new OrderClause(column, sorting));
        }

        #endregion

        #region SKIP/TAKE (LIMIT/OFFSET) Fluent Methods

        public QueryBuilderBase Take(int take)
        {
            LimitClause.Take = take;
            return this;
        }

        public QueryBuilderBase Skip(int skip)
        {
            OffsetClause.Skip = skip;
            return this;
        } 

        #endregion

        #endregion Fluent Methods

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

        public IQueryObject GetQueryObject()
        {
            // TODO: make language specific implementations here?
            var o = new QueryObjectBase
            {
                SelectColumns = SelectedColumns.Select(x=> new SchemaObject() { Value = x, ValueType = SchemaValueType.Preformatted}).ToList(),
                Top = TopClause.Quantity,
                FromTable = new SchemaObject(SelectedTable, null, null, SchemaValueType.Preformatted),
                Joins = JoinClauses.Select(x=> (Join)x).ToList(),
                WhereFilters = WhereClauses.Select(x=>(Where)(WhereClause)x).ToList(),
                GroupBy = GroupByClauses.Select(x=>x).ToList(),
                HavingClauses = HavingClauses.Select(x => (Having)x).ToList(),
                OrderByClauses = SortClauses.Select(x => (OrderBy)x).ToList(),
                Skip = OffsetClause.Skip,
                Take = LimitClause.Take
            };

            return o;
        }

        #endregion IQueryBuilder Methods

        #region Private Methods

        #region Build Query Methods

        private string CompileSelectSegment()
        {
            if (!SelectedColumns.Any()) SelectAllColumns();
            if (OffsetClause.Skip > 0)
            {
                TopClause.Quantity = 0;
                if (!SortClauses.Any())
                {
                    SortClauses.Add(new OrderClause("1"));
                }
            }
            var query = $"SELECT{(TopClause.Quantity > 0 ? $" TOP {TopClause.Quantity} " : " ")}{GetSelectedColumnString()}{CompileFromSegment()}";
            return ApplyJoins(query);
        }

        private string GetSelectedColumnString()
        {
            return string.Join("\r\n\t,", SelectedColumns);
        }

        private string CompileFromSegment()
        {
            return $"\r\nFROM {SelectedTable}";
        }

        private string ApplyJoins(string query)
        {
            return JoinClauses.Aggregate(query, (current, joinClause) => 
            current + $" {GetJoinType(joinClause.JoinType)} {joinClause.ToTable} \r\nON {RemoveTableSchema(GetTableAlias(joinClause.FromTable))}.{joinClause.FromColumn} {GetComparisonOperator(joinClause.ComparisonOperator)} {GetTableAlias(joinClause.ToTable)}.{joinClause.ToColumn}");
        }

        private static string RemoveTableSchema(string tableName)
        {
            return tableName.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase) > 0 ? tableName.Substring(tableName.LastIndexOf(".", StringComparison.InvariantCultureIgnoreCase) + ".".Length) : tableName;
        }

        private static string RemoveTableAlias(string tableName)
        {
            return tableName.IndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase) > 0 ? tableName.Substring(0, tableName.LastIndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase)): tableName;
        }

        private static string GetTableAlias(string tableName)
        {
            return tableName.IndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase) > 0 ? tableName.Substring(tableName.LastIndexOf(" AS ", StringComparison.InvariantCultureIgnoreCase) + " AS ".Length) : tableName;
        }

        private string CompileWhereSegment()
        {
            var rv = WhereClauses.Count > 0 ? "\r\nWHERE " : "";
            foreach (var whereClause in WhereClauses)
            {
                if (whereClause is WhereClause)
                {
                    rv += $"{GetFilterStatement(new List<WhereClause> { whereClause as WhereClause }, !whereClause.Equals(WhereClauses.Last()))}";
                }
                else if (whereClause is GroupWhereClause)
                {
                    rv += $"({GetFilterStatement((whereClause as GroupWhereClause).WhereClauses)})";
                    if (!whereClause.Equals(WhereClauses.Last()))
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
            var query = GroupByClauses.Count > 0 ? "\r\nGROUP BY " : "";
            foreach (var groupByClause in GroupByClauses)
            {
                query += $"{groupByClause.AsString()}";
                if (!groupByClause.Equals(GroupByClauses.Last()))
                {
                    query += "\r\n\t, ";
                }
            }
            return query;
        }

        private string CompileHavingSegment()
        {
            var query = HavingClauses.Count > 0 ? "\r\nHAVING " : "";
            return HavingClauses.Aggregate(query, (current, havingClause) => current + GetFilterStatement(new List<HavingClause> { havingClause }, !havingClause.Equals(HavingClauses.Last())));
        }

        private string CompileOrderBySegment()
        {
            var query = SortClauses.Count > 0 ? "\r\nORDER BY " : "";
            /* TODO: Fix the logic here for correct formatting - (JKOSH)  */
            foreach (var sortClause in SortClauses)
            {
                query += $"{sortClause.Column} {GetSortingType(sortClause.Sorting)}";
                if (!sortClause.Equals(SortClauses.Last()))
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
                    return $"{(LimitClause.Take > 0 ? $"\r\nFETCH NEXT {LimitClause.Take} ROWS ONLY " : "")}";
                case SqlLanguageType.PostgreSql:
                    return $"{(LimitClause.Take > 0 ? $" LIMIT {LimitClause.Take} " : "")}";
                default:
                    return $"{(LimitClause.Take > 0 ? $" LIMIT {LimitClause.Take} " : "")}";
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
                    return LimitClause.Take > 0 ? $"{(OffsetClause.Skip > 0 ? $"\r\nOFFSET {OffsetClause.Skip} ROWS " : "\r\nOFFSET 0 ROWS ")}" : $"{(OffsetClause.Skip > 0 ? $"\r\nOFFSET {OffsetClause.Skip} ROWS " : "")}";
                case SqlLanguageType.PostgreSql:
                    return $"{(OffsetClause.Skip > 0 ? $" OFFSET {OffsetClause.Skip} " : "")}";
                default:
                    return $"{(OffsetClause.Skip > 0 ? $" OFFSET {OffsetClause.Skip} " : "")}";
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

        private static string GetFilterStatement<T>(List<T> filterClauses, bool ignoreCheckLastClause = false)
            where T : FilterClause
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

        private static string GetComparisonClause(FilterClause filterClause)
        {

            if (filterClause.ComparisonOperator == ComparisonOperatorType.In || filterClause.ComparisonOperator == ComparisonOperatorType.NotIn)
            {
                return $"{filterClause.FieldName} {GetComparisonOperator(filterClause.ComparisonOperator, filterClause.Value == null)} ({FormatSqlValue(filterClause.Value)})";
            }
            else
            {
                return $"{filterClause.FieldName} {GetComparisonOperator(filterClause.ComparisonOperator, filterClause.Value == null)} {FormatSqlValue(filterClause.Value)}";
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