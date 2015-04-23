using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using Business.Common.System.Args;

// ReSharper disable once CheckNamespace
namespace ExBaseArguments
{
    /// <summary>
    /// Extension method helper class.
    /// </summary>
    public static class ArgumentsExtensionBase
    {
        #region Private Properties

        private const Char E1 = '\u00BE';
        private const Char E2 = '\u00B6';

        private const String UnEscapedSlash = @"\/";
        private static readonly String EscapedSlash = (E1) + (E2).ToString(CultureInfo.InvariantCulture);
        private const String UnEscapedDash = @"\-";
        private static readonly String EscapedDash = (E2) + (E1).ToString(CultureInfo.InvariantCulture);

        #endregion Private Properties

        #region Helper Methods

        public static String EscapeArgs(this String argumentString)
        {
            return argumentString.Replace(UnEscapedSlash, EscapedSlash).Replace(UnEscapedDash, EscapedDash);
        }

        public static String UnEscapeArgs(this String argumentString)
        {
            return argumentString.Replace(EscapedSlash, "/").Replace(EscapedDash, "-");
        }

        #endregion Helper Methods

        #region Conversion Methods

        /// <summary>
        /// Extension method to retrieve the Arguments values as a String[].
        /// </summary>
        /// <param name="value"></param>
        /// <returns>Returns a String[] of preprocessed args from the Arguments Dictionary.</returns>
        public static String[] ToValueArray(this Arguments value)
        {
            return value.Values.ToArray();
        }

        /// <summary>
        /// Extension method to convert a String[] to Int32[].
        /// </summary>
        /// <param name="value">The String[] to convert.</param>
        /// <param name="throwOnInvalidCast">
        /// If true an exception will be thrown if the value cannot be converted. False will skip
        /// errors and return any converted values.
        /// </param>
        /// <returns>Int32[]</returns>
        public static Int32[] ToIntArray(this String[] value, Boolean throwOnInvalidCast)
        {
            if (value == null) return null;
            if (throwOnInvalidCast)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                return value.Cast<Int32>().ToArray();
            }
            var tempout = 0;
            return (from a in value where Int32.TryParse(a, out tempout) select tempout).ToArray();
        }

        /// <summary>
        /// Extension method to convert a String[] to DateTime[].
        /// </summary>
        /// <param name="value">The String[] to convert.</param>
        /// <param name="throwOnInvalidCast">
        /// If true an exception will be thrown if the value cannot be converted. False will skip
        /// errors and return any converted values.
        /// </param>
        /// <returns>DateTime[]</returns>
        public static DateTime[] ToDateTimeArray(this String[] value, Boolean throwOnInvalidCast)
        {
            if (value == null) return null;
            if (throwOnInvalidCast)
            {
                // ReSharper disable once SuspiciousTypeConversion.Global
                return value.Cast<DateTime>().ToArray();
            }
            var tempout = new DateTime();
            return (from a in value where DateTime.TryParse(a, out tempout) select tempout).ToArray();
        }

        /// <summary>
        /// To expando.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static dynamic ToExpando(this Arguments args)
        {
            dynamic rv = new ExpandoObject();
            var d = rv as IDictionary<string, object>;

            foreach (var arg in args)
            {
                d.Add(arg.K, arg.V);
            }
            return rv;
        }

        /// <summary>
        /// To the name value collection.
        /// </summary>
        /// <param name="args">The args.</param>
        /// <returns></returns>
        public static NameValueCollection ToNameValueCollection(this Arguments args)
        {
            var rv = new NameValueCollection(args.Count);
            foreach (var a in args)
            {
                rv.Add(a.K, a.V);
            }
            return rv;
        }

        #endregion Conversion Methods

        #region Validation Methods

        public static void RequireOneOfTheseArgs(this Arguments value, params String[] names)
        {
            value.RequireOneOfTheseArgs(names.AsEnumerable());
        }

        public static void RequireOneOfTheseArgs(this Arguments value, IEnumerable<String> arglist)
        {
            var args = arglist.ToList();
            var exists = args.Any(value.ContainsKey);
            if (exists) return;
            var argstr = String.Join("' or '", args.ToArray());
            throw new Exception("RecurseWriter.Write(): '" + argstr + "' Missing Argument(s) Error! No matching argument was found!");
        }

        public static void RequireAllOfTheseArgs(this Arguments value, params String[] names)
        {
            value.RequireAllOfTheseArgs(names.AsEnumerable());
        }

        public static void RequireAllOfTheseArgs(this Arguments value, IEnumerable<String> arglist)
        {
            var args = arglist.ToList();
            var missing = false;
            var missinglist = new List<String>();
            foreach (var a in args.Where(a => !value.ContainsKey(a)))
            {
                missing = true;
                missinglist.Add(a);
            }
            if (!missing && arglist != null && args.Any()) return;
            var argstr = String.Join("', '", missinglist.ToArray());
            throw new Exception("RecurseWriter.Write(): ['" + argstr + "'] Required Argument(s) Error! Not all required arguments exist!");
        }

        public static void RequireAllOfTheseArgs(this Arguments value, IEnumerable<IEnumerable<String>> arglist)
        {
            var args = arglist.ToList();
            var missing = false;
            var missinglist = new List<String>();
            foreach (var a in args.Where(a => !value.ContainsOneOfTheseKeys(a)))
            {
                missing = true;
                missinglist.Add(String.Join(" or ", a.ToArray()));
            }
            if (!missing && args.Any()) return;
            var argstr = String.Join("', '", missinglist.ToArray());
            throw new Exception("RecurseWriter.Write(): ['" + argstr + "'] Required Argument(s) Error! Not all required arguments exist!");
        }

        public static Boolean ContainsOneOfTheseArgs(this Arguments value, params String[] names)
        {
            return value.ContainsOneOfTheseArgs(names.AsEnumerable());
        }

        public static Boolean ContainsOneOfTheseArgs(this Arguments value, IEnumerable<String> arglist)
        {
            return arglist.Any(value.ContainsKey);
        }

        public static Boolean ContainsAllOfTheseArgs(this Arguments value, params String[] names)
        {
            return value.ContainsAllOfTheseArgs(names.AsEnumerable());
        }

        public static Boolean ContainsAllOfTheseArgs(this Arguments value, IEnumerable<String> arglist)
        {
            var args = arglist.ToList();
            var missing = args.Any(a => !value.ContainsKey(a));
            return !missing && args.Any();
        }

        public static Boolean ContainsAllOfTheseArgs(this Arguments value, IEnumerable<IEnumerable<String>> arglist)
        {
            var args = arglist.ToList();
            var missing = args.Any(a => !value.ContainsOneOfTheseKeys(a));
            return !missing && args.Any();
        }

        public static Boolean ContainsOneOfTheseKeys<T>(this Dictionary<String, T> value, IEnumerable<String> arglist)
        {
            return arglist.Any(value.ContainsKey);
        }

        // Same as above: choose one???????
        public static Boolean ContainsKey<T>(this Dictionary<String, T> value, IEnumerable<String> arglist)
        {
            return arglist.Any(value.ContainsKey);
        }

        #endregion Validation Methods

        #region Unused Code

        ///// <summary>
        ///// Extension method to trim matching quotes from the start and end of a string.
        ///// </summary>
        ///// <param name="value">The string to trim.</param>
        ///// <param name="quote">Char to trim on.</param>
        ///// <returns>String</returns>
        //public static String TrimMatchingQuotes(this string value, char quote)
        //{
        //    if ((value.Length >= 2) && (value[0] == quote) && (value[value.Length - 1] == quote))
        //    {
        //        return value.Substring(1, value.Length - 2);
        //    }
        //    return value;
        //}

        //public static IEnumerable<string> SplitCommandLine(this string commandLine)
        //{
        //    bool inQuotes = false;
        //    return commandLine.Split(x =>
        //    {
        //        if (x == '\"')inQuotes = !inQuotes;
        //        return !inQuotes && x == ' ';
        //    }).Select(arg => arg.Trim().TrimMatchingQuotes('\"'))
        //    .Where(arg => !string.IsNullOrEmpty(arg));
        //}

        //public static IEnumerable<string> Split(this string str, Func<char, bool> controller)
        //{
        //    int nextPiece = 0;
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        if (controller(str[i]))
        //        {
        //            yield return str.Substring(nextPiece, i - nextPiece);
        //            nextPiece = i + 1;
        //        }
        //    }
        //    yield return str.Substring(nextPiece);
        //}

        #endregion Unused Code
    }
}