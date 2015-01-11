using Business.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Utilities
{
    [TestClass]
    public class EnvironmentTests
    {
        [TestMethod]
        public void GetAssemblyName()
        {
            var name = EnvironmentUtilities.GetAssemblyName();
            Assert.IsNotNull(name);
        }

        [TestMethod]
        public void GetAssemblyLocation()
        {
            var name = EnvironmentUtilities.GetAssemblyLocation();
            Assert.IsNotNull(name);
        }

        [TestMethod]
        public void GetAssemblyDirectory()
        {
            var name = EnvironmentUtilities.GetAssemblyDirectory();
            Assert.IsNotNull(name);
        }

        [TestMethod]
        public void GetAssemblyDirectoryInfo()
        {
            var name = EnvironmentUtilities.GetAssemblyDirectoryInfo();
            Assert.IsNotNull(name);
        }
    }
}