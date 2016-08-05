using System;
using System.Collections.Generic;
using Business.Common.Extensions;
using Data.DbClient.Fluent.Select;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BasicTests.Data
{
    [TestClass]
    public class DbSqlQueryBuilderTests
    {

        [TestMethod]
        public void SelectQueryBuilderTest()
        {
            var queryBuilder = new DbSqlQueryBuilder();

            queryBuilder.FromTable("customers")
            .Join(JoinType.InnerJoin, "regions", "zip", Comparison.Equals, "customers", "zip")
            .Columns("customers.name", "customers.firstname", "regions.city")
            .Where("regions.city", Comparison.Like, "do%")
            .GroupBy("regions.city", "customers.name", "customers.firstname")
            .Having("COUNT(*)", Comparison.GreaterThan, 5)
            .OrderBy("customers.name");

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }


        [TestMethod]
        public void SelectQueryBuilderTest2()
        {
            var queryBuilder = new DbSqlQueryBuilder();

            queryBuilder
                .FromTable("customers")
                .Where("zip", Comparison.In, new SqlLiteral("'58965','47841','12569'"))
                .OrderBy("name", Order.Descending);

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void SelectQueryBuilderTest3()
        {
            var queryBuilder = new DbSqlQueryBuilder();

            queryBuilder
            .Limit(100)
            .Offset(50)
            .FromTable("customers")
            .Where("age", Comparison.LessThan, 55, LogicOperator.And)
            .Where(new GroupWhereClause(new List<WhereClause>
            {
                new WhereClause("name", Comparison.Like, "jo%", LogicOperator.Or),
                new WhereClause("name", Comparison.Like, "pe%"),
            }));

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void IsoDateTimeTest()
        {
            var str = DateTime.Now.ToStringAs8601Format();
            Assert.IsNotNull(str);
            // 2016-05-31T10:39:47.2911337-04:00
        }
    }
}
