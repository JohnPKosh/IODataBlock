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

            var newo = QueryObject.SelectAll().From("LinkedInProfile");
            Trace.WriteLine(newo.FromTable.Value);

            Assert.Pass("Your first passing test");
        }
    }
}
