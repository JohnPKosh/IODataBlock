using System;
using System.Dynamic;
using System.Linq;
using System.Runtime.CompilerServices;
using Fasterflect;

namespace Business.Common.Reflection
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether [is anonymous type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Boolean IsAnonymousType(this Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Count() > 0;
            var nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            var isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;
            return isAnonymousType;
        }

        /// <summary>
        /// Determines whether [is dynamic type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Boolean IsDynamicType(this Type type)
        {
            return type.InheritsOrImplements<IDynamicMetaObjectProvider>();
        }

        /// <summary>
        /// Determines whether [is anonymous type] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean IsAnonymousType(this object value)
        {
            return value.GetType().IsAnonymousType();
        }

        /// <summary>
        /// Determines whether [is dynamic type] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean IsDynamicType(this object value)
        {
            return value.GetType().IsDynamicType();
        }

        /// <summary>
        /// Determines whether [is anonymous or dynamic type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static Boolean IsAnonymousOrDynamicType(this Type type)
        {
            return type.IsAnonymousType() || type.IsDynamicType();
        }

        /// <summary>
        /// Determines whether [is anonymous or dynamic type] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static Boolean IsAnonymousOrDynamicType(this object value)
        {
            return value.GetType().IsAnonymousOrDynamicType();
        }
    }
}