using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;
using ExBaseStringUtil;

namespace Business.Utilities.Extensions
{
    /// <summary>
    /// String Parameter Extension Base Methods.
    /// </summary>
    public static class ParameterExtensionBase
    {
        #region Numbered Parameters

        /// <summary>
        /// Replaces the number parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="quotedIdentifier"></param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="startTag"></param>
        /// <param name="endTag"></param>
        /// <returns></returns>
        public static String ReplaceNumberParameters(this String value,
            String startTag = @"@",
            String endTag = null,
            String quotedIdentifier = @"'",
            IEnumerable<Object> numberedArgs = null)
        {
            return numberedArgs == null ? value : value.ReplaceNumberParametersInternal(startTag, endTag, quotedIdentifier, numberedArgs.ToArray());
        }

        private static String ReplaceNumberParametersInternal(this String value,
            String startTag = @"@",
            String endTag = null,
            String quotedIdentifier = @"'",
            params Object[] numberedArgs)
        {
            if (numberedArgs == null) return value;
            startTag = String.IsNullOrWhiteSpace(startTag) ? String.Empty : startTag;
            endTag = String.IsNullOrWhiteSpace(endTag) ? String.Empty : endTag;
            for (var i = 0; i < numberedArgs.Length; i++)
            {
                var arg = numberedArgs[i];
                if (!String.IsNullOrWhiteSpace(quotedIdentifier) && numberedArgs[i] is string)
                {
                    arg = quotedIdentifier + numberedArgs[i] + quotedIdentifier;
                }
                value = value.Replace(String.Format(@"{0}{1}{2}", startTag, i, endTag), arg.ToString());
            }
            return value;
        }

        ///// <summary>
        ///// Replaces the number parameters.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <param name="NumberedArgs">The numbered args.</param>
        ///// <returns></returns>
        //public static String ReplaceNumberParameters(this String value, params Object[] NumberedArgs)
        //{
        //    if (NumberedArgs == null) return value;
        //    for (int i = 0; i < NumberedArgs.Length; i++)
        //    {
        //        var tmp = @"@" + i.ToString();
        //        var arg = typeof(String) == NumberedArgs[i].GetType() ? @"'" + NumberedArgs[i] + @"'" : NumberedArgs[i];
        //        value = value.Replace(tmp, arg.ToString());
        //    }
        //    return value;
        //}

        #endregion Numbered Parameters

        #region Named Parameters

        /// <summary>
        /// Replaces the named parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="namedArgs">The named arguments.</param>
        /// <param name="startTag">The start tag.</param>
        /// <param name="endTag">The end tag.</param>
        /// <returns></returns>
        public static String ReplaceNamedParameters(this String value,
            IDictionary<String, String> namedArgs,
            String startTag = null,
            String endTag = null
            )
        {
            if (namedArgs == null) return value;
            startTag = String.IsNullOrWhiteSpace(startTag) ? String.Empty : startTag;
            endTag = String.IsNullOrWhiteSpace(endTag) ? String.Empty : endTag;
            return namedArgs.Aggregate(value, (current, k) => current.Replace(String.Format(@"{0}{1}{2}", startTag, k.Key, endTag), k.Value));
        }

        /// <summary>
        /// Replaces the named parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="namedArgs">The named arguments.</param>
        /// <param name="startTag">The start tag.</param>
        /// <param name="endTag">The end tag.</param>
        /// <returns></returns>
        public static String ReplaceNamedParameters(this String value,
            NameValueCollection namedArgs,
            String startTag = null,
            String endTag = null
            )
        {
            if (namedArgs == null) return value;
            startTag = String.IsNullOrWhiteSpace(startTag) ? String.Empty : startTag;
            endTag = String.IsNullOrWhiteSpace(endTag) ? String.Empty : endTag;
            return namedArgs.Cast<KeyValuePair<string, string>>().Aggregate(value, (current, k) => current.Replace(String.Format(@"{0}{1}{2}", startTag, k.Key, endTag), k.Value));
        }

        /// <summary>
        /// Replaces the named parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="namedArgString">The named argument string.</param>
        /// <param name="startTag">The start tag.</param>
        /// <param name="endTag">The end tag.</param>
        /// <param name="parameterSeparator">The parameter separator.</param>
        /// <param name="valueSeparator">The value separator.</param>
        /// <returns></returns>
        public static String ReplaceNamedParameters(this String value,
            String namedArgString,
            String startTag = @"$(",
            String endTag = @")",
            String parameterSeparator = ";",
            String valueSeparator = "="
            )
        {
            var paramlist = namedArgString.ParseAsNameValueCollection(parameterSeparator, valueSeparator);
            // ReSharper disable once AssignNullToNotNullAttribute
            return paramlist == null ? value : paramlist.Keys.Cast<string>().Aggregate(value, (current1, p) => paramlist.GetValues(p).Aggregate(current1, (current, v) => current.Replace(String.Format(@"{0}{1}{2}", startTag, p, endTag), v)));
        }

        public static String ReplaceSqlcmdParameters(this String value, IDictionary<String, String> namedArgs)
        {
            return namedArgs == null ? value : namedArgs.Aggregate(value, (current, k) => current.Replace(String.Format(@"$({0})", k.Key), k.Value));
        }

        public static String ReplaceSqlcmdParameter(this String value, String key, String replacement)
        {
            return value.Replace(String.Format(@"$({0})", key), replacement);
        }

        // is this not same as above????
        public static String ReplaceParameters(this String value, IDictionary<String, String> namedArgs, String startTag = @"$(", String endTag = @")")
        {
            if (namedArgs == null) return value;
            return (from k in namedArgs let p = String.Format(@"{0}{1}{2}", startTag, k.Key, endTag) select k).Aggregate(value, (current, k) => current.Replace(String.Format(@"{0}{1}{2}", startTag, k.Key, endTag), k.Value));
        }

        #endregion Named Parameters

        #region Replace Named Parameters with value collections

        public static String ReplaceNamedParameterByIEnumerableStrings(this String value,
            String namedArg,
            IEnumerable<String> listValues,
            Func<IEnumerable<String>, IEnumerable<String>> valueFormatter = null,
            String valueSeperator = ",\r\n",
            Func<String, String> replacementFormatter = null,
            String startTag = @"$(",
            String endTag = @")"
            )
        {
            if (namedArg == null) return value;
            startTag = String.IsNullOrWhiteSpace(startTag) ? String.Empty : startTag;
            endTag = String.IsNullOrWhiteSpace(endTag) ? String.Empty : endTag;

            var outputItems = valueFormatter != null ? new List<string>(valueFormatter(listValues)) : new List<string>(listValues);

            var outputstring = outputItems.ToDelimitedString(valueSeperator);
            if (replacementFormatter != null) outputstring = replacementFormatter(outputstring);
            value = value.Replace(String.Format(@"{0}{1}{2}", startTag, namedArg, endTag), outputstring);
            return value;
        }

        public static String ReplaceNamedParameterByIEnumerableObjects(this String value,
            String namedArg,
            IEnumerable<Object> listValues,
            Func<IEnumerable<Object>, IEnumerable<String>> valueFormatter = null,
            String valueSeperator = ",\r\n",
            Func<String, String> replacementFormatter = null,
            String startTag = @"$(",
            String endTag = @")"
            )
        {
            if (namedArg == null) return value;
            startTag = String.IsNullOrWhiteSpace(startTag) ? String.Empty : startTag;
            endTag = String.IsNullOrWhiteSpace(endTag) ? String.Empty : endTag;

            var outputItems = valueFormatter != null ? new List<string>(valueFormatter(listValues)) : new List<string>(listValues.Select(x => x.ToString()));

            var outputstring = outputItems.ToDelimitedString(valueSeperator);
            if (replacementFormatter != null) outputstring = replacementFormatter(outputstring);
            value = value.Replace(String.Format(@"{0}{1}{2}", startTag, namedArg, endTag), outputstring);
            return value;
        }

        #region SQL IN Parameter Helpers

        /// <summary>
        /// Converts the value to SQL IN parameter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertValueToSqlInParam(String value)
        {
            return String.Format(@"'{0}'", value);
        }

        /// <summary>
        /// Converts the value to escaped SQL IN parameter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertValueToEscapedSqlInParam(String value)
        {
            return String.Format(@"''{0}''", value);
        }

        /// <summary>
        /// Converts the values to SQL IN values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<string> ConvertValuesToSqlInValues(IEnumerable<string> values)
        {
            return values.Where(v => !String.IsNullOrWhiteSpace(v)).Select(ConvertValueToSqlInParam);
        }

        /// <summary>
        /// Converts the values to escaped SQL IN values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<string> ConvertValuesToEscapedSqlInValues(IEnumerable<string> values)
        {
            return values.Where(v => !String.IsNullOrWhiteSpace(v)).Select(ConvertValueToEscapedSqlInParam);
        }

        /// <summary>
        /// Joins the strings to SQL value string.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static string JoinStringsToSqlValueString(IEnumerable<string> values)
        {
            return String.Join(",", values.ToArray());
        }

        public static string ReplaceNamedParameterBySqlValueString(this String value, String namedArg, IEnumerable<String> listValues, Boolean escapeValues = false)
        {
            return escapeValues ?
                ReplaceNamedParameterByIEnumerableStrings(value, namedArg, listValues, ConvertValuesToEscapedSqlInValues, ",")
                : ReplaceNamedParameterByIEnumerableStrings(value, namedArg, listValues, ConvertValuesToSqlInValues, ",");
        }

        #endregion SQL IN Parameter Helpers

        #endregion Replace Named Parameters with value collections

        #region Find Parameters

        public static IEnumerable<String> FindSqlcmdParameters(this String value)
        {
            var rx = new Regex(@"(?<param>\$\([$\w\d]+\))", RegexOptions.IgnoreCase);
            var matches = rx.Matches(value);
            var rv = (from Match m in matches select m.Value.Remove(m.Value.Length - 1).Replace("$(", String.Empty)).ToList();
            return rv.Distinct();
        }

        public static IEnumerable<String> FindParameters(this String value, String startTagRegex = @"\$\(", String endTagRegex = @"\)")
        {
            var rx = new Regex(String.Format(@"(?<param>{0}[$\w\d]+{1})", startTagRegex, endTagRegex), RegexOptions.IgnoreCase);
            var matches = rx.Matches(value);
            var rv = (from Match m in matches select m.Value.Remove(m.Value.Length - 1).Replace("$(", String.Empty)).ToList();
            return rv.Distinct();
        }

        public static String ToParameterString(
            this IDictionary<String, String> value,
            String parameterSeparator = ";",
            String valueSeparator = "=",
            String keyPrefix = null,
            String keySuffix = null,
            String valuePrefix = null,
            String valueSuffix = null
            )
        {
            keyPrefix = String.IsNullOrWhiteSpace(keyPrefix) ? String.Empty : keyPrefix;
            keySuffix = String.IsNullOrWhiteSpace(keySuffix) ? String.Empty : keySuffix;
            valuePrefix = String.IsNullOrWhiteSpace(valuePrefix) ? String.Empty : valuePrefix;
            valueSuffix = String.IsNullOrWhiteSpace(valueSuffix) ? String.Empty : valueSuffix;
            var items = value.Select(v => String.Format("{0}{1}{2}{3}{4}{5}{6}", keyPrefix, v.Key, keySuffix, valueSeparator, valuePrefix, v.Value, valueSuffix)).ToList();
            return String.Join(parameterSeparator, items);
        }

        #endregion Find Parameters
    }
}