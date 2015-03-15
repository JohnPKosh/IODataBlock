using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Business.Common.System;
using Fasterflect;

namespace Business.Common.Reflection
{
    public class TypeSearch
    {

        public IList<Type> GetTypesInAssembly(Assembly assembly)
        {
            return assembly.Types();
        }

        public IList<Type> GetICommandTypesInAssembly(Assembly assembly)
        {
            return assembly.Types().Where(x => x.InheritsOrImplements<ICommandObject>()).ToList();
        }

        public IList<MethodInfo> GetMethodsInType(Type type)
        {
            return type.Methods();
        }

        public IList<Type> GetTypesInReferencedAssemblies(Assembly assembly)
        {
            var rv = new List<Type>();
            foreach (var a in assembly.GetReferencedAssemblies())
            {
                rv.AddRange(Assembly.Load(a).Types());
            }
            return rv;
        }

        public IList<Type> GetICommandTypesInReferencedAssemblies(Assembly assembly)
        {
            var rv = new List<Type>();
            foreach (var a in assembly.GetReferencedAssemblies())
            {
                rv.AddRange(Assembly.Load(a).Types().Where(x => x.InheritsOrImplements<ICommandObject>()));
            }
            return rv;
        }
    }
}
