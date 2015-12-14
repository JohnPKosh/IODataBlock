using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Business.Common.Extensions
{
    public static class StringExtensionBase
    {
        #region Conversion Methods

        public static String EncodeToString(this Byte[] buffer, Encoding encodingType = null, Int32 index = -1, Int32 cnt = -1)
        {
            if (index <= -1 && cnt <= -1)
                return encodingType == null ? Encoding.Default.GetString(buffer) : encodingType.GetString(buffer);
            if (index == -1) index = 0;
            if (cnt == -1) cnt = buffer.Length - index;
            if (cnt > buffer.Length - index) cnt = buffer.Length - index;
            return encodingType == null ? Encoding.Default.GetString(buffer, index, cnt) : encodingType.GetString(buffer, index, cnt);
        }

        public static SecureString ConvertToSecureString(this String value)
        {
            return new StringBuilder(value).ConvertToSecureString();
        }

        /// <summary>
        /// Converts a string to a SecureString object.
        /// </summary>
        /// <param name="value">The unprotected password returned by CredUI.</param>
        /// <returns>The password as a SecureString object.</returns>
        public static SecureString ConvertToSecureString(this StringBuilder value)
        {
            if (value == null)
                throw new ArgumentNullException(@"value");

            var secureValue = new SecureString();
            for (var i = 0; i < value.Length; i++)
            {
                secureValue.AppendChar(value[i]);
                value[i] = '\x0000';
            }

            secureValue.MakeReadOnly();
            value.Clear();
            return secureValue;
        }

        #endregion Conversion Methods

        #region Basic Methods

        public static StringBuilder AppendIEnumerable<T>(this StringBuilder buf, IEnumerable<T> values, Func<T, String> func, string delimiter)
        {
            return buf.Append(String.Join(delimiter, values.Select(func).ToArray()));
        }

        public static StringBuilder AppendIEnumerable<T>(this StringBuilder buf, string prefix, IEnumerable<T> values, string suffix, string delimiter)
        {
            return buf.AppendIEnumerable(values, x => prefix + x.ToString() + suffix, delimiter);
        }

        public static string DefaultDelimiter = "\t";

        /// <summary>
        /// Convert a sequence of items to a delimited string. By default, ToString() will be called
        /// on each item in the sequence to formulate the result. The default delimiter of ', ' will
        /// be used
        /// </summary>
        public static string ToDelimitedString<T>(this IEnumerable<T> source)
        {
            return source.ToDelimitedString(x => x.ToString(), DefaultDelimiter);
        }

        /// <summary>
        /// Convert a sequence of items to a delimited string. By default, ToString() will be called
        /// on each item in the sequence to formulate the result
        /// </summary>
        /// <param name="source"></param>
        /// <param name="delimiter">The delimiter to separate each item with</param>
        public static string ToDelimitedString<T>(this IEnumerable<T> source, string delimiter)
        {
            return source.ToDelimitedString(x => x.ToString(), delimiter);
        }

        /// <summary>
        /// Convert a sequence of items to a delimited string. The default delimiter of ', ' will be used
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector">
        /// A lambda expression to select a string property of <typeparamref name="T" />
        /// </param>
        public static string ToDelimitedString<T>(this IEnumerable<T> source, Func<T, string> selector)
        {
            return source.ToDelimitedString(selector, DefaultDelimiter);
        }

        /// <summary>
        /// Convert a sequence of items to a delimited string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector">
        /// A lambda expression to select a string property of <typeparamref name="T" />
        /// </param>
        /// <param name="delimiter">The delimiter to separate each item with</param>
        public static string ToDelimitedString<T>(this IEnumerable<T> source, Func<T, string> selector, string delimiter)
        {
            return string.Join(delimiter, source.Select(selector).ToArray());
        }

        /// <summary>
        /// Convert a sequence of items to a delimited string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector">
        /// A lambda expression to select a string property of <typeparamref name="T" />
        /// </param>
        /// <param name="columnWidths"></param>
        public static string ToFixedWidthString<T>(this IEnumerable<T> source, Func<T, string> selector, IEnumerable<Int32> columnWidths)
        {
            var clist = source.Select(selector).ToArray();
            return String.Join(String.Empty, clist.Select((t, i) => t.PadRight(columnWidths.ToArray()[i])).ToArray());
        }

        /// <summary>
        /// Convert a sequence of items to a delimited string.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="selector">
        /// A lambda expression to select a string property of <typeparamref name="T" />
        /// </param>
        /// <param name="columnWidths"></param>
        /// <param name="truncateLengths"></param>
        public static string ToFixedWidthString<T>(this IEnumerable<T> source, Func<T, string> selector, IEnumerable<Int32> columnWidths, Boolean truncateLengths)
        {
            var rv = new List<String>();
            var columnWidthsList = columnWidths.ToArray();
            var clist = source.Select(selector).ToArray();
            for (var i = 0; i < clist.Length; i++)
            {
                if (truncateLengths) rv.Add(clist[i].Truncate(columnWidthsList[i]).PadRight(columnWidthsList[i]));
                else
                {
                    if (clist[i].Length > columnWidthsList[i]) throw new ArgumentOutOfRangeException();
                    rv.Add(clist[i].PadRight(columnWidthsList[i]));
                }
            }
            return String.Join(String.Empty, rv.ToArray());
        }

        public static List<String> ParseFixedWidthLine(this String row, IEnumerable<Int32> columnWidths)
        {
            var newlist = new List<String>();
            var curpos = 0;
            foreach (var w in columnWidths)
            {
                newlist.Add((curpos + w) <= row.Length ? row.Substring(curpos, w).Trim() : row.Substring(curpos).Trim());
                curpos += w;
            }
            return newlist;
        }

        #region ToDelimitedString() Examples

        // **************************** Array example****************************
        /*
        int[] ints = { 1, 4, 5, 7 };

        // Returns "1, 4, 5, 7"
        string result = ints.ToDelimitedString();
        */

        // **************************** List example****************************
        /*
        var products = new List<Product>
                       {
                           new Product { ProductName = "Chai" },
                           new Product { ProductName = "Chang" },
                           new Product { ProductName = "Tofu" },
                       };

        // Returns "Chai, Chang, Tofu"
        string result1 = products.ToDelimitedString(p => p.ProductName);

        // Returns "Chai;Chang;Tofu"
        string result2 = products.ToDelimitedString(p => p.ProductName, ";");
        */

        // **************************** Datatable example****************************
        /*

        // Returns "Chai, Chang, Tofu"
        string result = table.AsEnumerable().ToDelimitedString(row => (string)row["ProductName"]);
        */

        #endregion ToDelimitedString() Examples

        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static bool IsNotNullOrEmpty(this string s)
        {
            return !String.IsNullOrEmpty(s);
        }

        public static Boolean IsNullOrWhiteSpace(this String obj)
        {
            return obj == null || obj.Trim() == String.Empty;
        }

        public static string TrimOrEmpty(this String val)
        {
            return string.IsNullOrWhiteSpace(val) ? string.Empty : val.Trim();
        }

        /// <summary>
        /// Returns the last few characters of the string with a length specified by the given
        /// parameter. If the string's length is less than the given length the complete string is
        /// returned. If length is zero or less an empty string is returned
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="len">Number of characters to return</param>
        /// <returns></returns>
        public static string Right(this string s, int len)
        {
            len = Math.Max(len, 0);
            return s.Length > len ? s.Substring(s.Length - len, len) : s;
        }

        /// <summary>
        /// Returns the last few characters of the string with a length specified by the given
        /// parameter. If the string's length is less than the given length the complete string is
        /// returned. If length is zero or less an empty string is returned
        /// </summary>
        /// <param name="s">The string to process</param>
        /// <param name="len">Number of characters to return</param>
        /// <param name="trim">
        /// True will Trim() the string of leading and trailing white space before parsing. The
        /// default value is false.
        /// </param>
        /// <returns></returns>
        public static string Right(this string s, int len, Boolean trim)
        {
            if (trim)
            {
                s = s.Trim();
            }
            len = Math.Max(len, 0);
            return s.Length > len ? s.Substring(s.Length - len, len) : s;
        }

        public static string RightOfIndexOf(this string s, char character)
        {
            var pos = s.IndexOf(character);
            return pos == -1 ? s : s.Substring(pos + 1);
        }

        public static string RightOfIndexOf(this string s, String characters)
        {
            var pos = s.IndexOf(characters, StringComparison.Ordinal);
            return pos == -1 ? s : s.Substring(pos + (characters.Length + 1));
        }

        public static string RightOfLastIndexOf(this string s, char character)
        {
            var pos = s.LastIndexOf(character);
            if (pos == -1) return s;
            return s.Substring(pos + 1);
        }

        public static string RightOfLastIndexOf(this string s, String characters)
        {
            var pos = s.LastIndexOf(characters, StringComparison.Ordinal);
            return pos == -1 ? s : s.Substring(pos + (characters.Length + 1));
        }

        /// <summary>
        /// Returns the first few characters of the string with a length specified by the given
        /// parameter. If the string's length is less than the given length the complete string is
        /// returned. If length is zero or less an empty string is returned
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="len">Number of characters to return</param>
        /// <returns></returns>
        public static string Left(this string s, int len)
        {
            len = Math.Max(len, 0);
            return s.Length > len ? s.Substring(0, len) : s;
        }

        /// <summary>
        /// Returns the first few characters of the string with a length specified by the given
        /// parameter. If the string's length is less than the given length the complete string is
        /// returned. If length is zero or less an empty string is returned
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="len">Number of characters to return</param>
        /// <param name="trim">
        /// True will Trim() the string of leading and trailing white space before parsing. The
        /// default value is false.
        /// </param>
        /// <returns></returns>
        public static string Left(this string s, int len, Boolean trim)
        {
            if (trim)
            {
                s = s.Trim();
            }
            len = Math.Max(len, 0);
            return s.Length > len ? s.Substring(0, len) : s;
        }

        public static string LeftOfIndexOf(this string s, char character)
        {
            var pos = s.IndexOf(character);
            return pos >= 0 ? s.Substring(0, pos) : s;
        }

        public static string LeftOfIndexOf(this string s, String characters)
        {
            var pos = s.IndexOf(characters, StringComparison.Ordinal);
            return pos >= 0 ? s.Substring(0, pos) : s;
        }

        public static string LeftOfLastIndexOf(this string s, char character)
        {
            var pos = s.LastIndexOf(character);
            return pos >= 0 ? s.Substring(0, pos) : s;
        }

        public static string LeftOfLastIndexOf(this string s, String characters)
        {
            var pos = s.LastIndexOf(characters, StringComparison.Ordinal);
            return pos >= 0 ? s.Substring(0, pos) : s;
        }

        /// <summary>
        /// Strip a string of the specified character.
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="character"></param>
        /// <example>
        /// string s = "abcde";
        ///
        /// s = s.Strip('b'); //s becomes 'acde;
        /// </example>
        /// <returns></returns>
        public static string Strip(this string s, char character)
        {
            s = s.Replace(character.ToString(CultureInfo.InvariantCulture), "");
            return s;
        }

        /// <summary>
        /// Strip a string of the specified characters.
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="chars">list of characters to remove from the string</param>
        /// <example>
        /// string s = "abcde";
        ///
        /// s = s.Strip('a', 'd'); //s becomes 'bce;
        /// </example>
        /// <returns></returns>
        public static string Strip(this string s, params char[] chars)
        {
            return chars.Aggregate(s, (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), ""));
        }

        /// <summary>
        /// Strip a string of the specified substring.
        /// </summary>
        /// <param name="s">the string to process</param>
        /// <param name="subString">substring to remove</param>
        /// <example>
        /// string s = "abcde";
        ///
        /// s = s.Strip("bcd"); //s becomes 'ae;
        /// </example>
        /// <returns></returns>
        public static string Strip(this string s, string subString)
        {
            s = s.Replace(subString, "");
            return s;
        }

        /// <summary>
        /// Truncates the string to a specified length.
        /// </summary>
        /// <param name="s">The String parameter to be truncated.</param>
        /// <param name="len">The maximum length of return value.</param>
        /// <returns>The truncated string result.</returns>
        public static string Truncate(this string s, int len)
        {
            if (s.IsNullOrWhiteSpace() || s.Length <= len) return s;
            return s.Substring(0, len);
        }

        /// <summary>
        /// Truncates the string to a specified length and replace the last several characters with
        /// supplied string.
        /// </summary>
        /// <param name="s">The String parameter to be truncated.</param>
        /// <param name="len">The maximum length of return value.</param>
        /// <param name="end">
        /// The end of string suffix to replace the last characters in the string with.
        /// </param>
        /// <returns>The truncated string result.</returns>
        public static string Truncate(this string s, int len, String end)
        {
            if (s.IsNullOrWhiteSpace() || s.Length <= len) return s;
            if (len - end.Length > 0) return s.Substring(0, len - end.Length) + end;
            return s.Substring(0, Math.Max(len, 0));
        }

        /// <summary>
        /// Changes the string to title case.
        /// </summary>
        /// <param name="s">The String to convert to title case.</param>
        /// <returns>String</returns>
        public static string ToTitleCase(this string s)
        {
            return new CultureInfo("en-US").TextInfo.ToTitleCase(s);
        }

        public static String PrefixZeros(this String value, Int32 totalWidth)
        {
            return value.PadLeft(totalWidth, '0');
        }

        public static String PrefixZeros(this Int16 value, Int32 totalWidth)
        {
            return value.ToString(CultureInfo.InvariantCulture).PrefixZeros(totalWidth);
        }

        public static String PrefixZeros(this Int32 value, Int32 totalWidth)
        {
            return value.ToString(CultureInfo.InvariantCulture).PrefixZeros(totalWidth);
        }

        public static String PrefixZeros(this Int64 value, Int32 totalWidth)
        {
            return value.ToString(CultureInfo.InvariantCulture).PrefixZeros(totalWidth);
        }

        public static String Wrap(this String value, String prefix, String suffix)
        {
            return prefix + value + suffix;
        }

        public static String WrapInTag(this String value, String tag)
        {
            return "<" + tag + ">" + value + "</" + tag + ">";
        }

        public static String Reverse(this String value)
        {
            var charArray = value.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        public static int CountWords(this String value)
        {
            return Regex.Matches(value, @"[A-Za-z0-9\-]+").Count;
        }

        public static int CountLines(this String value)
        {
            var count = 1;
            var start = 0;
            while ((start = value.IndexOf('\n', start)) != -1)
            {
                count++;
                start++;
            }
            return count;
        }

        public static Boolean ContainsOneOfTheseArgs(this String value, IEnumerable<String> arglist)
        {
            return arglist.Any(value.Contains);
        }

        public static Boolean ContainsAllOfTheseArgs(this String value, IEnumerable<String> arglist)
        {
            var missing = false;
            var missinglist = new List<String>();
            var enumerable = arglist as IList<string> ?? arglist.ToList();
            foreach (var a in enumerable.Where(a => !value.Contains(a)))
            {
                missing = true;
                missinglist.Add(a);
            }
            return !missing && enumerable.Any();
        }

        #endregion Basic Methods

        #region Regex Formatting

        public static bool IsUrl(this string s)
        {
            return s.IsUrl(false);
        }

        public static bool IsUrl(this string s, Boolean ignoreSurroundingWhitespace)
        {
            if (s.IsNullOrWhiteSpace()) return false;
            var rx = new Regex(@"\A(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*\Z", RegexOptions.IgnoreCase);
            return rx.IsMatch(ignoreSurroundingWhitespace ? s.Trim() : s);
        }

        public static bool ContainsUrl(this string s)
        {
            if (s.IsNullOrWhiteSpace()) return false;
            var rx = new Regex(@"(?<Protocol>\w+):\/\/(?<Domain>[\w@][\w.:@]+)\/?[\w\.?=%&=\-@/$,]*", RegexOptions.IgnoreCase);
            return rx.IsMatch(s);
        }

        public static bool IsEmail(this string s)
        {
            return s.IsEmail(false);
        }

        public static bool IsEmail(this string s, Boolean ignoreSurroundingWhitespace)
        {
            if (s.IsNullOrWhiteSpace()) return false;
            var rx = new Regex(@"\A([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})\Z", RegexOptions.IgnoreCase);
            return rx.IsMatch(ignoreSurroundingWhitespace ? s.Trim() : s);
        }

        public static bool ContainsEmail(this string s)
        {
            if (s.IsNullOrWhiteSpace()) return false;
            var rx = new Regex(@"([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})", RegexOptions.IgnoreCase);
            return rx.IsMatch(s);
        }

        public static bool IsStrongPassword(this string s)
        {
            return s.IsStrongPassword(6);
        }

        public static bool IsStrongPassword(this string s, Int32 minLength)
        {
            return Regex.IsMatch(s, @"[\d]")
                && Regex.IsMatch(s, @"[a-z]")
                && Regex.IsMatch(s, @"[A-Z]")
                && Regex.IsMatch(s, @"[\s~!@#\$%\^&\*\(\)\{\}\|\[\]\\:;'?,.`+=<>\/]")
                && s.Length >= minLength;
        }

        public static bool IsStrongPassword(this string s, Int32 minLength, Int32 maxLength)
        {
            return Regex.IsMatch(s, @"[\d]")
                && Regex.IsMatch(s, @"[a-z]")
                && Regex.IsMatch(s, @"[A-Z]")
                && Regex.IsMatch(s, @"[\s~!@#\$%\^&\*\(\)\{\}\|\[\]\\:;'?,.`+=<>\/]")
                && (s.Length >= minLength && s.Length <= maxLength);
        }

        public static bool IsMarkup(this string s)
        {
            return s.IsMarkup(false);
        }

        public static bool IsMarkup(this string s, Boolean ignoreSurroundingWhitespace)
        {
            if (s.IsNullOrWhiteSpace()) return false;
            var rx = new Regex(@"<[a-z0-9]*>.*?</[a-z0-9]*>", RegexOptions.IgnoreCase);
            return rx.IsMatch(ignoreSurroundingWhitespace ? s.Trim() : s);
        }

        public static bool ContainsMarkup(this string s)
        {
            if (s.IsNullOrWhiteSpace()) return false;
            var rx = new Regex(@"<[a-z0-9]*>.*?</[a-z0-9]*>", RegexOptions.IgnoreCase);
            return rx.IsMatch(s);
        }

        public static bool IsRegexMatch(this string s, String pattern)
        {
            return s.IsRegexMatch(pattern, false);
        }

        public static bool IsRegexMatch(this string s, String pattern, Boolean ignoreSurroundingWhitespace)
        {
            if (s.IsNullOrWhiteSpace() || pattern.IsNullOrWhiteSpace()) return false;
            var rx = new Regex(pattern);
            return rx.IsMatch(ignoreSurroundingWhitespace ? s.Trim() : s);
        }

        public static bool IsRegexMatch(this string s, String pattern, RegexOptions regexOptions, Boolean ignoreSurroundingWhitespace)
        {
            if (s.IsNullOrWhiteSpace() || pattern.IsNullOrWhiteSpace()) return false;
            var rx = new Regex(pattern, regexOptions);
            return rx.IsMatch(ignoreSurroundingWhitespace ? s.Trim() : s);
        }

        public static bool IsRegexMatchAny(this string s, params String[] patterns)
        {
            return s.IsRegexMatchAny(false, patterns);
        }

        public static bool IsRegexMatchAny(this string s, Boolean ignoreSurroundingWhitespace, params String[] patterns)
        {
            return patterns.Any(p => s.IsRegexMatch(p, ignoreSurroundingWhitespace));
        }

        public static bool IsRegexMatchAny(this string s, RegexOptions regexOptions, Boolean ignoreSurroundingWhitespace, params String[] patterns)
        {
            return patterns.Any(p => s.IsRegexMatch(p, regexOptions, ignoreSurroundingWhitespace));
        }

        public static bool IsRegexMatchAll(this string s, params String[] patterns)
        {
            return s.IsRegexMatchAll(false, patterns);
        }

        public static bool IsRegexMatchAll(this string s, Boolean ignoreSurroundingWhitespace, params String[] patterns)
        {
            var rv = true;
            foreach (var p in patterns)
            {
                rv = (s.IsRegexMatch(p, ignoreSurroundingWhitespace));
                if (!rv) break;
            }
            return rv;
        }

        public static bool IsRegexMatchAll(this string s, RegexOptions regexOptions, Boolean ignoreSurroundingWhitespace, params String[] patterns)
        {
            var rv = true;
            foreach (var p in patterns)
            {
                rv = (s.IsRegexMatch(p, regexOptions, ignoreSurroundingWhitespace));
                if (!rv) break;
            }
            return rv;
        }

        public static bool IsRegexWildcardMatch(this string s, string pattern, string wildcardCharacters = "*", RegexOptions regexOptions = RegexOptions.None, Boolean ignoreSurroundingWhitespace = true)
        {
            var wildcards = wildcardCharacters.ToCharArray();
            pattern = wildcards.Aggregate(pattern, (current, c) => current.Replace(c.ToString(CultureInfo.InvariantCulture), @"[\s\S]*"));
            if (ignoreSurroundingWhitespace) pattern = pattern.Trim();
            return s.IsRegexMatch(pattern, regexOptions, ignoreSurroundingWhitespace);
        }

        public static String RegexMatch(this string s, String pattern, RegexOptions regexOptions = RegexOptions.None, Boolean ignoreSurroundingWhitespace = false)
        {
            var rx = new Regex(pattern, regexOptions);
            return ignoreSurroundingWhitespace ? rx.Match(s.Trim()).ToString() : rx.Match(s).ToString();
        }

        public static String RegexMatchThenReplaceInOther(this String s, String sourcePattern, String destination, String destinationPattern)
        {
            var rx = new Regex(destinationPattern, RegexOptions.None);
            var sourceMatch = s.RegexMatch(sourcePattern);
            return rx.Replace(destination, sourceMatch, 1);
        }

        #endregion Regex Formatting

        #region Advanced Parsing

        public static Boolean TryParseAsEnum<T>(this string value, out T result) where T : struct
        {
            result = default(T);
            try
            {
                if (!value.IsNullOrWhiteSpace())
                {
                    result = (T)Enum.Parse(typeof(T), value, true);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static T ParseAsEnum<T>(this string value) where T : struct
        {
            if (!value.IsNullOrWhiteSpace()) return (T)Enum.Parse(typeof(T), value, true);
            return default(T);
        }

        public static Boolean TryParseAs<T>(this string value, out T result)
        {
            // Get default value for type so if string is empty then we can return default value.
            result = default(T);
            try
            {
                if (!value.IsNullOrWhiteSpace())
                {
                    var tc = TypeDescriptor.GetConverter(typeof(T));
                    result = (T)tc.ConvertFrom(value);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public static T ParseAs<T>(this string value)
        {
            // Get default value for type so if string is empty then we can return default value.
            var result = default(T);
            if (value.IsNullOrWhiteSpace()) return result;
            var tc = TypeDescriptor.GetConverter(typeof(T));
            result = (T)tc.ConvertFrom(value);
            return result;
        }

        /*

            // regular parsing
            int i = "123".Parse<int>();
            int? inull = "123".Parse<int?>();
            DateTime d = "01/12/2008".Parse<DateTime>();
            DateTime? dn = "01/12/2008".Parse<DateTime?>();

            // null values
            string sample = null;
            int? k = sample.Parse<int?>(); // returns null
            int l = sample.Parse<int>();   // returns 0
            DateTime dd = sample.Parse<DateTime>(); // returns 01/01/0001
            DateTime? ddn = sample.Parse<DateTime?>(); // returns null
         */

        /// <summary>
        /// Returns an enumerable collection of the specified type containing the substrings in this
        /// instance that are delimited by elements of a specified Char array
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator">
        /// An array of Unicode characters that delimit the substrings in this instance, an empty
        /// array containing no delimiters, or null.
        /// </param>
        /// <typeparam name="T">
        /// The type of the elemnt to return in the collection, this type must implement IConvertible.
        /// </typeparam>
        /// <returns>
        /// An enumerable collection whose elements contain the substrings in this instance that are
        /// delimited by one or more characters in separator.
        /// </returns>
        public static IEnumerable<T> ParseAsIEnumerable<T>(this string value, params char[] separator) where T : IConvertible
        {
            return value.Split(separator, StringSplitOptions.None).Select(s => (T)Convert.ChangeType(s, typeof(T)));
        }

        /// <summary>
        /// Splits a string into a NameValueCollection, where each "namevalue" is separated by the
        /// "OuterSeparator". The parameter "NameValueSeparator" sets the split between Name and Value.
        /// Example: String str = "param1=value1;param2=value2"; NameValueCollection nvOut =
        ///          str.ToNameValueCollection(';', '=');
        ///
        /// The result is a NameValueCollection where: key[0] is "param1" and value[0] is "value1"
        /// key[1] is "param2" and value[1] is "value2"
        /// </summary>
        /// <param name="s"></param>
        /// <param name="parameterSeparator"></param>
        /// <param name="valueSeparator"></param>
        /// <returns></returns>
        public static NameValueCollection ParseAsNameValueCollection(this String s, Char parameterSeparator, Char valueSeparator)
        {
            NameValueCollection nvText = null;
            s = s.TrimEnd(parameterSeparator);
            if (s.IsNullOrWhiteSpace()) return null;
            var arrStrings = s.TrimEnd(parameterSeparator).Split(parameterSeparator);
            foreach (var a in arrStrings)
            {
                var posSep = a.IndexOf(valueSeparator);
                var name = a.Substring(0, posSep);
                var value = a.Substring(posSep + 1);
                if (nvText == null) nvText = new NameValueCollection();
                nvText.Add(name, value);
            }
            return nvText;
        }

        /// <summary>
        /// Splits a string into a NameValueCollection, where each "namevalue" is separated by the
        /// "OuterSeparator". The parameter "NameValueSeparator" sets the split between Name and Value.
        /// Example: String str = "param1=value1;param2=value2"; NameValueCollection nvOut =
        ///          str.ToNameValueCollection(';', '=');
        ///
        /// The result is a NameValueCollection where: key[0] is "param1" and value[0] is "value1"
        /// key[1] is "param2" and value[1] is "value2"
        /// </summary>
        /// <param name="s"></param>
        /// <param name="parameterSeparator"></param>
        /// <param name="valueSeparator"></param>
        /// <returns></returns>
        public static NameValueCollection ParseAsNameValueCollection(this String s, String parameterSeparator, String valueSeparator)
        {
            NameValueCollection nvText = null;
            if (parameterSeparator.IsNullOrWhiteSpace() || valueSeparator.IsNullOrWhiteSpace()) return null;
            s = s.TrimEnd(parameterSeparator.ToCharArray());
            var arrStrings = s.TrimEnd(parameterSeparator.ToCharArray()).Split(parameterSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (var a in arrStrings)
            {
                var posSep = a.IndexOf(valueSeparator, StringComparison.Ordinal);
                if (posSep < 0) break;
                var name = a.Substring(0, posSep);
                var value = a.Substring(posSep + valueSeparator.Length);
                if (nvText == null) nvText = new NameValueCollection();
                nvText.Add(name, value);
            }
            return nvText;
        }

        /// <summary>
        /// Splits a string into a NameValueCollection, where each "namevalue" is separated by the
        /// "OuterSeparator". The parameter "NameValueSeparator" sets the split between Name and Value.
        /// Example: String str = "param1=value1;param2=value2"; NameValueCollection nvOut =
        ///          str.ToNameValueCollection(';', '=');
        ///
        /// The result is a NameValueCollection where: key[0] is "param1" and value[0] is "value1"
        /// key[1] is "param2" and value[1] is "value2"
        /// </summary>
        /// <param name="s"></param>
        /// <param name="parameterSeparator"></param>
        /// <param name="valueSeparator"></param>
        /// <returns></returns>
        public static IEnumerable<Tuple<String, String>> ParseAsStringTuples(this String s, Char parameterSeparator, Char valueSeparator)
        {
            s = s.TrimEnd(parameterSeparator);
            if (s.IsNullOrWhiteSpace()) yield break;
            var arrStrings = s.TrimEnd(parameterSeparator).Split(parameterSeparator);
            foreach (var a in arrStrings)
            {
                var posSep = a.IndexOf(valueSeparator);
                var name = a.Substring(0, posSep);
                var value = a.Substring(posSep + 1);
                yield return new Tuple<String, String>(name, value);
            }
        }

        ///// <summary>
        ///// Splits a string into a NameValueCollection, where each "namevalue" is separated by
        ///// the "OuterSeparator". The parameter "NameValueSeparator" sets the split between Name and Value.
        ///// Example:
        /////             String str = "param1=value1;param2=value2";
        /////             NameValueCollection nvOut = str.ToNameValueCollection(';', '=');
        /////
        ///// The result is a NameValueCollection where:
        /////             key[0] is "param1" and value[0] is "value1"
        /////             key[1] is "param2" and value[1] is "value2"
        ///// </summary>
        ///// <param name="str">String to process</param>
        ///// <param name="OuterSeparator">Separator for each "NameValue"</param>
        ///// <param name="NameValueSeparator">Separator for Name/Value splitting</param>
        ///// <returns></returns>
        //public static NameValueCollection ParseAsNameValueCollection(this String s, String ParameterSeparator, String ValueSeparator)
        //{
        //    NameValueCollection nvText = null;
        //    if (!ParameterSeparator.IsNullOrWhiteSpace() && !ValueSeparator.IsNullOrWhiteSpace())
        //    {
        //        s = s.TrimEnd(ParameterSeparator.ToCharArray());
        //        String[] arrStrings = s.TrimEnd(ParameterSeparator.ToCharArray()).Split(ParameterSeparator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //        foreach (String a in arrStrings)
        //        {
        //            Int32 posSep = a.IndexOf(ValueSeparator);
        //            if (posSep < 0) break;
        //            String name = a.Substring(0, posSep);
        //            String value = a.Substring(posSep + ValueSeparator.Length);
        //            if (nvText == null) nvText = new NameValueCollection();
        //            nvText.Add(name, value);
        //        }
        //    }
        //    return nvText;
        //}

        #endregion Advanced Parsing

        #region StringReader / StreamReader Methods

        /* TODO - Add last row paramater logic */

        public static IEnumerable<IEnumerable<String>> Lines(this string stringdata, string delimiter = "\t", Int32 firstrow = 1, String quotedNewLineReplacement = null, String quotedDelimiterReplacement = null, String trimChars = "\"' ")
        {
            using (var sr = new StringReader(stringdata))
            {
                String line;
                while (firstrow > 1 && sr.ReadLine() != null)
                {
                    firstrow--;
                }
                if (quotedNewLineReplacement == null) // only check QuotedNewLineReplacement for null (not empty string or whitespace)
                {
                    if (quotedDelimiterReplacement == null) // only check QuotedDelimiterReplacement for null (not empty string or whitespace)
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            /* return the results here */
                            yield return line.Replace("\0", "").Split(delimiter.ToCharArray()).Select(x => x.Trim(trimChars.ToCharArray()));
                        }
                    }
                    else // any other QuotedDelimiterReplacement value besides null
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            /* Do QuotedDelimiterReplacement here  */
                            var qm = Regex.Matches(line, "(\"+[^\"]*\"+?)");
                            line = (from Match m in qm select m.Value).Aggregate(line, (current, ms) => current.Replace(ms, ms.Replace(delimiter, quotedDelimiterReplacement)));

                            /* return the results here */
                            yield return line.Replace("\0", "").Split(delimiter.ToCharArray()).Select(x => x.Trim(trimChars.ToCharArray()));
                        }
                    }
                }
                else // any other QuotedNewLineReplacement value besides null
                {
                    if (quotedDelimiterReplacement == null) // only check QuotedDelimiterReplacement for null (not empty string or whitespace)
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            /* fix broken rows here */
                            while ((Regex.Matches(line, "\"").Count) % 2 != 0)
                            {
                                String nextline;
                                if ((nextline = sr.ReadLine()) != null)
                                {
                                    line += quotedNewLineReplacement + nextline;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            /* return the results here */
                            yield return line.Replace("\0", "").Split(delimiter.ToCharArray()).Select(x => x.Trim(trimChars.ToCharArray()));
                        }
                    }
                    else // any other QuotedDelimiterReplacement value besides null
                    {
                        while ((line = sr.ReadLine()) != null)
                        {
                            /* fix broken rows here */
                            while ((Regex.Matches(line, "\"").Count) % 2 != 0)
                            {
                                String nextline;
                                if ((nextline = sr.ReadLine()) != null)
                                {
                                    line += quotedNewLineReplacement + nextline;
                                }
                                else
                                {
                                    break;
                                }
                            }

                            /* Do QuotedDelimiterReplacement here  */
                            var qm = Regex.Matches(line, "(\"+[^\"]*\"+?)");
                            line = (from Match m in qm select m.Value).Aggregate(line, (current, ms) => current.Replace(ms, ms.Replace(delimiter, quotedDelimiterReplacement)));

                            /* return the results here */
                            yield return line.Replace("\0", "").Split(delimiter.ToCharArray()).Select(x => x.Trim(trimChars.ToCharArray()));
                        }
                    }
                }
            }
        }

        public static IEnumerable<string[]> Lines(this StringReader sr, string delimiter = "\t", Int32 firstrow = 1)
        {
            String line;
            while (firstrow > 1 && sr.ReadLine() != null)
            {
                firstrow--;
            }
            while ((line = sr.ReadLine()) != null)
            {
                yield return line.Replace("\0", "").Trim().Split(delimiter.ToCharArray());
            }
        }

        public static IEnumerable<string[]> Lines(this StreamReader sr, string delimiter = "\t", Int32 firstrow = 1)
        {
            String line;
            while (firstrow > 1 && sr.ReadLine() != null)
            {
                firstrow--;
            }
            while ((line = sr.ReadLine()) != null)
            {
                yield return line.Replace("\0", "").Trim().Split(delimiter.ToCharArray());
            }
        }

        public static IEnumerable<string[]> Lines(this string stringdata, IEnumerable<Int32> columnWidths, Int32 firstrow = 1)
        {
            var columnWidthArray = columnWidths.ToArray();
            using (var sr = new StringReader(stringdata))
            {
                String line;
                while (firstrow > 1 && sr.ReadLine() != null)
                {
                    firstrow--;
                }
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line.ParseFixedWidthLine(columnWidthArray).ToArray();
                }
            }
        }

        public static IEnumerable<string[]> Lines(this StringReader sr, IEnumerable<Int32> columnWidths, Int32 firstrow = 1)
        {
            String line;
            var columnWidthArray = columnWidths.ToArray();
            while (firstrow > 1 && sr.ReadLine() != null)
            {
                firstrow--;
            }
            while ((line = sr.ReadLine()) != null)
            {
                yield return line.ParseFixedWidthLine(columnWidthArray).ToArray();
            }
        }

        public static IEnumerable<string[]> Lines(this StreamReader sr, IEnumerable<Int32> columnWidths, Int32 firstrow = 1)
        {
            String line;
            var columnWidthArray = columnWidths.ToArray();
            while (firstrow > 1 && sr.ReadLine() != null)
            {
                firstrow--;
            }
            while ((line = sr.ReadLine()) != null)
            {
                yield return line.ParseFixedWidthLine(columnWidthArray).ToArray();
            }
        }

        public static IEnumerable<string> FullLines(this string stringdata)
        {
            using (var sr = new StringReader(stringdata))
            {
                String line;
                while ((line = sr.ReadLine()) != null)
                {
                    yield return line.Replace("\0", "").Trim();
                }
            }
        }

        #endregion StringReader / StreamReader Methods

        #region Seperated String Data methods

        public static DataTable LinesToDataTable(this IEnumerable<IEnumerable<String>> lineData, DataTable template = null, Boolean useFirstRowAsColumnNames = false, IEnumerable<String> columnNames = null, IEnumerable<Type> columnDataTypes = null)
        {
            var rv = template ?? new DataTable();
            var columnsExist = template != null;
            var isFirstRow = true;
            foreach (var l in lineData)
            {
                if (isFirstRow)
                {
                    var firstrow = new List<String>(l);
                    isFirstRow = false;
                    if (!columnsExist)
                    {
                        /* Since there is now columns we need to create them here. */

                        /* create an array to hold the column names */
                        var colnamearr = new String[firstrow.Count];

                        /* Use the first row values if UseFirstRowAsColumnNames == true */
                        if (useFirstRowAsColumnNames)
                        {
                            for (var i = 0; i < colnamearr.Length; i++)
                            {
                                colnamearr[i] = i < firstrow.Count ? firstrow[i] : String.Format("Column{0}", i);
                            }
                        }
                        else
                        {
                            for (var i = 0; i < colnamearr.Length; i++)
                            {
                                colnamearr[i] = String.Format("Column{0}", i);
                            }
                        }

                        /* Use the collection of user specified ColumnNames (overwriting what is there already) */
                        if (columnNames != null)
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            var columnNamesInputArray = columnNames.ToArray();
                            for (var i = 0; i < colnamearr.Length; i++)
                            {
                                if (i < columnNamesInputArray.Length)
                                {
                                    colnamearr[i] = columnNamesInputArray[i];
                                }
                                else if (colnamearr[i] == null)
                                {
                                    colnamearr[i] = String.Format("Column{0}", i.ToString(CultureInfo.InvariantCulture));
                                }
                            }
                        }

                        /* Next create an array to hold the column data types */

                        if (columnDataTypes == null)
                        {
                            foreach (var n in colnamearr)
                            {
                                rv.Columns.Add(n);
                            }
                        }
                        else
                        {
                            // ReSharper disable once PossibleMultipleEnumeration
                            var columnDataTypesInputArray = columnDataTypes.ToArray();
                            for (var i = 0; i < colnamearr.Length; i++)
                            {
                                if (i < columnDataTypesInputArray.Length)
                                {
                                    rv.Columns.Add(colnamearr[i], columnDataTypesInputArray[i]);
                                }
                                else if (colnamearr[i] == null)
                                {
                                    rv.Columns.Add(colnamearr[i]);
                                }
                            }
                        }
                        columnsExist = true;
                    }

                    /* if we did not use the first row for names insert it now */
                    if (useFirstRowAsColumnNames) continue;
                    var fr = rv.NewRow();
                    // ReSharper disable once CoVariantArrayConversion
                    fr.ItemArray = firstrow.ToArray();
                    rv.Rows.Add(fr);
                }
                else
                {
                    /* insert the rest of the rows here.  */
                    var dr = rv.NewRow();
                    // ReSharper disable once CoVariantArrayConversion
                    dr.ItemArray = l.ToArray();
                    rv.Rows.Add(dr);
                }
            }
            return rv;
        }

        public static void DataTableAppendSeperatedTxtToStreamWriter(ref StreamWriter streamWriter,
            DataTable data,
            Boolean colHeaders = false,
            String fieldSeperator = "\t",
            String textQualifier = null,
            String newLineChar = "\r\n",
            String nullValue = "")
        {
            if (newLineChar.Contains(@"\"))
            {
                newLineChar = newLineChar.Replace(@"\r", "\r").Replace(@"\n", "\n");
            }
            if (fieldSeperator.Contains(@"\"))
            {
                fieldSeperator = fieldSeperator.Replace(@"\t", "\t");
            }

            textQualifier = textQualifier ?? String.Empty; // just in case this is null
            nullValue = nullValue ?? String.Empty; // just in case this is null

            streamWriter.NewLine = newLineChar;

            using (var dr = data.CreateDataReader())
            {
                var tempobj = new Object[dr.FieldCount];
                if (!dr.HasRows) return;
                if (colHeaders)
                {
                    streamWriter.WriteLine(String.Join(fieldSeperator, (from DataColumn c in data.Columns select c.ColumnName).ToArray()));
                }
                while (dr.Read())
                {
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                    dr.GetValues(tempobj);
                    var rowdata = new List<string>();
                    foreach (var o in tempobj)
                    {
                        if (o is DateTime)
                        {
                            rowdata.Add(((DateTime)o).ToString("yyyy-MM-dd HH:mm:ss"));
                        }
                        else if (o is string && !String.IsNullOrWhiteSpace(textQualifier))
                        {
                            rowdata.Add(String.Format("\"{0}\"", o == DBNull.Value ? nullValue : o));
                        }
                        else
                        {
                            rowdata.Add((o == null || o == DBNull.Value ? nullValue : o.ToString()));
                        }
                    }
                    streamWriter.WriteLine(String.Join(fieldSeperator, rowdata.ToArray()));
                }
            }
        }

        public static void CreateSeperatedTxtFileFromDataTable(FileInfo file,
            DataTable data,
            Boolean colHeaders = false,
            String fieldSeperator = "\t",
            String textQualifier = null,
            String newLineChar = "\r\n",
            String nullValue = "")
        {
            if (file.Exists) file.Delete();
            var sw = new StreamWriter(file.FullName);
            DataTableAppendSeperatedTxtToStreamWriter(ref sw, data, colHeaders, fieldSeperator, textQualifier, newLineChar, nullValue);
            sw.Flush();
            sw.Close();
        }

        #endregion Seperated String Data methods

        //, Func<T, string> selector, string delimiter
        //, IEnumerable<Int32> ColumnWidths
    }
}