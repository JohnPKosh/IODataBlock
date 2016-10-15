using System.Collections.Generic;
using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Model;

namespace Data.DbClient.Fluent.Select
{
    public interface IQueryBuilder
    {
        QueryBuilderBase SelectAllColumns();
        QueryBuilderBase Top(int quantity);
        QueryBuilderBase SelectColumns(params string[] columns);
        QueryBuilderBase SelectColumns(IEnumerable<string> columns, bool clearExisting = false);
        QueryBuilderBase SelectColumns(string columnsString, bool clearExisting = false);
        QueryBuilderBase FromTable(string table);
        QueryBuilderBase Join(JoinClause joinClause);
        QueryBuilderBase Join(JoinType join, string toTableName, string toColumnName, ComparisonOperatorType comparisonOperator, string fromTableName, string fromColumnName);
        QueryBuilderBase Where(IWhereClause whereClause);
        QueryBuilderBase Where(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or);
        QueryBuilderBase WhereAnd(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value);
        QueryBuilderBase WhereOr(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value);
        QueryBuilderBase GroupBy(params string[] columns);
        QueryBuilderBase GroupBy(IEnumerable<string> columns, bool clearExisting = false);
        QueryBuilderBase GroupBy(string columnsString, bool clearExisting = false);
        QueryBuilderBase Having(HavingClause havingClause);
        QueryBuilderBase Having(string columNameOrAggregateFunction, ComparisonOperatorType comparisonOperator, object value, LogicalOperatorType logicalOperatorType = LogicalOperatorType.Or);
        QueryBuilderBase HavingAnd(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value);
        QueryBuilderBase HavingOr(string columNameOrScalarFunction, ComparisonOperatorType comparisonOperator, object value);
        QueryBuilderBase OrderBy(OrderClause sortClause);
        QueryBuilderBase OrderBy(string column, OrderType sorting = OrderType.Ascending);
        QueryBuilderBase Take(int take);
        QueryBuilderBase Skip(int skip);
        string BuildQuery();
        IQueryObject GetQueryObject();
    }
}