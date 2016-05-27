using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.DbClient.Fluent.Select
{
    internal class SqlLiteral
    {
        public string Expression { get; set; }

        public SqlLiteral(string expression)
        {
            Expression = expression;
        }
    }

    internal interface IQueryBuilder
    {
        string BuildQuery();
    }

    internal class SelectQueryBuilder : IQueryBuilder
    {
        protected string SelectedTable;
        protected List<IWhereClause> WhereClauses = new List<IWhereClause>();
        protected List<string> SelectedColumns = new List<string>();
        protected List<JoinClause> JoinClauses = new List<JoinClause>();
        protected List<OrderClause> SortClauses = new List<OrderClause>();
        protected List<GroupByClause> GroupByClauses = new List<GroupByClause>();
        protected List<HavingClause> HavingClauses = new List<HavingClause>();
        protected TopClause TopClause = new TopClause(0);
        protected string Schema = "dbo";

        public SelectQueryBuilder()
        {
            Allcolumns();
        }

        public SelectQueryBuilder(string schema)
            : this()
        {
            Schema = schema;
        }

        public SelectQueryBuilder FromTable(string table)
        {
            SelectedTable = table;
            return this;
        }

        public SelectQueryBuilder Columns(params string[] columns)
        {
            SelectedColumns.Clear();
            foreach (var column in columns)
            {
                SelectedColumns.Add(column);
            }
            return this;
        }

        public SelectQueryBuilder GroupBy(params string[] columns)
        {
            GroupByClauses.Clear();
            foreach (var column in columns)
            {
                GroupByClauses.Add(new GroupByClause(column));
            }
            return this;
        }

        public SelectQueryBuilder Allcolumns()
        {
            SelectedColumns.Clear();
            SelectedColumns.Add("*");
            return this;
        }

        public SelectQueryBuilder Where(IWhereClause whereClause)
        {
            WhereClauses.Add(whereClause);
            return this;
        }

        public SelectQueryBuilder Where(string columNameOrScalarFunction, Comparison comparison, object value, LogicOperator logicOperator = LogicOperator.Or)
        {
            return Where(new WhereClause(columNameOrScalarFunction, comparison, value, logicOperator));
        }

        public SelectQueryBuilder Having(HavingClause havingClause)
        {
            HavingClauses.Add(havingClause);
            return this;
        }

        public SelectQueryBuilder Having(string columNameOrAggregateFunction, Comparison comparison, object value, LogicOperator logicOperator = LogicOperator.Or)
        {
            return Having(new HavingClause(columNameOrAggregateFunction, comparison, value, logicOperator));
        }

        public SelectQueryBuilder Join(JoinClause joinClause)
        {
            JoinClauses.Add(joinClause);
            return this;
        }

        public SelectQueryBuilder Join(JoinType join, string toTableName, string toColumnName, Comparison comparison, string fromTableName, string fromColumnName)
        {
            return Join(new JoinClause(join, toTableName, toColumnName, comparison, fromTableName, fromColumnName));
        }

        public SelectQueryBuilder OrderBy(OrderClause sortClause)
        {
            SortClauses.Add(sortClause);
            return this;
        }

        public SelectQueryBuilder OrderBy(string column, Order sorting = Order.Ascending)
        {
            return OrderBy(new OrderClause(column, sorting));
        }

        public SelectQueryBuilder Top(int quantity)
        {
            TopClause.Quantity = quantity;
            return this;
        }

        private string FormatSqlValue(object value)
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
                    return string.Format("CONVERT (DATETIME,'{0}')", ((DateTime)value).ToString("yyyy-dd-MM"));

                case "SqlLiteral":
                    return (value as SqlLiteral).Expression;

                case "SqlParameter":
                    return (value as SqlParameter).ParameterName;

                default:
                    return value.ToString();
            }
        }

        public string BuildQuery()
        {
            // SELECT
            var query = string.Format("SELECT{0}{1} FROM [{2}].[{3}]",
                TopClause.Quantity > 0 ? string.Format(" TOP {0} ", TopClause.Quantity) : " ",
                string.Join(",", SelectedColumns),
                Schema,
                SelectedTable);
            // JOIN
            foreach (var joinClause in JoinClauses)
            {
                query += string.Format(" {0} [{7}].[{1}] ON {2}.{3} {4} {5}.{6}",
                    GetJoinType(joinClause.JoinType), joinClause.ToTable, joinClause.FromTable, joinClause.FromColumn,
                    GetComparisonOperator(joinClause.ComparisonOperator), joinClause.ToTable, joinClause.ToColumn, Schema);
            }
            // WHERE
            query += WhereClauses.Count > 0 ? " WHERE " : "";
            foreach (var whereClause in WhereClauses)
            {
                if (whereClause is WhereClause)
                {
                    query += GetFilterStatement(new List<WhereClause> { whereClause as WhereClause }, !whereClause.Equals(WhereClauses.Last()));
                }
                else if (whereClause is GroupWhereClause)
                {
                    query += string.Format("({0})", GetFilterStatement((whereClause as GroupWhereClause).WhereClauses));
                    if (!whereClause.Equals(WhereClauses.Last()))
                    {
                        // Build Logicoperator
                        query += string.Format(" {0} ",
                            GetLogicOperator((whereClause as GroupWhereClause).LogicOperator));
                    }
                }
            }
            // GROUP BY
            query += GroupByClauses.Count > 0 ? " GROUP BY " : "";
            foreach (var groupByClause in GroupByClauses)
            {
                query += string.Format("{0}", groupByClause.Column);
                if (!groupByClause.Equals(GroupByClauses.Last()))
                {
                    query += ", ";
                }
            }
            // HAVING
            query += HavingClauses.Count > 0 ? " HAVING " : "";
            foreach (var havingClause in HavingClauses)
            {
                query += GetFilterStatement(new List<HavingClause> { havingClause }, !havingClause.Equals(HavingClauses.Last()));
            }
            // ORDER BY
            query += SortClauses.Count > 0 ? " ORDER BY " : "";
            foreach (var sortClause in SortClauses)
            {
                query += string.Format("{0} {1}", sortClause.Column, GetSortingType(sortClause.Sorting));
                if (!sortClause.Equals(SortClauses.Last()))
                {
                    query += ", ";
                }
            }
            return query;
        }

        private string GetFilterStatement<T>(List<T> filterClauses, bool ignoreCheckLastClause = false)
            where T : FilterClause
        {
            var output = "";
            foreach (var clause in filterClauses)
            {
                output = output + (GetComparisonClause(clause));
                if (ignoreCheckLastClause || !clause.Equals(filterClauses.Last()))
                {
                    output += string.Format(" {0} ", GetLogicOperator(clause.LogicOperator));
                }
            }
            return output;
        }

        private string GetComparisonClause(FilterClause filterClause)
        {
            var pattern = filterClause.ComparisonOperator == Comparison.In ? "{0} {1} ({2})" : "{0} {1} {2}";
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
                    return "AND";

                case LogicOperator.Or:
                    return "OR";

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

                default:
                    return "";
            }
        }
    }
}