using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Fluent.Base;
using Data.Fluent.Enums;

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
                .From("LinkedInProfile")
                .Join(JoinType.InnerJoin, "Ids", "Id", ComparisonOperatorType.Equals, "LinkedInProfile", "Id")
                .WhereAnd("Id", ComparisonOperatorType.Equals, 12)

                ;

            var sql = newo.BuildQuery();
            Assert.IsNotNull(sql);

            Assert.Pass("Your first passing test");

            /*
                SELECT Data.Fluent.Model.Schema.SelectColumn
                FROM Data.Fluent.Model.Schema.FromTable 
                INNER JOIN Data.Fluent.Model.Schema.JoinTable 
                ON LinkedInProfile.Data.Fluent.Model.Schema.JoinColumn = Ids.Data.Fluent.Model.Schema.JoinColumn
                WHERE Data.Fluent.Model.Schema.FilterColumn = 12
             */
        }
    }
}
