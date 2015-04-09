using System.Reflection;
using Business.Common.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Business.Test.Common
{
    [TestClass]
    public class TypeSearchTests
    {
        [TestMethod]
        public void LoopThroughAllTypesTest()
        {
            var ts = new TypeSearch();
            var types = ts.GetTypesInAssembly(Assembly.GetExecutingAssembly());

            foreach (var type in types)
            {
                var nm = type.AssemblyQualifiedName;
            }
        }

        [TestMethod]
        public void LoopThroughAllMethodsTest()
        {
            var ts = new TypeSearch();
            var types = ts.GetTypesInAssembly(Assembly.GetExecutingAssembly());

            foreach (var type in types)
            {
                foreach (var m in ts.GetMethodsInType(type))
                {
                    var nm = m.Name;
                }
            }
        }

        [TestMethod]
        public void LoopThroughAllTypesInReferencedAssembliesTest()
        {
            var ts = new TypeSearch();
            var types = ts.GetTypesInReferencedAssemblies(Assembly.GetExecutingAssembly());

            foreach (var type in types)
            {
                var nm = type.AssemblyQualifiedName;
            }
        }
    }
}