using Data.DbClient.Fluent.Select;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

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
            var queryBuilder = new MsSqlQueryBuilder();

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
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
            .Top(100)
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
        public void SelectWhereAnd_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
            .FromTable("[dbo].[LinkedInProfile]")
            .WhereAnd("LinkedInFullName", Comparison.Equals, "Andi Cook")
            .Where("LinkedInCompanyName", Comparison.Equals, "Onvoy, LLC");

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void SelectColumnsWhereAnd_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
            .Columns("LinkedInFullName", "LinkedInConnections", "LinkedInTitle", "LinkedInCompanyName")
            .FromTable("[dbo].[LinkedInProfile]")
            .WhereAnd("LinkedInFullName", Comparison.Equals, "Andi Cook")
            .Where("LinkedInCompanyName", Comparison.Equals, "Onvoy, LLC");

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }


        [TestMethod]
        public void SelectColumnStringWhereAnd_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
            .Columns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],[LinkedInCompanyName]")
            .FromTable("[dbo].[LinkedInProfile]")
            .WhereAnd("LinkedInFullName", Comparison.Equals, "Andi Cook")
            .Where("LinkedInCompanyName", Comparison.Equals, "Onvoy, LLC");

            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

        [TestMethod]
        public void SelectSkipTakeTop_string_IsValid()
        {
            var queryBuilder = new MsSqlQueryBuilder();

            queryBuilder
                .Columns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],[LinkedInCompanyName]")
                .FromTable("[dbo].[LinkedInProfile] AS a")
                //.Join(JoinType.InnerJoin, "regions", "zip", Comparison.Equals, "customers", "zip")
                .WhereAnd("[LinkedInFullName]", Comparison.Equals, "Andi Cook")
                .WhereAnd("[LinkedInFullName]", Comparison.Equals, "Andi Cook")
                .Where("[LinkedInCompanyName]", Comparison.Equals, "Onvoy, LLC")
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
                .Columns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],a.[LinkedInCompanyName]")
                .FromTable("[dbo].[LinkedInProfile] AS a")
                .Join(JoinType.InnerJoin, "[dbo].[LinkedInCompany]", "[LinkedInCompanyName]", Comparison.Equals, "[dbo].[LinkedInProfile] AS a", "[LinkedInCompanyName]")
                .WhereAnd("[LinkedInFullName]", Comparison.Equals, "Jess Gilman")
                .WhereAnd("[LinkedInFullName]", Comparison.Equals, "Jess Gilman")
                .Where("a.[LinkedInCompanyName]", Comparison.Equals, "T3 Motion")
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
                .Columns("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],a.[LinkedInCompanyName]")
                .GroupBy("[LinkedInFullName],[LinkedInConnections],[LinkedInTitle],a.[LinkedInCompanyName]")
                .FromTable("[dbo].[LinkedInProfile] AS a")
                .Join(JoinType.InnerJoin, "[dbo].[LinkedInCompany] as b", "[LinkedInCompanyName]", Comparison.Equals, "[dbo].[LinkedInProfile] AS a", "[LinkedInCompanyName]")
                .WhereAnd("[LinkedInFullName]", Comparison.Equals, "Jess Gilman")
                .WhereAnd("[LinkedInFullName]", Comparison.Equals, "Jess Gilman")
                .Where("a.[LinkedInCompanyName]", Comparison.Equals, "T3 Motion")
                .Skip(2)
                .Take(10)
                .Top(100);


            var sql = queryBuilder.BuildQuery();
            Assert.IsNotNull(sql);
        }

    }
}