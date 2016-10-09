using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.DbClient.Fluent.Select
{
    /* https://github.com/thoss/select-query-builder*/

    public class MsSqlQueryBuilder : IQueryBuilder
    {
        #region Class Initialization

        public MsSqlQueryBuilder()
        {
            //Allcolumns();
        }

        public MsSqlQueryBuilder(string schema) : this()
        {
            Schema = schema;
        }

        #endregion Class Initialization

        #region Fields and Properties

        protected string SelectedTable;
        protected List<IWhereClause> WhereClauses = new List<IWhereClause>();
        protected List<string> SelectedColumns = new List<string>();
        protected List<JoinClause> JoinClauses = new List<JoinClause>();
        protected List<OrderClause> SortClauses = new List<OrderClause>();
        protected List<GroupByClause> GroupByClauses = new List<GroupByClause>();
        protected List<HavingClause> HavingClauses = new List<HavingClause>();
        protected TopClause TopClause = new TopClause(0);
        protected LimitClause LimitClause = new LimitClause(0);
        protected OffsetClause OffsetClause = new OffsetClause(0);
        protected string Schema = "dbo";

        #endregion Fields and Properties

        #region Fluent Methods

        public MsSqlQueryBuilder FromTable(string table)
        {
            SelectedTable = table;
            return this;
        }

        public MsSqlQueryBuilder Columns(params string[] columns)
        {
            return Columns(columns, false);
        }

        public MsSqlQueryBuilder Columns(IEnumerable<string> columns, bool clearExisting = false)
        {
            if (clearExisting) SelectedColumns.Clear();
            foreach (var column in columns)
            {
                SelectedColumns.Add(column);
            }
            return this;
        }

        public MsSqlQueryBuilder Columns(string columnsString, bool clearExisting = false)
        {
            return Columns(columnsString.Split(new char[','], StringSplitOptions.RemoveEmptyEntries), clearExisting);
        }

        public MsSqlQueryBuilder GroupBy(params string[] columns)
        {
            GroupByClauses.Clear();
            foreach (var column in columns)
            {
                GroupByClauses.Add(new GroupByClause(column));
            }
            return this;
        }

        public MsSqlQueryBuilder Allcolumns()
        {
            //SelectedColumns.Clear();
            SelectedColumns.Add("*");
            return this;
        }

        public MsSqlQueryBuilder Where(IWhereClause whereClause)
        {
            WhereClauses.Add(whereClause);
            return this;
        }

        public MsSqlQueryBuilder Where(string columNameOrScalarFunction, Comparison comparison, object value, LogicOperator logicOperator = LogicOperator.Or)
        {
            return Where(new WhereClause(columNameOrScalarFunction, comparison, value, logicOperator));
        }
        
        public MsSqlQueryBuilder WhereAnd(string columNameOrScalarFunction, Comparison comparison, object value)
        {
            return Where(new WhereClause(columNameOrScalarFunction, comparison, value, LogicOperator.And));
        }

        public MsSqlQueryBuilder WhereOr(string columNameOrScalarFunction, Comparison comparison, object value)
        {
            return Where(new WhereClause(columNameOrScalarFunction, comparison, value, LogicOperator.Or));
        }

        public MsSqlQueryBuilder Having(HavingClause havingClause)
        {
            HavingClauses.Add(havingClause);
            return this;
        }

        public MsSqlQueryBuilder Having(string columNameOrAggregateFunction, Comparison comparison, object value, LogicOperator logicOperator = LogicOperator.Or)
        {
            return Having(new HavingClause(columNameOrAggregateFunction, comparison, value, logicOperator));
        }
        
        public MsSqlQueryBuilder HavingAnd(string columNameOrScalarFunction, Comparison comparison, object value)
        {
            return Having(new HavingClause(columNameOrScalarFunction, comparison, value, LogicOperator.And));
        }

        public MsSqlQueryBuilder HavingOr(string columNameOrScalarFunction, Comparison comparison, object value)
        {
            return Having(new HavingClause(columNameOrScalarFunction, comparison, value, LogicOperator.Or));
        }

        public MsSqlQueryBuilder Join(JoinClause joinClause)
        {
            JoinClauses.Add(joinClause);
            return this;
        }

        public MsSqlQueryBuilder Join(JoinType join, string toTableName, string toColumnName, Comparison comparison, string fromTableName, string fromColumnName)
        {
            return Join(new JoinClause(join, toTableName, toColumnName, comparison, fromTableName, fromColumnName));
        }

        public MsSqlQueryBuilder OrderBy(OrderClause sortClause)
        {
            SortClauses.Add(sortClause);
            return this;
        }

        public MsSqlQueryBuilder OrderBy(string column, Order sorting = Order.Ascending)
        {
            return OrderBy(new OrderClause(column, sorting));
        }

        public MsSqlQueryBuilder Top(int quantity)
        {
            TopClause.Quantity = quantity;
            return this;
        }
        
        public MsSqlQueryBuilder Take(int take)
        {
            LimitClause.Take = take;
            return this;
        }

        public MsSqlQueryBuilder Skip(int skip)
        {
            OffsetClause.Skip = skip;
            return this;
        }

        #endregion Fluent Methods

        #region IQueryBuilder Methods

        public string BuildQuery()
        {
            var query = CompileSelectSegment(); /* SELECT */
            query += CompileWhereSegment(); /* Append WHERE */
            query += CompileGroupBySegment();  /* Append GROUP BY */
            query += CompileHavingSegment(); /* Append HAVING */
            query += CompileOrderBySegment(); /* Append ORDER BY */
            query += CompileOffsetSegment(); /* Append OFFSET BY (SKIP) */
            query += CompileLimitSegment(); /* Append LIMIT BY (TAKE) */
            return query;
        }

        #endregion IQueryBuilder Methods

        #region Private Methods

        #region Build Query Methods

        private string CompileSelectSegment()
        {
            if (!SelectedColumns.Any()) Allcolumns();
            if (OffsetClause.Skip > 0)
            {
                TopClause.Quantity = 0;
                if (!SortClauses.Any())
                {
                    SortClauses.Add(new OrderClause("1"));
                }
            }
            var query = $"SELECT{(TopClause.Quantity > 0 ? $" TOP {TopClause.Quantity} " : " ")}{GetSelectedColumnString()}\r\nFROM [{Schema}].[{SelectedTable}]";
            return ApplyJoins(query);
        }

        private string GetSelectedColumnString()
        {
            return string.Join("\r\n\t,", SelectedColumns);
        }

        private string ApplyJoins(string query)
        {
            return JoinClauses.Aggregate(query, (current, joinClause) => current + string.Format(" {0} [{7}].[{1}] ON {2}.{3} {4} {5}.{6}", GetJoinType(joinClause.JoinType), joinClause.ToTable, joinClause.FromTable, joinClause.FromColumn, GetComparisonOperator(joinClause.ComparisonOperator), joinClause.ToTable, joinClause.ToColumn, Schema));
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
                        rv += $"{GetLogicOperator((whereClause as GroupWhereClause).LogicOperator)} ";
                    }
                }
            }
            return rv;
        }

        private string CompileGroupBySegment()
        {
            var query = GroupByClauses.Count > 0 ? " GROUP BY " : "";
            foreach (var groupByClause in GroupByClauses)
            {
                query += $"{groupByClause.Column}";
                if (!groupByClause.Equals(GroupByClauses.Last()))
                {
                    query += ", ";
                }
            }
            return query;
        }

        private string CompileHavingSegment()
        {
            var query = HavingClauses.Count > 0 ? " HAVING " : "";
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
            return $"{(LimitClause.Take > 0 ? $"\r\nFETCH NEXT {LimitClause.Take} ROWS ONLY " : "")}";
        }

        /// <summary>
        /// Generates the OFFSET or SKIP clause. e.g. OFFSET 2 ROWS
        /// </summary>
        /// <returns>string</returns>
        private string CompileOffsetSegment()
        {
            return LimitClause.Take > 0 ? $"{(OffsetClause.Skip > 0 ? $"\r\nOFFSET {OffsetClause.Skip} ROWS " : "\r\nOFFSET 0 ROWS ")}" : $"{(OffsetClause.Skip > 0 ? $"\r\nOFFSET {OffsetClause.Skip} ROWS " : "")}";
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
                    return $"CONVERT (DATETIME,'{((DateTime)value).ToString("yyyy-dd-MM")}')";

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
                    output += $" {GetLogicOperator(clause.LogicOperator)} ";
                }
            }
            return output;
        }

        private static string GetComparisonClause(FilterClause filterClause)
        {
            var pattern = filterClause.ComparisonOperator == Comparison.In || filterClause.ComparisonOperator == Comparison.NotIn ? "{0} {1} ({2})" : "{0} {1} {2}";
            return string.Format(pattern, filterClause.FieldName, GetComparisonOperator(filterClause.ComparisonOperator, filterClause.Value == null), FormatSqlValue(filterClause.Value));
        }

        private static string GetJoinType(JoinType joinType)
        {
            switch (joinType)
            {
                case JoinType.InnerJoin:
                    return "INNER JOIN";

                case JoinType.LeftJoin:
                    return "LEFT OUTER JOIN";

                case JoinType.RightJoin:
                    return "RIGHT OUTER JOIN";

                case JoinType.OuterJoin:
                    return "FULL OUTER JOIN";

                default:
                    return "";
            }
        }

        private static string GetLogicOperator(LogicOperator logicOperator)
        {
            switch (logicOperator)
            {
                case LogicOperator.And:
                    return "\r\nAND";

                case LogicOperator.Or:
                    return "\r\nOR";

                default:
                    return "";
            }
        }

        private static string GetSortingType(Order sorting)
        {
            switch (sorting)
            {
                case Order.Ascending:
                    return "ASC";

                case Order.Descending:
                    return "DESC";

                default:
                    return "";
            }
        }

        private static string GetComparisonOperator(Comparison comparison, bool isNull = false)
        {
            if (isNull)
            {
                switch (comparison)
                {
                    case Comparison.Equals:
                        return "IS";

                    case Comparison.NotEquals:
                        return "IS NOT";
                }
            }
            switch (comparison)
            {
                case Comparison.Equals:
                    return "=";

                case Comparison.NotEquals:
                    return "!=";

                case Comparison.GreaterThan:
                    return ">";

                case Comparison.GreaterOrEquals:
                    return ">=";

                case Comparison.LessThan:
                    return "<";

                case Comparison.LessOrEquals:
                    return "<=";

                case Comparison.Like:
                    return "LIKE";

                case Comparison.NotLike:
                    return "NOT LIKE";

                case Comparison.In:
                    return "IN";

                case Comparison.NotIn:
                    return "NOT IN";

                default:
                    return "";
            }
        }

        #endregion Utility Methods

        #endregion Private Methods
    }
}