using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;

namespace Business.Common.Extensions
{
    public static partial class ClassExtensions
    {
        public static void AddMember<T>(this ExpandoObject source, string key, T value)
        {
            source.AsDictionary()[key] = value;
        }

        public static bool TryAddMember<T>(this ExpandoObject source, string key, T value)
        {
            try
            {
                var p = source.AsDictionary();
                if (string.IsNullOrWhiteSpace(key) || p.ContainsKey(key)) return false;
                p[key] = value;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static void DeleteMember(this ExpandoObject source, string key)
        {
            var p = source.AsDictionary();
            p.Remove(key);
        }

        public static bool TryDeleteMember(this ExpandoObject source, string key)
        {
            try
            {
                source.DeleteMember(key);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        internal static bool TryGetObject(this ExpandoObject source, string key, out object value)
        {
            return source.AsDictionary().TryGetValue(key, out value);
        }

        public static bool TryGetValue<T>(this ExpandoObject source, string key, out T value)
        {
            try
            {
                object v = default(T);
                var rv = source.TryGetObject(key, out v);
                value = (T)v;
                return rv;
            }
            catch (Exception)
            {
                value = default(T);
                return false;
            }
        }

        public static IEnumerable<string> GetMetaMemberNames(this ExpandoObject source)
        {
            var tTarget = source as IDynamicMetaObjectProvider;
            return source != null ? tTarget.GetMetaObject(Expression.Constant(tTarget)).GetDynamicMemberNames() : null;
        }

        public static IDictionary<string, Type> GetMemberTypes(this ExpandoObject source)
        {
            // TODO: the value is null then what friggin type will it be? Can or should some type of non-nullable type be used?
            var rv = source.AsDictionary().ToDictionary(o => o.Key, o => o.Value?.GetType());
            return rv;
        }

        public static bool HasMember(this ExpandoObject source, string key)
        {
            return source.AsDictionary().ContainsKey(key);
        }

        public static IDictionary<string, object> AsDictionary(this ExpandoObject source)
        {
            return source;
        }
    }
}