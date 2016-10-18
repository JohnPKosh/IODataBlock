using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Select;

namespace Data.DbClient.Fluent.Interfaces
{
    public interface IModelQueryBuilder
    {
        ModelQueryBuilder SelectAllColumns();
        ModelQueryBuilder Top(int quantity);
        ModelQueryBuilder SelectColumns(params string[] columns);
        ModelQueryBuilder SelectColumns(IEnumerable<string> columns, bool clearExisting = false);
        ModelQueryBuilder SelectColumns(string columnsString, bool clearExisting = false);
        ModelQueryBuilder FromTable(string table);
        ModelQueryBuilder Join(JoinClause joinClause);
        ModelQueryBuilder Join(JoinType join, string toTableName, string toColumnName, ComparisonOperatorType comparisonOperator, string fromTableName, string fromColumnName);
        ModelQueryBuilder Where(IWhereClause whereClause);
        ModelQueryBuilder Where(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or);
        ModelQueryBuilder WhereAnd(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value);
        ModelQueryBuilder WhereOr(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value);
        ModelQueryBuilder GroupBy(params string[] columns);
        ModelQueryBuilder GroupBy(IEnumerable<string> columns, bool clearExisting = false);
        ModelQueryBuilder GroupBy(string columnsString, bool clearExisting = false);
        ModelQueryBuilder Having(HavingClause havingClause);
        ModelQueryBuilder Having(string columNameOrAggregateFunction, ComparisonOperatorType comparisonOperator, object value, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or);
        ModelQueryBuilder HavingAnd(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value);
        ModelQueryBuilder HavingOr(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value);
        ModelQueryBuilder OrderBy(OrderClause sortClause);
        ModelQueryBuilder OrderBy(string column, OrderType sorting = OrderType.Ascending);
        ModelQueryBuilder Take(int take);
        ModelQueryBuilder Skip(int skip);
        string BuildQuery();
        IQueryObject GetQueryObject();
    }
}
