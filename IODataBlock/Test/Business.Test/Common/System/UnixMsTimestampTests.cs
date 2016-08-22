using Business.Common.System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Business.Test.Common.System
{
    [TestClass]
    public class UnixMsTimestampTests
    {
        [TestMethod]
        public void GetNowAndBack()
        {
            UnixMsTimestamp ts = 1442314613077L;
            Int64? value = ts;
            Assert.IsNotNull(value);

            DateTime? rv = ts;
            Assert.IsTrue(rv.HasValue);

            var local = rv.Value.ToLocalTime();
            Assert.IsNotNull(local);
        }

        [TestMethod]
        public void GetNowAndBackFromString()
        {
            UnixMsTimestamp ts = "1442314613077";
            Int64? value = ts;
            Assert.IsNotNull(value);

            DateTime? rv = ts;
            Assert.IsTrue(rv.HasValue);

            var local = rv.Value.ToLocalTime();
            Assert.IsNotNull(local);
        }
    }
}