using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;

namespace Business.Common.Reflection
{
    public static class PropertyExtenstions
    {

        public static Lazy<Dictionary<string, MemberGetter>> GetLazyPropertyInfo<T>(this T o) where T : class
        {
            return new Lazy<Dictionary<string, MemberGetter>>(GetPropertyInformation<T>);
        }

        private static Dictionary<string, MemberGetter> GetPropertyInformation<T>()
        {
            return typeof(T).Properties().ToDictionary(x => x.Name, x => x.DelegateForGetPropertyValue());
        }

    }
}
