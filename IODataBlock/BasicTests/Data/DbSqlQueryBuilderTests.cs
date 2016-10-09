using Business.Common.Extensions;
using Data.DbClient.Fluent.Select;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using Data.DbClient.Fluent.Enums;

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
            .Join(JoinType.InnerJoin, "regions", "zip", ComparisonOperatorType.Equals, "customers", "zip")
            .Columns("customers.name", "customers.firstname", "regions.city")
            .Where("regions.city", ComparisonOperatorType.Like, "do%")
            .GroupBy("regions.city", "customers.name", "customers.firstname")
            .Having("COUNT(*)", ComparisonOperatorType.GreaterThan, 5)
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
                .Where("zip", ComparisonOperatorType.In, new SqlLiteral("'58965','47841','12569'"))
                .OrderBy("name", OrderType.Descending);

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
            .Where("age", ComparisonOperatorType.LessThan, 55, LogicalOperatorType.And)
            .Where(new GroupWhereClause(new List<WhereClause>
            {
                new WhereClause("name", ComparisonOperatorType.Like, "jo%", LogicalOperatorType.Or),
                new WhereClause("name", ComparisonOperatorType.Like, "pe%"),
            }));

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void SelectWhereAnd_string_IsValid()
        {
            var queryBuilder = new DbSqlQueryBuilder();

            queryBuilder
            .FromTable("[dbo].[LinkedInProfile]")
            .WhereAnd("LinkedInFullName", ComparisonOperatorType.Equals, "Andi Cook")
            .Where("LinkedInCompanyName", ComparisonOperatorType.Equals, "Onvoy, LLC");

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