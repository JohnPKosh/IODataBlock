using System;
using System.Collections.Generic;

namespace Business.Utilities.Extensions
{
    public static class StringFilterExtensions
    {
        public static IEnumerable<object> Filter(this IEnumerable<object> values, string member,
            StringFilterOption filterOption, params object[] paramList)
        {
            if (filterOption == StringFilterOption.None) return values;
            var rv = new List<object>();
            foreach (var value in values)
            {
                var m = value as IDictionary<String, Object>;
                if (m == null || !m.ContainsKey(member)) continue;
                switch (filterOption)
                {
                    case StringFilterOption.Equals:
                        break;

                    case StringFilterOption.StartsWith:
                        break;

                    case StringFilterOption.Contains:
                        break;

                    case StringFilterOption.EndsWith:
                        break;

                    case StringFilterOption.NotEquals:
                        break;

                    case StringFilterOption.GreaterThan:
                        if (_isGreaterThan(m[member], paramList[0])) rv.Add(value);
                        break;

                    case StringFilterOption.LessThan:
                        break;

                    case StringFilterOption.GreaterThanOrEqualTo:
                        break;

                    case StringFilterOption.LessThanOrEqualTo:
                        break;

                    case StringFilterOption.Between:
                        if (_isBetween(m[member], paramList[0], paramList[1])) rv.Add(value);
                        break;

                    case StringFilterOption.Null:
                        break;

                    case StringFilterOption.NotNull:
                        break;

                    case StringFilterOption.NullOrWhiteSpace:
                        break;

                    case StringFilterOption.NotNullOrWhiteSpace:
                        break;

                    case StringFilterOption.None:
                        break;

                    default:
                        throw new ArgumentOutOfRangeException("filterOption");
                }
            }
            return rv;
        }

        public static Func<object, object> GetMemberStringFilterFunc(
            string member,
            StringFilterOption filterOption,
            params object[] paramList)
        {
            return value =>
            {
                if (value == null) return null;
                var members = value as IDictionary<String, Object>;
                if (members == null)
                {
                    throw new ArgumentNullException(member, @"GetMemberStringFilterFunc requires a non-null parameter! No object members exist!");
                }
                if (!members.ContainsKey(member))
                {
                    throw new ArgumentNullException(member, @"GetMemberStringFilterFunc requires a non-null parameter! No {0} parameter exists!");
                }
                if (members[member].GetType() != typeof(string))
                {
                    throw new ArgumentException(String.Format(@"GetMemberStringFilterFunc requires a string type member! Target member type is {0}", members[member].GetType().Name), member);
                }
                return value;
            };
        }

        public static bool IsStringFilterMatch(this string value, StringFilterOption filterOption, string[] paramList, StringComparison comparison = StringComparison.Ordinal)
        {
            switch (filterOption)
            {
                case StringFilterOption.Equals:
                    return value.Equals(paramList[0], comparison);

                case StringFilterOption.StartsWith:
                    return value.StartsWith(paramList[0], comparison);

                case StringFilterOption.Contains:
                    switch (comparison)
                    {
                        case StringComparison.OrdinalIgnoreCase:
                            return value.ToUpperInvariant().Contains(paramList[0].ToUpperInvariant());

                        case StringComparison.CurrentCultureIgnoreCase:
                            return value.ToUpperInvariant().Contains(paramList[0].ToUpperInvariant());

                        case StringComparison.InvariantCultureIgnoreCase:
                            return value.ToUpperInvariant().Contains(paramList[0].ToUpperInvariant());

                        default: return value.Contains(paramList[0]);
                    }
                case StringFilterOption.EndsWith:
                    return value.EndsWith(paramList[0], comparison);

                case StringFilterOption.NotEquals:
                    return !value.Equals(paramList[0], comparison);

                case StringFilterOption.GreaterThan:
                    return value.IsStringGtString(paramList[0], comparison);

                case StringFilterOption.LessThan:
                    return value.IsStringLtString(paramList[0], comparison);

                case StringFilterOption.GreaterThanOrEqualTo:
                    return value.Equals(paramList[0], comparison) || value.IsStringGtString(paramList[0], comparison);

                case StringFilterOption.LessThanOrEqualTo:
                    return value.Equals(paramList[0], comparison) || value.IsStringLtString(paramList[0], comparison);

                case StringFilterOption.Between:
                    //if (_isBetween(m[member], paramList[0], paramList[1])) rv.Add(value);
                    break;

                case StringFilterOption.Null:
                    break;

                case StringFilterOption.NotNull:
                    break;

                case StringFilterOption.NullOrWhiteSpace:
                    break;

                case StringFilterOption.NotNullOrWhiteSpace:
                    break;

                case StringFilterOption.None:
                    break;

                default:
                    throw new ArgumentOutOfRangeException("filterOption");
            }
            return false;
        }

        private static bool _isGreaterThan(dynamic value, dynamic compareTo)
        {
            return value > compareTo;
        }

        public static bool IsGreaterThan<T>(this T value, dynamic compareTo)
        {
            return _isGreaterThan(value, ClassExtensionBase.To<T>(compareTo));
        }

        private static bool _isBetween(dynamic value, dynamic lower, dynamic upper)
        {
            return value >= lower && value <= upper;
        }

        public static bool IsBetween<T>(this T value, T lower, T upper)
        {
            return _isBetween(value, lower, upper);
        }

        public static bool IsStringGtString(this String value, string compareTo, StringComparison comparison = StringComparison.Ordinal)
        {
            return String.Compare(value, compareTo, comparison) > 0;
        }

        public static bool IsStringLtString(this String value, string compareTo, StringComparison comparison = StringComparison.Ordinal)
        {
            return String.Compare(value, compareTo, comparison) < 0;
        }
    }
}