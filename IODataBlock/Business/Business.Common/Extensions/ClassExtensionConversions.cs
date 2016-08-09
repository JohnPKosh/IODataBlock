using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Fasterflect;

namespace Business.Common.Extensions
{
    public static partial class ClassExtensions
    {
        #region Basic Extension Methods

        // ReSharper disable once UnusedMember.Local
        private static object GetAsUnderlyingType(Enum enval)
        {
            var entype = enval.GetType();
            var undertype = Enum.GetUnderlyingType(entype);
            return Convert.ChangeType(enval, undertype);
        }

        public static Object GetAsType(this IConvertible obj, String typeCodeName)
        {
            return Convert.ChangeType(obj, (TypeCode)Enum.Parse(typeof(TypeCode), typeCodeName));
        }

        public static TypeCode GetTypeCode(this IConvertible obj)
        {
            var typeCode = Type.GetTypeCode(obj.GetType());
            return typeCode;
        }

        // var myo = (Object)"55555";

        //var obj = new MyValue();
        //obj.valu = myo;
        //obj.TypeName = "Int32";

        //var objValConverted = Convert.ChangeType(obj.valu, (TypeCode)Enum.Parse(typeof(TypeCode), obj.TypeName));

        //if (objValConverted != null)
        //{
        //    Console.WriteLine("exists");
        //}

        public static String GetTypeCodeAsString(this IConvertible obj)
        {
            return GetTypeCode(obj).ToString();
        }

        public static Boolean IsNull<T>(this T obj) where T : class
        {
            return obj == null;
        }

        public static void ThrowIfNull<T>(this T obj) where T : class
        {
            if (obj != null) return;
            var typ = typeof(T);
            throw new NullReferenceException("Object  reference not set to an instance of an object. " + typ.AssemblyQualifiedName);
        }

        public static T To<T>(this IConvertible obj)
        {
            var t = typeof(T);

            if (!t.IsGenericType || (t.GetGenericTypeDefinition() != typeof(Nullable<>)))
                return (T)Convert.ChangeType(obj, t);
            if (obj == null)
            {
                return (T)(object)null;
            }
            return (T)Convert.ChangeType(obj, Nullable.GetUnderlyingType(t));
        }

        public static T ToOrDefault<T>(this IConvertible obj)
        {
            try
            {
                return To<T>(obj);
            }
            catch
            {
                return default(T);
            }
        }

        public static bool ToOrDefault<T>(this IConvertible obj, out T newObj)
        {
            try
            {
                newObj = To<T>(obj);
                return true;
            }
            catch
            {
                newObj = default(T);
                return false;
            }
        }

        public static T ToOrOther<T>(this IConvertible obj, T other = default(T))
        {
            try
            {
                return To<T>(obj);
            }
            catch
            {
                return other;
            }
        }

        public static bool ToOrOther<T>(this IConvertible obj, out T newObj, T other)
        {
            try
            {
                newObj = To<T>(obj);
                return true;
            }
            catch
            {
                newObj = other;
                return false;
            }
        }

        public static T ToOrNull<T>(this IConvertible obj) where T : class
        {
            try
            {
                return To<T>(obj);
            }
            catch
            {
                return null;
            }
        }

        public static bool ToOrNull<T>(this IConvertible obj, out T newObj) where T : class
        {
            try
            {
                newObj = To<T>(obj);
                return true;
            }
            catch
            {
                newObj = null;
                return false;
            }
        }

        public static Type GetPropertyType(this Object obj, String propertyName)
        {
            var propInfo = obj.GetType().GetProperty(propertyName);
            return propInfo.GetType();
        }

        public static T GetPropertyValue<T>(this Object obj, String propertyName)
        {
            var propInfo = obj.GetType().GetProperty(propertyName);
            return (T)Convert.ChangeType(propInfo.GetValue(obj, null), typeof(T));
        }

        public static void SetPropertyValue(this Object obj, String propertyName, Object value)
        {
            obj.GetType().GetProperty(propertyName).SetValue(obj, value, null);
        }

        #region New Basic Methods

        public static String AsString(this IConvertible obj)
        {
            return AsString(obj, true);
        }

        public static String AsString(this IConvertible obj, Boolean asEmpty)
        {
            if (obj != null) return obj.ToString(CultureInfo.InvariantCulture);
            return asEmpty ? String.Empty : null;
        }

        public static T ParseAs<T>(this String obj)
        {
            var t = typeof(T);

            if (!t.IsGenericType || (t.GetGenericTypeDefinition() != typeof(Nullable<>)))
                return (T)Convert.ChangeType(obj.Trim(), t);
            if (String.IsNullOrEmpty(obj.Trim()) || obj.Trim().ToUpper() == "NULL")
            {
                return (T)(object)null;
            }
            return (T)Convert.ChangeType(obj.Trim(), Nullable.GetUnderlyingType(t));
        }

        public static Object ParseAs(this String obj, Type t)
        {
            //Type t = typeof(T);
            if (t.IsEnum)
            {
                return Enum.Parse(t, obj);
            }
            if (!t.IsGenericType || (t.GetGenericTypeDefinition() != typeof(Nullable<>)))
                return Convert.ChangeType(obj.Trim(), t);
            if (String.IsNullOrEmpty(obj.Trim()) || obj.Trim().ToUpper() == "NULL")
            {
                return null;
            }
            return Convert.ChangeType(obj.Trim(), Nullable.GetUnderlyingType(t));
        }

        public static T ParseAsOrDefault<T>(this String obj)
        {
            try
            {
                return ParseAs<T>(obj);
            }
            catch
            {
                return default(T);
            }
        }

        public static bool ParseAsOrDefault<T>(this String obj, out T newObj)
        {
            try
            {
                newObj = ParseAs<T>(obj);
                return true;
            }
            catch
            {
                newObj = default(T);
                return false;
            }
        }

        public static T ParseAsOrOther<T>(this String obj, T other)
        {
            try
            {
                return ParseAs<T>(obj);
            }
            catch
            {
                return other;
            }
        }

        public static bool ParseAsOrOther<T>(this String obj, out T newObj, T other)
        {
            try
            {
                newObj = ParseAs<T>(obj);
                return true;
            }
            catch
            {
                newObj = other;
                return false;
            }
        }

        public static string ToBitStringFromBool(this bool obj)
        {
            return obj ? "1" : "0";
        }

        public static string ToNullWhenEmpty(this string obj)
        {
            return obj.Trim() == "" ? null : obj;
        }

        #endregion New Basic Methods

        #endregion Basic Extension Methods

        #region ToString Formatted

        public static String ToStringWithFormat(this IConvertible obj, ref Dictionary<Type, Func<Object, String>> formatDictionary)
        {
            var t = obj.GetType();

            if (!t.IsGenericType || (t.GetGenericTypeDefinition() != typeof(Nullable<>)))
                return formatDictionary.ContainsKey(t)
                    ? formatDictionary[t](Convert.ChangeType(obj, t))
                    : Convert.ChangeType(obj, t).ToString();
            var ut = Nullable.GetUnderlyingType(t);
            return formatDictionary.ContainsKey(ut) ? formatDictionary[ut](Convert.ChangeType(obj, Nullable.GetUnderlyingType(t))) : Convert.ChangeType(obj, Nullable.GetUnderlyingType(t)).ToString();
        }

        #endregion ToString Formatted

        #region Dynamic Extension Methods

        #region Convert To Expando Methods

        public static ExpandoObject ToExpando(this object anonymousObject)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
            {
                var obj = propertyDescriptor.GetValue(anonymousObject);
                expando.Add(propertyDescriptor.Name, obj);
            }
            return (ExpandoObject)expando;
        }

        public static ExpandoObject ToExpando(this object anonymousObject, IDictionary<Type, Func<Object, Object>> propertyTypeConverters)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(anonymousObject))
            {
                var t = propertyDescriptor.PropertyType;
                var obj = propertyDescriptor.GetValue(anonymousObject);
                if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
                {
                    if (obj == null)
                    {
                        expando.Add(propertyDescriptor.Name, null);
                    }
                    else
                    {
                        var ut = Nullable.GetUnderlyingType(t);
                        expando.Add(propertyDescriptor.Name,
                            propertyTypeConverters.ContainsKey(ut)
                                ? propertyTypeConverters[ut](Convert.ChangeType(obj, Nullable.GetUnderlyingType(t)))
                                : Convert.ChangeType(obj, Nullable.GetUnderlyingType(t)));
                    }
                }
                else
                {
                    expando.Add(propertyDescriptor.Name,
                        propertyTypeConverters.ContainsKey(t) ? propertyTypeConverters[t](obj) : obj);
                }
            }
            return (ExpandoObject)expando;
        }

        public static ExpandoObject ToExpando(this IDictionary<string, object> values)
        {
            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var i in values)
            {
                expando.Add(i.Key, i.Value);
            }
            return (ExpandoObject)expando;
        }

        #endregion Convert To Expando Methods

        #region Convert From Dynamic Methods

        public static void ConvertFromDynamic(this object anonymousObject
            , dynamic source
            , Action<dynamic> inputConverter = null
            , IEnumerable<Type> ignorePropertyTypes = null
            )
        {
            if (inputConverter != null)
            {
                inputConverter.Invoke(source);
            }
            var values = source as IDictionary<string, object>;
            if (values == null) return;

            foreach (var item in anonymousObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!values.ContainsKey(item.Name)) continue;
                var val = values[item.Name];
                if (val == null) continue;

                var prop = anonymousObject.GetType().GetProperty(item.Name);
                var t = prop.PropertyType;
                var v = val.GetType();

                if (t == v)
                {
                    prop.SetValue(anonymousObject, val, null);
                }
                else
                {
                    if (ignorePropertyTypes != null)
                    {
                        // ReSharper disable once PossibleMultipleEnumeration
                        if (ignorePropertyTypes.Contains(v)) continue;
                        if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            prop.SetValue(anonymousObject, Convert.ChangeType(val, Nullable.GetUnderlyingType(t)), null);
                        }
                        else
                        {
                            prop.SetValue(anonymousObject, v == typeof(string) ? ((string)val).ParseAs(t) : Convert.ChangeType(val, t), null);
                        }
                    }
                    else
                    {
                        if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            prop.SetValue(anonymousObject, Convert.ChangeType(val, Nullable.GetUnderlyingType(t)), null);
                        }
                        else
                        {
                            prop.SetValue(anonymousObject, v == typeof(string) ? ((string)val).ParseAs(t) : Convert.ChangeType(val, t), null);
                        }
                    }
                }
            }
        }

        //public static void ConvertFromDynamic(this object anonymousObject
        //    , dynamic source
        //    , Action<IDictionary<string, object>> inputConverter = null
        //    , IEnumerable<Type> ignorePropertyTypes = null
        //    )
        //{
        //    var values = source as IDictionary<string, object>;
        //    if (inputConverter != null)
        //    {
        //        inputConverter.Invoke(values);
        //    }
        //    if (values == null) return;

        //    foreach (var item in anonymousObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
        //    {
        //        if (!values.ContainsKey(item.Name)) continue;
        //        var val = values[item.Name];
        //        if (val == null) continue;

        //        var prop = anonymousObject.GetType().GetProperty(item.Name);
        //        var t = prop.PropertyType;
        //        var v = val.GetType();

        //        if (t == v)
        //        {
        //            prop.SetValue(anonymousObject, val, null);
        //        }
        //        else
        //        {
        //            if (ignorePropertyTypes != null)
        //            {
        //                // ReSharper disable once PossibleMultipleEnumeration
        //                if (ignorePropertyTypes.Contains(v)) continue;
        //                if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
        //                {
        //                    prop.SetValue(anonymousObject, Convert.ChangeType(val, Nullable.GetUnderlyingType(t)), null);
        //                }
        //                else
        //                {
        //                    prop.SetValue(anonymousObject, v == typeof(string) ? ((string)val).ParseAs(t) : Convert.ChangeType(val, t), null);
        //                }
        //            }
        //            else
        //            {
        //                if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
        //                {
        //                    prop.SetValue(anonymousObject, Convert.ChangeType(val, Nullable.GetUnderlyingType(t)), null);
        //                }
        //                else
        //                {
        //                    prop.SetValue(anonymousObject, v == typeof(string) ? ((string)val).ParseAs(t) : Convert.ChangeType(val, t), null);
        //                }
        //            }
        //        }
        //    }
        //}

        public static void ConvertFromIDictionary(this object anonymousObject
            , IDictionary<string, object> values
            , Action<IDictionary<string, object>> inputConverter = null
            , IEnumerable<Type> ignorePropertyTypes = null
            , Dictionary<Type, Func<Object, Object>> typeConversionDictionary = null
            )
        {
            if (inputConverter != null)
            {
                inputConverter.Invoke(values);
            }
            if (values == null) return;

            foreach (var item in anonymousObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!values.ContainsKey(item.Name)) continue;
                var val = values[item.Name];
                if (val == null) continue;

                var prop = anonymousObject.GetType().GetProperty(item.Name);
                var t = prop.PropertyType;
                var v = val.GetType();

                if (t == v)
                {
                    prop.SetValue(anonymousObject, val, null);
                }
                else
                {
                    if (ignorePropertyTypes != null)
                    {
                        // ReSharper disable once PossibleMultipleEnumeration
                        if (ignorePropertyTypes.Contains(v)) continue;
                        if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            prop.SetValue(anonymousObject, Convert.ChangeType(val, Nullable.GetUnderlyingType(t)), null);
                        }
                        else
                        {
                            prop.SetValue(anonymousObject, v == typeof(string) ? ((string)val).ParseAs(t) : Convert.ChangeType(val, t), null);
                        }
                    }
                    else
                    {
                        if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            prop.SetValue(anonymousObject, Convert.ChangeType(val, Nullable.GetUnderlyingType(t)), null);
                        }
                        else
                        {
                            prop.SetValue(anonymousObject, v == typeof(string) ? ((string)val).ParseAs(t) : Convert.ChangeType(val, t), null);
                        }
                    }
                }
            }
        }

        #endregion Convert From Dynamic Methods

        #region NEW Dynamic to Static Typed Objects

        public static T ConvertDynamicTo<T>(this object source
            , Action<dynamic> inputConverter = null
            , IEnumerable<Type> ignorePropertyTypes = null)
        {
            dynamic input = source;
            return (input as IDictionary<string, object>).ConvertTo<T>(inputConverter, ignorePropertyTypes);
        }

        public static T ConvertTo<T>(this ExpandoObject source
            , Action<dynamic> inputConverter = null
            , IEnumerable<Type> ignorePropertyTypes = null)
        {
            return (source as IDictionary<string, object>).ConvertTo<T>(inputConverter, ignorePropertyTypes);
        }

        public static T ConvertTo<T>(this IDictionary<string, object> source
            , Action<dynamic> inputConverter = null
            , IEnumerable<Type> ignorePropertyTypes = null)
        {
            var anonymousObject = Activator.CreateInstance<T>();

            if (inputConverter != null)
            {
                inputConverter.Invoke(source);
            }
            var values = source;
            if (values == null) return anonymousObject;

            foreach (var item in anonymousObject.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                if (!values.ContainsKey(item.Name)) continue;
                var val = values[item.Name];
                if (val == null) continue;

                var prop = anonymousObject.GetType().GetProperty(item.Name);
                var t = prop.PropertyType;
                var v = val.GetType();

                if (t == v)
                {
                    prop.SetValue(anonymousObject, val, null);
                }
                else if (v.InheritsOrImplements<IList>())
                {
                    var list = val as IList;
                    if (list == null) continue;
                    var ti = t.GenericTypeArguments[0];
                    var vi = v.GenericTypeArguments[0];
                    var propertyList = t.CreateInstance(Flags.Default, list.Count) as IList;

                    foreach (var o in list)
                    {
                        if (ti.IsGenericType && (ti.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            if (propertyList != null)
                                propertyList.Add(Convert.ChangeType(o, Nullable.GetUnderlyingType(ti)));
                        }
                        else if (ti.Namespace != null && (ti.IsClass && !ti.Namespace.StartsWith("System")))
                        {
                            var newti = ti.CreateInstance();
                            newti.ConvertFromDynamic(o);
                            if (propertyList != null) propertyList.Add(newti);
                        }
                        else
                        {
                            if (propertyList != null)
                                propertyList.Add(vi == typeof(string) ? ((string)o).ParseAs(ti) : Convert.ChangeType(o, ti));
                        }
                    }
                    prop.SetValue(anonymousObject, propertyList, null);
                }
                else
                {
                    if (ignorePropertyTypes != null)
                    {
                        // ReSharper disable once PossibleMultipleEnumeration
                        if (ignorePropertyTypes.Contains(v)) continue;
                        if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            prop.SetValue(anonymousObject, Convert.ChangeType(val, Nullable.GetUnderlyingType(t)), null);
                        }
                        else
                        {
                            prop.SetValue(anonymousObject, v == typeof(string) ? ((string)val).ParseAs(t) : Convert.ChangeType(val, t), null);
                        }
                    }
                    else
                    {
                        if (t.IsGenericType && (t.GetGenericTypeDefinition() == typeof(Nullable<>)))
                        {
                            prop.SetValue(anonymousObject, Convert.ChangeType(val, Nullable.GetUnderlyingType(t)), null);
                        }
                        else if (t.IsGenericType)
                        {
                            var newti = t.CreateInstance();
                            newti.ConvertFromDynamic(val);
                            prop.SetValue(anonymousObject, newti, null);
                        }
                        else
                        {
                            prop.SetValue(anonymousObject, v == typeof(string) ? ((string)val).ParseAs(t) : Convert.ChangeType(val, t), null);
                        }
                    }
                }
            }
            return anonymousObject;
        }

        #endregion NEW Dynamic to Static Typed Objects

        public static IEnumerable<Dictionary<String, Object>> ToIEnumerableDictionaryObjects(this IEnumerable<dynamic> data)
        {
            return data.Cast<IDictionary<string, object>>().Select(d => d.ToDictionary(x => x.Key, x => x.Value));
        }

        public static IEnumerable<Dictionary<String, String>> ToIEnumerableDictionaryStrings(this IEnumerable<dynamic> data)
        {
            return
                data.Cast<IDictionary<string, object>>()
                    .Select(d => d.ToDictionary(x => x.Key, x => x.Value.ToString()));
        }

        #endregion Dynamic Extension Methods
    }
}
