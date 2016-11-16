using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Fluent.Base;
using Data.Fluent.Enums;
using Data.Fluent.Model.Schema;

namespace Data.Fluent.Tests
{
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            var QueryObject = new QueryObjectBase {LanguageType = SqlLanguageType.SqlServer};

            var newo = QueryObject
                .SelectAll()
                .From("LinkedInProfile")
                .Join(JoinType.InnerJoin, "Ids", "Id", ComparisonOperatorType.Equals, "LinkedInProfile", "Id")
                .WhereAnd("Id", ComparisonOperatorType.Equals, 12)
               
                ;
            Trace.WriteLine(newo.FromTable.Value);

            newo.SelectColumns[0].Alias = "fuck";

            Assert.Pass("Your first passing test");
        }

        [Test]
        public void SimpleQuery()
        {
            var QueryObject = new QueryObjectBase { LanguageType = SqlLanguageType.SqlServer };
            var newo = QueryObject
                .SelectAll()
                .From(new FromTable("LinkedInProfile", "dbo", "a"))
                .Join(JoinType.InnerJoin, new JoinTable("LinkedInCompany", "dbo", "b"), new JoinColumn("Id"), ComparisonOperatorType.Equals, new JoinTable("LinkedInProfile", "dbo","a"), new JoinColumn("LinkedInCompanyId"))
                .WhereAnd(new FilterColumn("Id", "a"), ComparisonOperatorType.Equals, 33)
                ;
            var sql = newo.BuildQuery();
            Assert.IsNotNull(sql);
            Assert.Pass("Your first passing test");
        }

        [Test]
        public void SimpleQuery2()
        {
            var QueryObject = new QueryObjectBase { LanguageType = SqlLanguageType.SqlServer };
            var newo = QueryObject
                .Select(new []{new SelectColumn("Id", "a") })
                .Top(10)
                .From(new FromTable("LinkedInProfile", "dbo", "a"))
                .Join(JoinType.InnerJoin, new JoinTable("LinkedInCompany", "dbo", "b"), new JoinColumn("Id"), ComparisonOperatorType.Equals, new JoinTable("LinkedInProfile", "dbo", "a"), new JoinColumn("LinkedInCompanyId"))
                .WhereAnd(new FilterColumn("Id", "a"), ComparisonOperatorType.Equals, 33)
                .OrderBy("Id", OrderType.Descending, "a")
                .GroupBy(new []{new GroupByColumn("Id", "a") })
                ;
            var sql = newo.BuildQuery();
            Assert.IsNotNull(sql);
            Assert.Pass("Your first passing test");
        }

        [Test]
        public void SimpleQuery3()
        {
            var QueryObject = new QueryObjectBase { LanguageType = SqlLanguageType.SqlServer };
            var newo = QueryObject
                .Select(new[] { new SelectColumn("Id", "a") })
                .Top(10)
                .From(new FromTable("LinkedInProfile", "dbo", "a"))
                .Join(JoinType.InnerJoin, new JoinTable("LinkedInCompany", "dbo", "b"), new JoinColumn("Id"), ComparisonOperatorType.Equals, new JoinTable("LinkedInProfile", "dbo", "a"), new JoinColumn("LinkedInCompanyId"))
                .WhereAnd(new FilterColumn("Id", "a"), ComparisonOperatorType.Equals, 33)
                .HavingAnd(new FilterColumn("Id", "a"), ComparisonOperatorType.Equals, "MAX(a.[Id])")
                .OrderBy("Id", OrderType.Descending, "a")
                .GroupBy(new[] { new GroupByColumn("Id", "a") })
                ;
            var sql = newo.BuildQuery();
            Assert.IsNotNull(sql);
            Assert.Pass("Your first passing test");
        }


        [Test]
        public void SimpleQuery4()
        {
            var QueryObject = new QueryObjectBase { LanguageType = SqlLanguageType.SqlServer };
            var newo = QueryObject
                .Select(new[] { new SelectColumn("Id", "a") })
                .Top(10)
                .Skip(5)
                .Take(5)
                .From(new FromTable("LinkedInProfile", "dbo", "a"))
                .OrderBy("Id", OrderType.Descending, "a")
                ;
            var sql = newo.BuildQuery();
            Assert.IsNotNull(sql);
            Assert.Pass("Your first passing test");
        }


        [Test]
        public void SimpleQuery5()
        {
            var QueryObject = new QueryObjectBase { LanguageType = SqlLanguageType.SqlServer };
            var newo = QueryObject
                .Select(new[] { new SelectColumn("Id", "a") })
                .Top(10)
                .Skip(5)
                .Take(5)
                .From(new FromTable("LinkedInProfile", "dbo", "a"))
                ;
            var sql = newo.BuildQuery();
            Assert.IsNotNull(sql);
            Assert.Pass("Your first passing test");
        }
    }
}
