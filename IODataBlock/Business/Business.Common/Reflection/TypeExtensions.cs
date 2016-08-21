using Fasterflect;
using System;
using System.Dynamic;
using System.Runtime.CompilerServices;

namespace Business.Common.Reflection
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether [is anonymous type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsAnonymousType(this Type type)
        {
            var hasCompilerGeneratedAttribute = type.GetCustomAttributes(typeof(CompilerGeneratedAttribute), false).Length <= 0;
            var nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            var isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;
            return isAnonymousType;
        }

        /// <summary>
        /// Determines whether [is dynamic type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsDynamicType(this Type type)
        {
            return type.InheritsOrImplements<IDynamicMetaObjectProvider>();
        }

        /// <summary>
        /// Determines whether [is anonymous type] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool IsAnonymousType(this object value)
        {
            return value.GetType().IsAnonymousType();
        }

        /// <summary>
        /// Determines whether [is dynamic type] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool IsDynamicType(this object value)
        {
            return value.GetType().IsDynamicType();
        }

        /// <summary>
        /// Determines whether [is anonymous or dynamic type] [the specified type].
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static bool IsAnonymousOrDynamicType(this Type type)
        {
            return type.IsAnonymousType() || type.IsDynamicType();
        }

        /// <summary>
        /// Determines whether [is anonymous or dynamic type] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static bool IsAnonymousOrDynamicType(this object value)
        {
            return value.GetType().IsAnonymousOrDynamicType();
        }
    }
}