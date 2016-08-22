using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text.RegularExpressions;

namespace Business.Common.Extensions
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
        public static string ReplaceNumberParameters(this string value,
            string startTag = @"@",
            string endTag = null,
            string quotedIdentifier = @"'",
            IEnumerable<object> numberedArgs = null)
        {
            return numberedArgs == null ? value : value.ReplaceNumberParametersInternal(startTag, endTag, quotedIdentifier, numberedArgs.ToArray());
        }

        private static string ReplaceNumberParametersInternal(this string value,
            string startTag = @"@",
            string endTag = null,
            string quotedIdentifier = @"'",
            params object[] numberedArgs)
        {
            if (numberedArgs == null) return value;
            startTag = string.IsNullOrWhiteSpace(startTag) ? string.Empty : startTag;
            endTag = string.IsNullOrWhiteSpace(endTag) ? string.Empty : endTag;
            for (var i = 0; i < numberedArgs.Length; i++)
            {
                var arg = numberedArgs[i];
                if (!string.IsNullOrWhiteSpace(quotedIdentifier) && numberedArgs[i] is string)
                {
                    arg = quotedIdentifier + numberedArgs[i] + quotedIdentifier;
                }
                value = value.Replace($@"{startTag}{i}{endTag}", arg.ToString());
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
        public static string ReplaceNamedParameters(this string value,
            IDictionary<string, string> namedArgs,
            string startTag = null,
            string endTag = null
            )
        {
            if (namedArgs == null) return value;
            startTag = string.IsNullOrWhiteSpace(startTag) ? string.Empty : startTag;
            endTag = string.IsNullOrWhiteSpace(endTag) ? string.Empty : endTag;
            return namedArgs.Aggregate(value, (current, k) => current.Replace($@"{startTag}{k.Key}{endTag}", k.Value));
        }

        /// <summary>
        /// Replaces the named parameters.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="namedArgs">The named arguments.</param>
        /// <param name="startTag">The start tag.</param>
        /// <param name="endTag">The end tag.</param>
        /// <returns></returns>
        public static string ReplaceNamedParameters(this string value,
            NameValueCollection namedArgs,
            string startTag = null,
            string endTag = null
            )
        {
            if (namedArgs == null) return value;
            startTag = string.IsNullOrWhiteSpace(startTag) ? string.Empty : startTag;
            endTag = string.IsNullOrWhiteSpace(endTag) ? string.Empty : endTag;
            return namedArgs.Cast<KeyValuePair<string, string>>().Aggregate(value, (current, k) => current.Replace(
                $@"{startTag}{k.Key}{endTag}", k.Value));
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
        public static string ReplaceNamedParameters(this string value,
            string namedArgString,
            string startTag = @"$(",
            string endTag = @")",
            string parameterSeparator = ";",
            string valueSeparator = "="
            )
        {
            var paramlist = namedArgString.ParseAsNameValueCollection(parameterSeparator, valueSeparator);
            // ReSharper disable once AssignNullToNotNullAttribute
            return paramlist == null ? value : paramlist.Keys.Cast<string>().Aggregate(value, (current1, p) => paramlist.GetValues(p).Aggregate(current1, (current, v) => current.Replace(
                $@"{startTag}{p}{endTag}", v)));
        }

        public static string ReplaceSqlcmdParameters(this string value, IDictionary<string, string> namedArgs)
        {
            return namedArgs == null ? value : namedArgs.Aggregate(value, (current, k) => current.Replace(
                $@"$({k.Key})", k.Value));
        }

        public static string ReplaceSqlcmdParameter(this string value, string key, string replacement)
        {
            return value.Replace($@"$({key})", replacement);
        }

        // is this not same as above????
        public static string ReplaceParameters(this string value, IDictionary<string, string> namedArgs, string startTag = @"$(", string endTag = @")")
        {
            if (namedArgs == null) return value;
            return (from k in namedArgs let p = $@"{startTag}{k.Key}{endTag}" select k).Aggregate(value, (current, k) => current.Replace(
                $@"{startTag}{k.Key}{endTag}", k.Value));
        }

        #endregion Named Parameters

        #region Replace Named Parameters with value collections

        public static string ReplaceNamedParameterByIEnumerableStrings(this string value,
            string namedArg,
            IEnumerable<string> listValues,
            Func<IEnumerable<string>, IEnumerable<string>> valueFormatter = null,
            string valueSeperator = ",\r\n",
            Func<string, string> replacementFormatter = null,
            string startTag = @"$(",
            string endTag = @")"
            )
        {
            if (namedArg == null) return value;
            startTag = string.IsNullOrWhiteSpace(startTag) ? string.Empty : startTag;
            endTag = string.IsNullOrWhiteSpace(endTag) ? string.Empty : endTag;

            var outputItems = valueFormatter != null ? new List<string>(valueFormatter(listValues)) : new List<string>(listValues);

            var outputstring = outputItems.ToDelimitedString(valueSeperator);
            if (replacementFormatter != null) outputstring = replacementFormatter(outputstring);
            value = value.Replace($@"{startTag}{namedArg}{endTag}", outputstring);
            return value;
        }

        public static string ReplaceNamedParameterByIEnumerableObjects(this string value,
            string namedArg,
            IEnumerable<object> listValues,
            Func<IEnumerable<object>, IEnumerable<string>> valueFormatter = null,
            string valueSeperator = ",\r\n",
            Func<string, string> replacementFormatter = null,
            string startTag = @"$(",
            string endTag = @")"
            )
        {
            if (namedArg == null) return value;
            startTag = string.IsNullOrWhiteSpace(startTag) ? string.Empty : startTag;
            endTag = string.IsNullOrWhiteSpace(endTag) ? string.Empty : endTag;

            var outputItems = valueFormatter != null ? new List<string>(valueFormatter(listValues)) : new List<string>(listValues.Select(x => x.ToString()));

            var outputstring = outputItems.ToDelimitedString(valueSeperator);
            if (replacementFormatter != null) outputstring = replacementFormatter(outputstring);
            value = value.Replace($@"{startTag}{namedArg}{endTag}", outputstring);
            return value;
        }

        #region SQL IN Parameter Helpers

        /// <summary>
        /// Converts the value to SQL IN parameter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertValueToSqlInParam(string value)
        {
            return $@"'{value}'";
        }

        /// <summary>
        /// Converts the value to escaped SQL IN parameter.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ConvertValueToEscapedSqlInParam(string value)
        {
            return $@"''{value}''";
        }

        /// <summary>
        /// Converts the values to SQL IN values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<string> ConvertValuesToSqlInValues(IEnumerable<string> values)
        {
            return values.Where(v => !string.IsNullOrWhiteSpace(v)).Select(ConvertValueToSqlInParam);
        }

        /// <summary>
        /// Converts the values to escaped SQL IN values.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static IEnumerable<string> ConvertValuesToEscapedSqlInValues(IEnumerable<string> values)
        {
            return values.Where(v => !string.IsNullOrWhiteSpace(v)).Select(ConvertValueToEscapedSqlInParam);
        }

        /// <summary>
        /// Joins the strings to SQL value string.
        /// </summary>
        /// <param name="values">The values.</param>
        /// <returns></returns>
        public static string JoinStringsToSqlValueString(IEnumerable<string> values)
        {
            return string.Join(",", values.ToArray());
        }

        public static string ReplaceNamedParameterBySqlValueString(this string value, string namedArg, IEnumerable<string> listValues, bool escapeValues = false)
        {
            return escapeValues ?
                ReplaceNamedParameterByIEnumerableStrings(value, namedArg, listValues, ConvertValuesToEscapedSqlInValues, ",")
                : ReplaceNamedParameterByIEnumerableStrings(value, namedArg, listValues, ConvertValuesToSqlInValues, ",");
        }

        #endregion SQL IN Parameter Helpers

        #endregion Replace Named Parameters with value collections

        #region Find Parameters

        public static IEnumerable<string> FindSqlcmdParameters(this string value)
        {
            var rx = new Regex(@"(?<param>\$\([$\w\d]+\))", RegexOptions.IgnoreCase);
            var matches = rx.Matches(value);
            var rv = (from Match m in matches select m.Value.Remove(m.Value.Length - 1).Replace("$(", string.Empty)).ToList();
            return rv.Distinct();
        }

        public static IEnumerable<string> FindParameters(this string value, string startTagRegex = @"\$\(", string endTagRegex = @"\)")
        {
            var rx = new Regex($@"(?<param>{startTagRegex}[$\w\d]+{endTagRegex})", RegexOptions.IgnoreCase);
            var matches = rx.Matches(value);
            var rv = (from Match m in matches select m.Value.Remove(m.Value.Length - 1).Replace("$(", string.Empty)).ToList();
            return rv.Distinct();
        }

        public static string ToParameterString(
            this IDictionary<string, string> value,
            string parameterSeparator = ";",
            string valueSeparator = "=",
            string keyPrefix = null,
            string keySuffix = null,
            string valuePrefix = null,
            string valueSuffix = null
            )
        {
            keyPrefix = string.IsNullOrWhiteSpace(keyPrefix) ? string.Empty : keyPrefix;
            keySuffix = string.IsNullOrWhiteSpace(keySuffix) ? string.Empty : keySuffix;
            valuePrefix = string.IsNullOrWhiteSpace(valuePrefix) ? string.Empty : valuePrefix;
            valueSuffix = string.IsNullOrWhiteSpace(valueSuffix) ? string.Empty : valueSuffix;
            var items = value.Select(v =>
                $"{keyPrefix}{v.Key}{keySuffix}{valueSeparator}{valuePrefix}{v.Value}{valueSuffix}").ToList();
            return string.Join(parameterSeparator, items);
        }

        #endregion Find Parameters
    }
}