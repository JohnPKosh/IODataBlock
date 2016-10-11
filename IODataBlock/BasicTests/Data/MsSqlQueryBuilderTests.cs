using System;
using Data.DbClient.Fluent.Select;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Model;
using Newtonsoft.Json.Linq;

namespace BasicTests.Data
{
    [TestClass]
    public class MsSqlQueryBuilderTests
    {
        [TestMethod]
        public void SelectQueryBuilderTest()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder.FromTable("customers")
            .Join(JoinType.InnerJoin, "regions", "zip", ComparisonOperatorType.Equals, "customers", "zip")
            .SelectColumns("customers.name", "customers.firstname", "regions.city")
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
            var queryBuilder = new MsSqlQueryBuilder();

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
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
            .Top(100)
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
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
            .FromTable("[dbo].[LinkedInProfile]")
            .WhereAnd("LinkedInFullName", ComparisonOperatorType.Equals, "Andi Cook")
            .Where("LinkedInCompanyName", ComparisonOperatorType.Equals, "Onvoy, LLC");

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void SelectColumnsWhereAnd_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
            .SelectColumns("LinkedInFullName", "LinkedInConnections", "LinkedInTitle", "LinkedInCompanyName")
            .FromTable("[dbo].[LinkedInProfile]")
            .WhereAnd("LinkedInFullName", ComparisonOperatorType.Equals, "Andi Cook")
            .Where("LinkedInCompanyName", ComparisonOperatorType.Equals, "Onvoy, LLC");

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }


        [TestMethod]
        public void SelectColumnStringWhereAnd_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
            .SelectColumns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],[LinkedInCompanyName]")
            .FromTable("[dbo].[LinkedInProfile]")
            .WhereAnd("LinkedInFullName", ComparisonOperatorType.Equals, "Andi Cook")
            .Where("LinkedInCompanyName", ComparisonOperatorType.Equals, "Onvoy, LLC");

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void SelectSkipTakeTop_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
                .SelectColumns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],[LinkedInCompanyName]")
                .FromTable("[dbo].[LinkedInProfile] AS a")
                //.Join(JoinType.InnerJoin, "regions", "zip", ComparisonOperatorType.Equals, "customers", "zip")
                .WhereAnd("[LinkedInFullName]", ComparisonOperatorType.Equals, "Andi Cook")
                .WhereAnd("[LinkedInFullName]", ComparisonOperatorType.Equals, "Andi Cook")
                .Where("[LinkedInCompanyName]", ComparisonOperatorType.Equals, "Onvoy, LLC")
                .Skip(2)
                .Take(10)
                .Top(100);
            

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        // [LinkedInFullName],[LinkedInConnections],[LinkedInTitle],[LinkedInCompanyName]



        [TestMethod]
        public void SelectSkipTakeTopJoin_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
                .SelectColumns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],a.[LinkedInCompanyName]")
                .FromTable("[dbo].[LinkedInProfile] AS a")
                .Join(JoinType.InnerJoin, "[dbo].[LinkedInCompany]", "[LinkedInCompanyName]", ComparisonOperatorType.Equals, "[dbo].[LinkedInProfile] AS a", "[LinkedInCompanyName]")
                .WhereAnd("[LinkedInFullName]", ComparisonOperatorType.Equals, "Jess Gilman")
                .WhereAnd("[LinkedInFullName]", ComparisonOperatorType.Equals, "Jess Gilman")
                .Where("a.[LinkedInCompanyName]", ComparisonOperatorType.Equals, "T3 Motion")
                .Skip(2)
                .Take(10)
                .Top(100);


            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void SelectSkipTakeTopJoin02_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
                .SelectColumns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],a.[LinkedInCompanyName]")
                .GroupBy("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],a.[LinkedInCompanyName]")
                .FromTable("[dbo].[LinkedInProfile] AS a")
                .Join(JoinType.InnerJoin, "[dbo].[LinkedInCompany] as b", "[LinkedInCompanyName]", ComparisonOperatorType.Equals, "[dbo].[LinkedInProfile] AS a", "[LinkedInCompanyName]")
                .WhereAnd("[LinkedInFullName]", ComparisonOperatorType.Equals, "Jess Gilman")
                .WhereAnd("[LinkedInFullName]", ComparisonOperatorType.Equals, "Jess Gilman")
                .Where("a.[LinkedInCompanyName]", ComparisonOperatorType.Equals, "T3 Motion")
                .Skip(2)
                .Take(10)
                .Top(100);


            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void SelectSkipTakeTopLeftJoin_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
                .SelectColumns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],a.[LinkedInCompanyName]")
                //.GroupBy("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],a.[LinkedInCompanyName]")
                .GroupBy("LinkedInFullName", "LinkedInConnections", "LinkedInTitle", "a.LinkedInCompanyName")
                .FromTable("[dbo].[LinkedInProfile] AS a")
                .Join(JoinType.LeftJoin, "[dbo].[LinkedInCompany] as b", "[LinkedInCompanyName]", ComparisonOperatorType.Equals, "[dbo].[LinkedInProfile] AS a", "[LinkedInCompanyName]")
                .WhereAnd("[LinkedInFullName]", ComparisonOperatorType.Equals, "Jess Gilman")
                .WhereAnd("[LinkedInFullName]", ComparisonOperatorType.Equals, "Jess Gilman")
                .Where("a.[LinkedInCompanyName]", ComparisonOperatorType.Equals, "T3 Motion")
                .Skip(2)
                .Take(10)
                .Top(100);


            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void GenericSelectSkipTakeTopLeftJoin_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
                .SelectColumns("LinkedInFullName,LinkedInConnections,LinkedInTitle,a.LinkedInCompanyName")
                //.GroupBy("LinkedInFullName,LinkedInConnections,LinkedInTitle,a.LinkedInCompanyName")
                .GroupBy("LinkedInFullName", "LinkedInConnections", "LinkedInTitle", "a.LinkedInCompanyName")
                .FromTable("dbo.LinkedInProfile AS a")
                .Join(JoinType.LeftJoin, "dbo.LinkedInCompany as b", "LinkedInCompanyName", ComparisonOperatorType.Equals, "dbo.LinkedInProfile AS a", "LinkedInCompanyName")
                .WhereAnd("LinkedInFullName", ComparisonOperatorType.Equals, "Jess Gilman")
                .WhereAnd("LinkedInFullName", ComparisonOperatorType.Equals, "Jess Gilman")
                .Where("a.LinkedInCompanyName", ComparisonOperatorType.Equals, "T3 Motion")
                .Skip(2)
                .Take(10)
                .Top(100);


            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void GenericSelectSkipTakeTopLeftJoinOrderBy_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
                .SelectColumns("LinkedInFullName,LinkedInConnections,LinkedInTitle,a.LinkedInCompanyName, COUNT(*) as cnt")
                //.GroupBy("LinkedInFullName,LinkedInConnections,LinkedInTitle,a.LinkedInCompanyName")
                .GroupBy("LinkedInFullName", "LinkedInConnections", "LinkedInTitle", "a.LinkedInCompanyName")
                .FromTable("dbo.LinkedInProfile AS a")
                .Join(JoinType.LeftJoin, "dbo.LinkedInCompany as b", "LinkedInCompanyName", ComparisonOperatorType.Equals, "dbo.LinkedInProfile AS a", "LinkedInCompanyName")
                .WhereAnd("LinkedInFullName", ComparisonOperatorType.Equals, "Jess Gilman")
                .WhereAnd("LinkedInFullName", ComparisonOperatorType.Equals, "Jess Gilman")
                .WhereAnd("a.[CreatedDate]", ComparisonOperatorType.GreaterThan, DateTime.Now.AddYears(-2))
                .Where("a.LinkedInCompanyName", ComparisonOperatorType.Equals, "T3 Motion")
                .Having("COUNT(*)", ComparisonOperatorType.GreaterThan, 0)
                .OrderBy("LinkedInFullName", OrderType.Ascending)
                .Skip(2)
                .Take(10)
                .Top(100);


            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void Create_QueryStatement_Success()
        {
            var q = new QueryStatement();
            q.SelectColumns = new List<string>() { "LinkedInFullName", "LinkedInConnections", "LinkedInTitle", "a.LinkedInCompanyName" };
            q.FromTable = "[dbo].[SomeTableName]";
            q.WhereFilters = new List<Where>();
            q.WhereFilters.Add(new Where() { FieldName = "[SomeID]", ComparisonOperator = ComparisonOperatorType.GreaterThan, Value = -1, LogicalOperatorType = LogicalOperatorType.Or });
            q.OrderByClauses = new List<OrderBy>();
            q.OrderByClauses.Add(new OrderBy() { Column = "LinkedInFullName" });
            q.Skip = 2;
            q.Take = 10;

            JObject o = q;
            var json = o.ToString();

            var queryBuilder = new MsSqlQueryBuilder();
            queryBuilder.FromTable(q.FromTable);
            foreach (var w in q.WhereFilters)
            {
                queryBuilder = queryBuilder.Where(w.FieldName, w.ComparisonOperator, w.Value, w.LogicalOperatorType);
            }


            var sql = queryBuilder.BuildQuery();

            Assert.IsNotNull(sql);
        }


    }
}