using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Business.Common.Extensions
{
    /// <summary>
    /// Extension Methods for DateTime.
    /// </summary>
    public static class DateTimeExtensions
    {
        #region Utility Methods

        /// <summary>
        /// Gets the unix timestamp string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static string GetUnixTimestampString(this DateTime value, bool useUtc = true)
        {
            return DateTimeToUnixTimestampString(value, useUtc);
        }

        /// <summary>
        /// Dates the time to unix timestamp int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static Int64 DateTimeToUnixTimestampInt64(DateTime? value = null, bool useUtc = true)
        {
            if (useUtc)
            {
                var date1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                if (value < date1) value = date1;
                var ts = value.HasValue ? value.Value.ToUniversalTime() - date1 : DateTime.UtcNow - date1;
                return Convert.ToInt64(ts.TotalSeconds);
            }
            else
            {
                var date1 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local);
                if (value < date1) value = date1;
                var ts = value.HasValue ? value.Value - date1 : DateTime.Now - date1;
                return Convert.ToInt64(ts.TotalSeconds);
            }
        }

        /// <summary>
        /// Dates the time to unix timestamp string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static string DateTimeToUnixTimestampString(DateTime? value = null, bool useUtc = true)
        {
            return Convert.ToString(DateTimeToUnixTimestampInt64(value, useUtc));
        }

        /// <summary>
        /// Dates the time from unix timestamp string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static DateTime DateTimeFromUnixTimestampString(string value, bool useUtc = true)
        {
            // ReSharper disable once RedundantAssignment
            var outvar = 0L;
            return Int64.TryParse(value, out outvar) ? DateTimeFromUnixTimestampInt64(outvar, useUtc) : DateTimeFromUnixTimestampInt64(0L, useUtc);
        }

        /// <summary>
        /// Dates the time from unix timestamp int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static DateTime DateTimeFromUnixTimestampInt64(Int64 value, bool useUtc = true)
        {
            return useUtc ? new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(value) : new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Local).AddSeconds(value);
        }

        /// <summary>
        /// Gets the unix timestamp string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static string GetUnixMsTimestampString(this DateTime value, bool useUtc = true)
        {
            return DateTimeToUnixMsTimestampString(value, useUtc);
        }

        /// <summary>
        /// Dates the time to unix timestamp int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static Int64 DateTimeToUnixMsTimestampInt64(DateTime? value = null, bool useUtc = true)
        {
            if (useUtc)
            {
                var date1 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
                if (value < date1) value = date1;
                var ts = value.HasValue ? value.Value.ToUniversalTime() - date1 : DateTime.UtcNow - date1;
                return Convert.ToInt64(ts.TotalMilliseconds);
            }
            else
            {
                var date1 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local);
                if (value < date1) value = date1;
                var ts = value.HasValue ? value.Value - date1 : DateTime.Now - date1;
                return Convert.ToInt64(ts.TotalMilliseconds);
            }
        }

        /// <summary>
        /// Dates the time to unix timestamp string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static string DateTimeToUnixMsTimestampString(DateTime? value = null, bool useUtc = true)
        {
            return Convert.ToString(DateTimeToUnixMsTimestampInt64(value, useUtc));
        }

        /// <summary>
        /// Dates the time from unix timestamp string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static DateTime DateTimeFromUnixMsTimestampString(string value, bool useUtc = true)
        {
            // ReSharper disable once RedundantAssignment
            var outvar = 0L;
            return Int64.TryParse(value, out outvar) ? DateTimeFromUnixMsTimestampInt64(outvar, useUtc) : DateTimeFromUnixMsTimestampInt64(0L, useUtc);
        }

        /// <summary>
        /// Dates the time from unix timestamp int64.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="useUtc">if set to <c>true</c> [use UTC].</param>
        /// <returns></returns>
        public static DateTime DateTimeFromUnixMsTimestampInt64(Int64 value, bool useUtc = true)
        {
            return useUtc ? new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(value) : new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Local).AddMilliseconds(value);
        }

        #endregion Utility Methods

        #region Conversion  Methods

        /// <summary>
        /// Hourses the minutes offset from.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        public static string HoursMinutesOffsetFrom(this DateTime value, DateTime endDateTime)
        {
            if (endDateTime > value)
            {
                var tspan = endDateTime.Subtract(value);
                return $"{(tspan.Hours > 24 ? tspan.Days * 24 + tspan.Hours : tspan.Hours)}:{tspan.Minutes}";
            }
            else
            {
                var tspan = value.Subtract(endDateTime);
                return $"-{(tspan.Hours > 24 ? tspan.Days * 24 + tspan.Hours : tspan.Hours)}:{tspan.Minutes}";
            }
        }

        /// <summary>
        /// Dayses the hours minutes offset from.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        public static string DaysHoursMinutesOffsetFrom(this DateTime value, DateTime endDateTime)
        {
            if (endDateTime > value)
            {
                var tspan = endDateTime.Subtract(value);
                if (tspan.Days > 0) return $"{tspan.Days} days, {tspan.Hours} hours, {tspan.Minutes} minutes until";
                return $"{tspan.Hours} hours, {tspan.Minutes} minutes until";
            }
            else
            {
                var tspan = value.Subtract(endDateTime);
                if (tspan.Days > 0) return $"{tspan.Days} days, {tspan.Hours} hours, {tspan.Minutes} minutes since";
                return $"{tspan.Hours} hours, {tspan.Minutes} minutes since";
            }
        }

        //public static DateTime ToDateTime(this DateTimeObj value)
        //{
        //    return value.Value;
        //}

        //public static DateTimeObj ToDateTimeObj(this DateTime value)
        //{
        //    return new DateTimeObj(value);
        //}

        /// <summary>
        /// Toes the SQL date time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static SqlDateTime ToSqlDateTime(this DateTime value)
        {
            return new SqlDateTime(value);
        }

        /// <summary>
        /// Toes the day ticks.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ToDayTicks(this DateTime value)
        {
            var sqldt = new SqlDateTime(value);
            return sqldt.DayTicks;
        }

        /// <summary>
        /// Toes the Q hour int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int ToQHourInt(this DateTime value)
        {
            return value.Hour * 4 + value.Minute / 15;
        }

        /// <summary>
        /// Toes the specific time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns></returns>
        public static DateTime ToSpecificTime(this DateTime value, int hours, int minutes, int seconds, int milliseconds)
        {
            return value.Date.Add(new TimeSpan(0, hours, minutes, seconds, milliseconds));
        }

        /// <summary>
        /// Toes the specific time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns></returns>
        public static DateTime ToSpecificTime(this DateTime value, int hours, int minutes, int seconds)
        {
            return value.Date.Add(new TimeSpan(0, hours, minutes, seconds));
        }

        /// <summary>
        /// Toes the specific time.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static DateTime ToSpecificTime(this DateTime value, int hours, int minutes)
        {
            return value.Date.Add(new TimeSpan(0, hours, minutes));
        }

        /// <summary>
        /// Toes the range of date time string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dateFormat">The date format.</param>
        /// <param name="seperatorString">The seperator string.</param>
        /// <param name="endDateTime">The end date time.</param>
        /// <returns></returns>
        public static string ToRangeOfDateTimeString(this DateTime value, string dateFormat, string seperatorString, DateTime endDateTime)
        {
            return value.ToString(dateFormat) + seperatorString + endDateTime.ToString(dateFormat);
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="format">The format.</param>
        /// <param name="prefix">The prefix.</param>
        /// <param name="suffix">The suffix.</param>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public static string ToString(this DateTime value, string format, string prefix, string suffix)
        {
            return prefix + value.ToString(format) + suffix;
        }

        // http: //msdn.microsoft.com/en-us/library/az4se3k1.aspx for more info on formats

        /// <summary>
        /// ToString as8601 format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringAs8601Format(this DateTime value)  // ISO 8601 with Time Zone Info
        {
            return value.ToString("O");
        }

        ///// <summary>
        ///// ToString by XML convert.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <param name="dateTimeOption">The date time option.</param>
        ///// <returns></returns>
        //public static String ToStringByXmlConvert(this DateTime value, System.Xml.XmlDateTimeSerializationMode dateTimeOption)
        //{
        //    return System.Xml.XmlConvert.ToString(value, dateTimeOption);
        //}

        /// <summary>
        /// ToString as UTC offset.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringAsUtcOffset(this DateTime value)
        {
            return new DateTimeOffset(value, TimeZoneInfo.Local.GetUtcOffset(value)).ToString("o");
        }

        /// <summary>
        /// ToString as RF C1123 format.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string ToStringAsRFC1123Format(this DateTime value)
        {
            return value.ToString("r");
        }

        /// <summary>
        /// ToString as sortable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringAsSortable(this DateTime value)
        {
            return value.ToString("s");
        }

        /// <summary>
        /// ToString as U sortable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringAsUSortable(this DateTime value)
        {
            return value.ToString("u");
        }

        /// <summary>
        /// ToString default.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringDefault(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// ToString default.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringDefaultShort(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd HH:mm:ss");
        }

        /// <summary>
        /// ToString default.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringDefaultDate(this DateTime value)
        {
            return value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// ToString default.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringDefaultUtc(this DateTime value)
        {
            return value.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        /// <summary>
        /// ToString default.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string ToStringDefaultDateUtc(this DateTime value)
        {
            return value.ToUniversalTime().ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Parses from round trip UTC.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static DateTime ParseFromRoundTripUTC(string value)  // not an extension method - just a helper method to convert string to datetime!
        {
            return DateTime.Parse(value, null, DateTimeStyles.RoundtripKind);
        }

        /// <summary>
        /// Parses the exact date time string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="format">The format.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static DateTime ParseExactDateTimeString(string value, string format, CultureInfo culture)
        {
            return DateTime.ParseExact(value, format, culture);
        }

        /// <summary>
        /// Determines whether the specified value contains date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if the specified value contains date; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsDate(this string value)
        {
            if (Regex.IsMatch(value, "[0-9]{4}[-/._]{1}[0-9]{1,2}[-/._]{1}[0-9]{1,2}")) return true;
            if (Regex.IsMatch(value, "[0-9]{1,2}[-/._]{1}[0-9]{1,2}[-/._]{1}[0-9]{4}")) return true;
            if (Regex.IsMatch(value, "[1-2]{1}[0-9]{7}[^0-9]+")) return true;
            if (Regex.IsMatch(value, "[1-2]{1}[0-9]{13}[^0-9]+")) return true;
            if (Regex.IsMatch(value, "[1-2]{1}[0-9]{16}[^0-9]+")) return true;
            return false;
        }

        /// <summary>
        /// Determines whether [contains date time] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [contains date time] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsDateTime(this string value)
        {
            if (Regex.IsMatch(value, "[0-9]{4}[-/._]{1}[0-9]{1,2}[-/._]{1}[0-9]{1,2}[-/._]{1}[0-9]{1,2}[-/._:]{1}[0-9]{1,2}[-/._:]{1}[0-9]{1,2}")) return true;
            if (Regex.IsMatch(value, "[0-9]{1,2}[-/._]{1}[0-9]{1,2}[-/._]{1}[0-9]{4}[-/._]{1}[0-9]{1,2}[-/._:]{1}[0-9]{1,2}[-/._:]{1}[0-9]{1,2}")) return true;
            if (Regex.IsMatch(value, "[1-2]{1}[0-9]{13}[^0-9]+")) return true;
            if (Regex.IsMatch(value, "[1-2]{1}[0-9]{16}[^0-9]+")) return true;
            return false;
        }

        /// <summary>
        /// Dateses the in string.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> DatesInString(this string value)
        {
            var returnvalue = new List<DateTime>();

            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var match in Regex.Matches(value, "[0-9]{4}[-/._]{1}[0-9]{1,2}[-/._]{1}[0-9]{1,2}"))
            {
                var matchstr = Regex.Replace(match.ToString(), "[/._]", "-");
                var dt = DateTime.Parse(matchstr);
                returnvalue.Add(dt);
            }
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var match in Regex.Matches(value, "[0-9]{1,2}[-/._]{1}[0-9]{1,2}[-/._]{1}[0-9]{4}"))
            {
                var matchstr = Regex.Replace(match.ToString(), "[/._]", "-");
                var dt = DateTime.Parse(matchstr);
                returnvalue.Add(dt);
            }
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var match in Regex.Matches(value, "[1-2]{1}[0-9]{7}[^0-9]+"))
            {
                var matchstr = match.ToString();
                var yr = int.Parse(matchstr.Substring(0, 4));
                var month = int.Parse(matchstr.Substring(4, 2));
                var date = int.Parse(matchstr.Substring(6, 2));
                var dt = new DateTime(yr, month, date);
                returnvalue.Add(dt);
            }

            return returnvalue;
        }

        #endregion Conversion  Methods

        #region Boolean Methods

        /// <summary>
        /// Determines whether [is in same second] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same second] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameSecond(this DateTime value, DateTime compareDate)
        {
            return value.Date == compareDate.Date && value.Hour == compareDate.Hour && value.Minute == compareDate.Minute && value.Second == compareDate.Second;
        }

        /// <summary>
        /// Determines whether [is in same minute] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same minute] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameMinute(this DateTime value, DateTime compareDate)
        {
            return value.Date == compareDate.Date && value.Hour == compareDate.Hour && value.Minute == compareDate.Minute;
        }

        /// <summary>
        /// Determines whether [is in same Q hour] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same Q hour] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameQHour(this DateTime value, DateTime compareDate)
        {
            return value.StartOfQHour() == compareDate.StartOfQHour();
        }

        /// <summary>
        /// Determines whether [is in same hour] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same hour] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameHour(this DateTime value, DateTime compareDate)
        {
            return value.Date == compareDate.Date && value.Hour == compareDate.Hour;
        }

        /// <summary>
        /// Determines whether [is in same day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameDay(this DateTime value, DateTime compareDate)
        {
            return value.Date == compareDate.Date;
        }

        /// <summary>
        /// Determines whether [is in same week] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same week] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameWeek(this DateTime value, DateTime compareDate)
        {
            return value.Date.StartOfWeek() == compareDate.Date.StartOfWeek();
        }

        /// <summary>
        /// Determines whether [is in same work week] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same work week] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameWorkWeek(this DateTime value, DateTime compareDate)
        {
            return value.Date.StartOfWorkWeek() == compareDate.Date.StartOfWorkWeek();
        }

        /// <summary>
        /// Determines whether [is in same Q month] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same Q month] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameQMonth(this DateTime value, DateTime compareDate)
        {
            return value.StartOfQMonth() == compareDate.StartOfQMonth();
        }

        /// <summary>
        /// Determines whether [is in same month] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same month] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameMonth(this DateTime value, DateTime compareDate)
        {
            return value.StartOfMonth() == compareDate.StartOfMonth();
        }

        /// <summary>
        /// Determines whether [is in same year] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns>
        /// <c>true</c> if [is in same year] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInSameYear(this DateTime value, DateTime compareDate)
        {
            return value.StartOfYear() == compareDate.StartOfYear();
        }

        /// <summary>
        /// Determines whether the specified value is between.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns><c>true</c> if the specified value is between; otherwise, <c>false</c>.</returns>
        public static bool IsBetween(this DateTime value, DateTime startDate, DateTime endDate)
        {
            return value >= startDate && value <= endDate;
        }

        /// <summary>
        /// Determines whether [is greater than eq to time of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>
        /// <c>true</c> if [is greater than eq to time of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGreaterThanEqToTimeOfDay(this DateTime value, int hours, int minutes, int seconds, int milliseconds)
        {
            return value >= value.Date.Add(new TimeSpan(0, hours, minutes, seconds, milliseconds));
        }

        /// <summary>
        /// Determines whether [is greater than eq to time of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns>
        /// <c>true</c> if [is greater than eq to time of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGreaterThanEqToTimeOfDay(this DateTime value, int hours, int minutes, int seconds)
        {
            return value >= value.Date.Add(new TimeSpan(0, hours, minutes, seconds));
        }

        /// <summary>
        /// Determines whether [is greater than eq to time of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns>
        /// <c>true</c> if [is greater than eq to time of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGreaterThanEqToTimeOfDay(this DateTime value, int hours, int minutes)
        {
            return value >= value.Date.Add(new TimeSpan(0, hours, minutes));
        }

        /// <summary>
        /// Determines whether [is greater than eq to time of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <returns>
        /// <c>true</c> if [is greater than eq to time of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGreaterThanEqToTimeOfDay(this DateTime value, int hours)
        {
            return value >= value.Date.Add(new TimeSpan(0, hours, 0));
        }

        /// <summary>
        /// Determines whether [is less than eq to time of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>
        /// <c>true</c> if [is less than eq to time of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLessThanEqToTimeOfDay(this DateTime value, int hours, int minutes, int seconds, int milliseconds)
        {
            return value <= value.Date.Add(new TimeSpan(0, hours, minutes, seconds, milliseconds));
        }

        /// <summary>
        /// Determines whether [is less than eq to time of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns>
        /// <c>true</c> if [is less than eq to time of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLessThanEqToTimeOfDay(this DateTime value, int hours, int minutes, int seconds)
        {
            return value <= value.Date.Add(new TimeSpan(0, hours, minutes, seconds, 999));
        }

        /// <summary>
        /// Determines whether [is less than eq to time of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns>
        /// <c>true</c> if [is less than eq to time of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLessThanEqToTimeOfDay(this DateTime value, int hours, int minutes)
        {
            return value <= value.Date.Add(new TimeSpan(0, hours, minutes, 60, 999));
        }

        /// <summary>
        /// Determines whether [is less than eq to time of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <returns>
        /// <c>true</c> if [is less than eq to time of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLessThanEqToTimeOfDay(this DateTime value, int hours)
        {
            return value <= value.Date.Add(new TimeSpan(0, hours, 60, 60, 999));
        }

        /// <summary>
        /// Determines whether [is between hours of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startHour">The start hour.</param>
        /// <param name="endHour">The end hour.</param>
        /// <returns>
        /// <c>true</c> if [is between hours of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetweenHoursOfDay(this DateTime value, int startHour, int endHour)
        {
            return value.IsGreaterThanEqToTimeOfDay(startHour) && value.IsLessThanEqToTimeOfDay(endHour);
        }

        /// <summary>
        /// Determines whether [is between minutes of day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="startHour">The start hour.</param>
        /// <param name="startMinute">The start minute.</param>
        /// <param name="endHour">The end hour.</param>
        /// <param name="endMinute">The end minute.</param>
        /// <returns>
        /// <c>true</c> if [is between minutes of day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsBetweenMinutesOfDay(this DateTime value, int startHour, int startMinute, int endHour, int endMinute)
        {
            return value.IsGreaterThanEqToTimeOfDay(startHour, startMinute) && value.IsLessThanEqToTimeOfDay(endHour, endMinute);
        }

        /// <summary>
        /// Determines whether [is in current second] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in current second] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInCurrentSecond(this DateTime value)
        {
            var now = DateTime.Now;
            return value.Date == now.Date && value.Hour == now.Hour && value.Minute == now.Minute && value.Second == now.Second;
        }

        /// <summary>
        /// Determines whether [is in previous second] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in previous second] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInPreviousSecond(this DateTime value)
        {
            var now = DateTime.Now.AddSeconds(-1);
            return value.Date == now.Date && value.Hour == now.Hour && value.Minute == now.Minute && value.Second == now.Second;
        }

        /// <summary>
        /// Determines whether [is in next second] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in next second] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInNextSecond(this DateTime value)
        {
            var now = DateTime.Now.AddSeconds(1);
            return value.Date == now.Date && value.Hour == now.Hour && value.Minute == now.Minute && value.Second == now.Second;
        }

        /// <summary>
        /// Determines whether [is in current minute] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in current minute] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInCurrentMinute(this DateTime value)
        {
            var now = DateTime.Now;
            return value.Date == now.Date && value.Hour == now.Hour && value.Minute == now.Minute;
        }

        /// <summary>
        /// Determines whether [is in previous minute] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in previous minute] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInPreviousMinute(this DateTime value)
        {
            var now = DateTime.Now.AddMinutes(-1);
            return value.Date == now.Date && value.Hour == now.Hour && value.Minute == now.Minute;
        }

        /// <summary>
        /// Determines whether [is in next minute] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in next minute] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInNextMinute(this DateTime value)
        {
            var now = DateTime.Now.AddMinutes(1);
            return value.Date == now.Date && value.Hour == now.Hour && value.Minute == now.Minute;
        }

        /// <summary>
        /// Determines whether [is in current Q hour] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in current Q hour] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInCurrentQHour(this DateTime value)
        {
            return value.StartOfQHour() == DateTime.Now.StartOfQHour();
        }

        /// <summary>
        /// Determines whether [is in previous Q hour] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in previous Q hour] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInPreviousQHour(this DateTime value)
        {
            return value.StartOfQHour() == DateTime.Now.StartOfQHour().AddMinutes(-15);
        }

        /// <summary>
        /// Determines whether [is in next Q hour] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in next Q hour] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInNextQHour(this DateTime value)
        {
            return value.StartOfQHour() == DateTime.Now.StartOfQHour().AddMinutes(15);
        }

        /// <summary>
        /// Determines whether [is in current hour] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in current hour] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInCurrentHour(this DateTime value)
        {
            return value.Date == DateTime.Today && value.Date.Hour == DateTime.Now.Hour;
        }

        /// <summary>
        /// Determines whether [is in previous hour] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in previous hour] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInPreviousHour(this DateTime value)
        {
            return value.Date == DateTime.Now.AddHours(-1).Date && value.Hour == DateTime.Now.AddHours(-1).Hour;
        }

        /// <summary>
        /// Determines whether [is in next hour] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in next hour] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInNextHour(this DateTime value)
        {
            return value.Date == DateTime.Now.AddHours(1).Date && value.Hour == DateTime.Now.AddHours(1).Hour;
        }

        /// <summary>
        /// Determines whether the specified value is today.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is today; otherwise, <c>false</c>.</returns>
        public static bool IsToday(this DateTime value)
        {
            return value.Date == DateTime.Today;
        }

        /// <summary>
        /// Determines whether [is in past] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in past] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInPast(this DateTime value)
        {
            return value < DateTime.Now;
        }

        /// <summary>
        /// Determines whether [is in future] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is in future] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsInFuture(this DateTime value)
        {
            return value > DateTime.Now;
        }

        /// <summary>
        /// Determines whether [is week day] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is week day] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsWeekDay(this DateTime value)
        {
            switch (value.DayOfWeek)
            {
                case DayOfWeek.Saturday: return false;
                case DayOfWeek.Sunday: return false;
                default: return true;
            }
        }

        /// <summary>
        /// Determines whether the specified value is weekend.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if the specified value is weekend; otherwise, <c>false</c>.</returns>
        public static bool IsWeekend(this DateTime value)
        {
            return !value.IsWeekDay();
        }

        /// <summary>
        /// Determines whether [is start of work week] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is start of work week] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStartOfWorkWeek(this DateTime value)
        {
            return value.StartOfWorkWeek() == value.Date;
        }

        /// <summary>
        /// Determines whether [is end of work week] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is end of work week] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEndOfWorkWeek(this DateTime value)
        {
            return value.EndOfWorkWeek() == value.Date;
        }

        /// <summary>
        /// Determines whether [is start of week] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is start of week] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsStartOfWeek(this DateTime value)
        {
            return value.StartOfWorkWeek() == value.Date;
        }

        /// <summary>
        /// Determines whether [is end of week] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is end of week] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsEndOfWeek(this DateTime value)
        {
            return value.EndOfWeek() == value.Date;
        }

        /// <summary>
        /// Determines whether [is over N total milliseconds old] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>
        /// <c>true</c> if [is over N total milliseconds old] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalMillisecondsOld(this DateTime value, int milliseconds)
        {
            return DateTime.Now.Subtract(value).TotalMilliseconds > milliseconds;
        }

        /// <summary>
        /// Determines whether [is over N total milliseconds until] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="milliseconds">The milliseconds.</param>
        /// <returns>
        /// <c>true</c> if [is over N total milliseconds until] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalMillisecondsUntil(this DateTime value, int milliseconds)
        {
            return DateTime.Now.Subtract(value).TotalMilliseconds < -milliseconds;
        }

        /// <summary>
        /// Determines whether [is over N total seconds old] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns>
        /// <c>true</c> if [is over N total seconds old] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalSecondsOld(this DateTime value, int seconds)
        {
            return DateTime.Now.Subtract(value).TotalSeconds > seconds;
        }

        /// <summary>
        /// Determines whether [is over N total seconds until] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="seconds">The seconds.</param>
        /// <returns>
        /// <c>true</c> if [is over N total seconds until] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalSecondsUntil(this DateTime value, int seconds)
        {
            return DateTime.Now.Subtract(value).TotalSeconds < -seconds;
        }

        /// <summary>
        /// Determines whether [is over N total minutes old] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns>
        /// <c>true</c> if [is over N total minutes old] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalMinutesOld(this DateTime value, int minutes)
        {
            return DateTime.Now.Subtract(value).TotalMinutes > minutes;
        }

        /// <summary>
        /// Determines whether [is over N total minutes until] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns>
        /// <c>true</c> if [is over N total minutes until] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalMinutesUntil(this DateTime value, int minutes)
        {
            return DateTime.Now.Subtract(value).TotalMinutes < -minutes;
        }

        // QHs go here?????

        /// <summary>
        /// Determines whether [is over N total hours old] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <returns>
        /// <c>true</c> if [is over N total hours old] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalHoursOld(this DateTime value, int hours)
        {
            return DateTime.Now.Subtract(value).TotalHours > hours;
        }

        /// <summary>
        /// Determines whether [is over N total hours until] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <returns>
        /// <c>true</c> if [is over N total hours until] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalHoursUntil(this DateTime value, int hours)
        {
            return DateTime.Now.Subtract(value).TotalHours < -hours;
        }

        /// <summary>
        /// Determines whether [is over N total days old] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="days">The days.</param>
        /// <returns>
        /// <c>true</c> if [is over N total days old] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalDaysOld(this DateTime value, int days)
        {
            return DateTime.Now.Subtract(value).TotalDays > days;
            // can also be expressed like so:
            //
            //
            //
            //
            //
            //
            //
            //return value.Subtract(DateTime.Now).TotalDays < -(Days) ? true : false;
        }

        /// <summary>
        /// Determines whether [is over N total days until] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="days">The days.</param>
        /// <returns>
        /// <c>true</c> if [is over N total days until] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNTotalDaysUntil(this DateTime value, int days)
        {
            return value.Subtract(DateTime.Now).TotalDays > days;
            // can also be expressed like so:
            //
            //
            //
            //
            //
            //
            //
            //return DateTime.Now.Subtract(value).TotalDays < -(Days) ? true : false;
        }

        /// <summary>
        /// Determines whether [is over N dates old] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="days">The days.</param>
        /// <returns>
        /// <c>true</c> if [is over N dates old] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNDatesOld(this DateTime value, int days)
        {
            return DateTime.Today.Subtract(value.Date).TotalDays > days;
        }

        /// <summary>
        /// Determines whether [is over N dates until] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="days">The days.</param>
        /// <returns>
        /// <c>true</c> if [is over N dates until] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsOverNDatesUntil(this DateTime value, int days)
        {
            return value.Date.Subtract(DateTime.Today).TotalDays > days;
            // can also be expressed like so:
            //
            //
            //
            //
            //
            //
            //
            //return DateTime.Today.Subtract(value.Date).Days < -(Days) ? true : false;
        }

        /// <summary>
        /// Determines whether [is N day of month] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="day">The day.</param>
        /// <returns>
        /// <c>true</c> if [is N day of month] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNDayOfMonth(this DateTime value, int day)
        {
            return value.Day == day;
        }

        /// <summary>
        /// Determines whether [is N day of week in month] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="n">The N.</param>
        /// <param name="day">The day.</param>
        /// <returns>
        /// <c>true</c> if [is N day of week in month] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNDayOfWeekInMonth(this DateTime value, int n, DayOfWeek day)
        {
            return value.DayOfWeek == day && value.WeekOfMonth() == n;
        }

        /// <summary>
        /// Determines whether [is first day of week in month] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="day">The day.</param>
        /// <returns>
        /// <c>true</c> if [is first day of week in month] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsFirstDayOfWeekInMonth(this DateTime value, DayOfWeek day)
        {
            return value.DayOfWeek == day && value.WeekOfMonth() == 1;
        }

        /// <summary>
        /// Determines whether [is last day of week in month] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="day">The day.</param>
        /// <returns>
        /// <c>true</c> if [is last day of week in month] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLastDayOfWeekInMonth(this DateTime value, DayOfWeek day)
        {
            return value.GetMonthList().Where(x => x.DayOfWeek == day).Max() == value.Date;
        }

        /// <summary>
        /// Determines whether [is last day of month] [the specified value].
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// <c>true</c> if [is last day of month] [the specified value]; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsLastDayOfMonth(this DateTime value)
        {
            return value.IsInSameDay(value.StartOfMonth());
        }

        #endregion Boolean Methods

        #region Offset Methods

        /// <summary>
        /// Offsets the N values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hoursOffset">The hours offset.</param>
        /// <param name="minuteOffset">The minute offset.</param>
        /// <param name="secondOffset">The second offset.</param>
        /// <returns></returns>
        public static DateTime OffsetNValues(this DateTime value, int hoursOffset, int minuteOffset, int secondOffset)
        {
            return value.Add(new TimeSpan(hoursOffset, minuteOffset, secondOffset));
        }

        /// <summary>
        /// Offsets the N values.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="daysOffset">The days offset.</param>
        /// <param name="hoursOffset">The hours offset.</param>
        /// <param name="minuteOffset">The minute offset.</param>
        /// <param name="secondOffset">The second offset.</param>
        /// <param name="millisecondOffset">The millisecond offset.</param>
        /// <returns></returns>
        public static DateTime OffsetNValues(this DateTime value, int daysOffset, int hoursOffset, int minuteOffset, int secondOffset, int millisecondOffset)
        {
            return value.Add(new TimeSpan(daysOffset, hoursOffset, minuteOffset, secondOffset, millisecondOffset));
        }

        #endregion Offset Methods

        #region Second Methods

        /// <summary>
        /// Exactlies the N seconds from now.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="secondOffset">The second offset.</param>
        /// <returns></returns>
        public static DateTime ExactlyNSecondsFromNow(this DateTime value, int secondOffset)
        {
            return value.Add(new TimeSpan(0, 0, secondOffset));
        }

        /// <summary>
        /// Starts the of second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfSecond(this DateTime value)
        {
            return value.Date.AddHours(value.Hour).AddMinutes(value.Minute).AddSeconds(value.Second);
        }

        /// <summary>
        /// Starts the of second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="secondOffset">The second offset.</param>
        /// <returns></returns>
        public static DateTime StartOfSecond(this DateTime value, int secondOffset)
        {
            return value.StartOfSecond().AddSeconds(secondOffset);
        }

        /// <summary>
        /// Ends the of second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfSecond(this DateTime value)
        {
            return value.Date.AddHours(value.Hour).AddMinutes(value.Minute).AddSeconds(value.Second).AddMilliseconds(999);
        }

        /// <summary>
        /// Ends the of second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="secondOffset">The second offset.</param>
        /// <returns></returns>
        public static DateTime EndOfSecond(this DateTime value, int secondOffset)
        {
            return value.EndOfSecond().AddSeconds(secondOffset);
        }

        /// <summary>
        /// Starts the of previous second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousSecond(this DateTime value)
        {
            return value.StartOfSecond().AddSeconds(-1);
        }

        /// <summary>
        /// Starts the of next second.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextSecond(this DateTime value)
        {
            return value.StartOfSecond().AddSeconds(1);
        }

        #endregion Second Methods

        #region Minute Methods

        /// <summary>
        /// Starts the of minute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfMinute(this DateTime value)
        {
            return value.Date.AddHours(value.Hour).AddMinutes(value.Minute);
        }

        /// <summary>
        /// Starts the of minute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minuteOffset">The minute offset.</param>
        /// <returns></returns>
        public static DateTime StartOfMinute(this DateTime value, int minuteOffset)
        {
            return value.StartOfMinute().AddMinutes(minuteOffset);
        }

        /// <summary>
        /// Ends the of minute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfMinute(this DateTime value)
        {
            return value.Date.AddHours(value.Hour).AddMinutes(value.Minute).AddSeconds(59).AddMilliseconds(999);
        }

        /// <summary>
        /// Ends the of minute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minuteOffset">The minute offset.</param>
        /// <returns></returns>
        public static DateTime EndOfMinute(this DateTime value, int minuteOffset)
        {
            return value.EndOfMinute().AddMinutes(minuteOffset);
        }

        /// <summary>
        /// Starts the of previous minute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousMinute(this DateTime value)
        {
            return value.StartOfMinute().AddMinutes(-1);
        }

        /// <summary>
        /// Starts the of next minute.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextMinute(this DateTime value)
        {
            return value.StartOfMinute().AddMinutes(1);
        }

        #endregion Minute Methods

        #region QH Methods

        /// <summary>
        /// Starts the of Q hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfQHour(this DateTime value)
        {
            return value.Date.AddMinutes((value.Hour * 4 + value.Minute / 15) * 15);
        }

        /// <summary>
        /// Starts the of Q hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static DateTime StartOfQHour(this DateTime value, int offset)
        {
            return value.StartOfQHour().AddMinutes(offset * 15);
        }

        /// <summary>
        /// Ends the of Q hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfQHour(this DateTime value)
        {
            return value.Date.AddMinutes((value.Hour * 4 + value.Minute / 15) * 15).AddSeconds(899).AddMilliseconds(999);
        }

        /// <summary>
        /// Ends the of Q hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static DateTime EndOfQHour(this DateTime value, int offset)
        {
            return value.EndOfQHour().AddMinutes(offset * 15);
        }

        /// <summary>
        /// Starts the of previous Q hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousQHour(this DateTime value)
        {
            return value.StartOfQHour().AddMinutes(-15);
        }

        /// <summary>
        /// Starts the of next Q hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextQHour(this DateTime value)
        {
            return value.StartOfQHour().AddMinutes(15);
        }

        #endregion QH Methods

        #region Hour Methods

        /// <summary>
        /// Starts the of hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfHour(this DateTime value)
        {
            return value.Date.AddHours(value.Hour);
        }

        /// <summary>
        /// Starts the of hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static DateTime StartOfHour(this DateTime value, int offset)
        {
            return value.StartOfHour().AddHours(offset);
        }

        /// <summary>
        /// Ends the of hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfHour(this DateTime value)
        {
            return value.Date.AddHours(value.Hour).AddHours(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Ends the of hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static DateTime EndOfHour(this DateTime value, int offset)
        {
            return value.EndOfHour().AddHours(offset);
        }

        /// <summary>
        /// Starts the of previous hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousHour(this DateTime value)
        {
            return value.Date.AddHours(value.Hour).AddHours(-1);
        }

        /// <summary>
        /// Starts the of next hour.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextHour(this DateTime value)
        {
            return value.Date.AddHours(value.Hour).AddHours(1);
        }

        #endregion Hour Methods

        #region Day Methods

        #region Legacy Methods

        /// <summary>
        /// Gets the day end.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime GetDayEnd(this DateTime value)
        {
            return value.Date.AddDays(1).AddSeconds(-1);
        }

        /// <summary>
        /// Gets the day start.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime GetDayStart(this DateTime value)
        {
            return value.Date;
        }

        #endregion Legacy Methods

        /// <summary>
        /// Starts the of day.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime value)
        {
            return value.Date;
        }

        /// <summary>
        /// Starts the of day.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dayOffset">The day offset.</param>
        /// <returns></returns>
        public static DateTime StartOfDay(this DateTime value, int dayOffset)
        {
            return value.StartOfDay().AddDays(dayOffset);
        }

        /// <summary>
        /// Ends the of day.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime value)
        {
            return value.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// Ends the of day.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dayOffset">The day offset.</param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime value, int dayOffset)
        {
            return value.EndOfDay().AddDays(dayOffset);
        }

        /// <summary>
        /// Starts the of previous day.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousDay(this DateTime value)
        {
            return value.Date.AddDays(-1);
        }

        /// <summary>
        /// Starts the of previous work day.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousWorkDay(this DateTime value)
        {
            while (value.AddDays(-1).IsWeekend())
            {
                value = value.Date.AddDays(-1);
            }
            return value.Date.AddDays(-1);
        }

        /// <summary>
        /// Starts the of next day.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextDay(this DateTime value)
        {
            return value.Date.AddDays(1);
        }

        /// <summary>
        /// Starts the of next work day.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextWorkDay(this DateTime value)
        {
            while (value.AddDays(1).IsWeekend())
            {
                value = value.Date.AddDays(1);
            }
            return value.Date.AddDays(1);
        }

        /// <summary>
        /// Ns the total days until.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns></returns>
        public static int NTotalDaysUntil(this DateTime value, DateTime compareDate)
        {
            return (int)compareDate.Subtract(value).TotalDays;
        }

        /// <summary>
        /// Ns the dates until.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="compareDate">The compare date.</param>
        /// <returns></returns>
        public static int NDatesUntil(this DateTime value, DateTime compareDate)
        {
            return compareDate.Date.Subtract(value.Date).Days;
        }

        /// <summary>
        /// Days the of week int.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int DayOfWeekInt(this DateTime value)
        {
            return (int)value.StartOfMonth().DayOfWeek;
        }

        #endregion Day Methods

        #region Week Methods

        /// <summary>
        /// Starts the of week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime value)
        {
            return value.Date.AddDays(0 - (int)value.DayOfWeek);
        }

        /// <summary>
        /// Starts the of week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="weekOffset">The week offset.</param>
        /// <returns></returns>
        public static DateTime StartOfWeek(this DateTime value, int weekOffset)
        {
            return value.StartOfWeek().AddDays(weekOffset * 7);
        }

        /// <summary>
        /// Ends the of week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfWeek(this DateTime value)
        {
            return value.StartOfWeek().AddDays(6);
        }

        /// <summary>
        /// Ends the of week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="weekOffset">The week offset.</param>
        /// <returns></returns>
        public static DateTime EndOfWeek(this DateTime value, int weekOffset)
        {
            return value.EndOfWeek().AddDays(weekOffset * 7);
        }

        /// <summary>
        /// Starts the of previous week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousWeek(this DateTime value)
        {
            return value.StartOfWeek().AddDays(-7);
        }

        /// <summary>
        /// Starts the of next week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextWeek(this DateTime value)
        {
            return value.StartOfWeek().AddDays(7);
        }

        /// <summary>
        /// Starts the of work week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfWorkWeek(this DateTime value)
        {
            return value.Date.AddDays(0 - (int)value.DayOfWeek + 1);
        }

        /// <summary>
        /// Starts the of work week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static DateTime StartOfWorkWeek(this DateTime value, int offset)
        {
            return value.StartOfWorkWeek().AddDays(offset * 7);
        }

        /// <summary>
        /// Ends the of work week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfWorkWeek(this DateTime value)
        {
            return value.Date.AddDays(0 - (int)value.DayOfWeek + 5);
        }

        /// <summary>
        /// Ends the of work week.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static DateTime EndOfWorkWeek(this DateTime value, int offset)
        {
            return value.EndOfWorkWeek().AddDays(offset * 7);
        }

        /// <summary>
        /// Ends the of week bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfWeekBillDate(this DateTime value)
        {
            return value.StartOfWeek(1);
        }

        /// <summary>
        /// Ends the of week bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="weekOffset">The week offset.</param>
        /// <returns></returns>
        public static DateTime EndOfWeekBillDate(this DateTime value, int weekOffset)
        {
            return value.StartOfWeek(weekOffset + 1);
        }

        /// <summary>
        /// Ends the of week work bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfWeekWorkBillDate(this DateTime value)
        {
            value = value.StartOfWeek(1);
            while (value.IsWeekend())
            {
                value = value.AddDays(1);
            }
            return value;
        }

        /// <summary>
        /// Ends the of week work bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="weekOffset">The week offset.</param>
        /// <returns></returns>
        public static DateTime EndOfWeekWorkBillDate(this DateTime value, int weekOffset)
        {
            value = value.StartOfWeek(weekOffset + 1);
            while (value.IsWeekend())
            {
                value = value.AddDays(1);
            }
            return value;
        }

        /// <summary>
        /// Gets the day of week date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <returns></returns>
        public static DateTime GetDayOfWeekDate(this DateTime value, DayOfWeek dayOfWeek)
        {
            return value.Date.AddDays((int)dayOfWeek - value.DayOfWeekInt());
        }

        /// <summary>
        /// Gets the day of week date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="dayOfWeek">The day of week.</param>
        /// <param name="weekOffset">The week offset.</param>
        /// <returns></returns>
        public static DateTime GetDayOfWeekDate(this DateTime value, DayOfWeek dayOfWeek, int weekOffset)
        {
            return value.AddDays(weekOffset * 7).GetDayOfWeekDate(dayOfWeek);
        }

        #endregion Week Methods

        #region QMonth Methods

        /// <summary>
        /// Qs the month value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int QMonthValue(this DateTime value)
        {
            if (value.Day <= 8) return 1;
            if (value.Day >= 24) return 4;
            if (value.Day >= 9 && value.Day <= 15) return 2;
            return 3;
        }

        /// <summary>
        /// Qs the month code.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string QMonthCode(this DateTime value)
        {
            return value.Year + value.Month.ToString("00") + "QM" + QMonthValue(value).ToString("00");
        }

        /// <summary>
        /// Starts the of Q month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfQMonth(this DateTime value)
        {
            if (value.Day <= 8) return new DateTime(value.Year, value.Month, 1);
            if (value.Day >= 24) return new DateTime(value.Year, value.Month, 24);
            if (value.Day >= 9 && value.Day <= 15) return new DateTime(value.Year, value.Month, 9);
            return new DateTime(value.Year, value.Month, 16);
        }

        /// <summary>
        /// Starts the of Q month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="qMonthOffset">The Q month offset.</param>
        /// <returns></returns>
        public static DateTime StartOfQMonth(this DateTime value, int qMonthOffset)
        {
            if (qMonthOffset > 0)
            {
                while (qMonthOffset > 0)
                {
                    value = value.EndOfQMonth().AddDays(1);
                    qMonthOffset--;
                }
            }
            else if (qMonthOffset < 0)
            {
                while (qMonthOffset < 0)
                {
                    value = value.StartOfQMonth().AddDays(-1);
                    qMonthOffset++;
                }
            }
            return value.StartOfQMonth();
        }

        /// <summary>
        /// Starts the of previous Q month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousQMonth(this DateTime value)
        {
            return value.StartOfQMonth(-1);
        }

        /// <summary>
        /// Starts the of next Q month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextQMonth(this DateTime value)
        {
            return value.StartOfQMonth(1);
        }

        /// <summary>
        /// Ends the of Q month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfQMonth(this DateTime value)
        {
            if (value.Day <= 8) return new DateTime(value.Year, value.Month, 8);
            if (value.Day >= 24) return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
            if (value.Day >= 9 && value.Day <= 15) return new DateTime(value.Year, value.Month, 15);
            return new DateTime(value.Year, value.Month, 23);
        }

        /// <summary>
        /// Ends the of Q month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="qMonthOffset">The Q month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfQMonth(this DateTime value, int qMonthOffset)
        {
            if (qMonthOffset > 0)
            {
                while (qMonthOffset > 0)
                {
                    value = value.EndOfQMonth().AddDays(1);
                    qMonthOffset--;
                }
            }
            else if (qMonthOffset < 0)
            {
                while (qMonthOffset < 0)
                {
                    value = value.StartOfQMonth().AddDays(-1);
                    qMonthOffset++;
                }
            }
            return value.EndOfQMonth();
        }

        /// <summary>
        /// Ends the of Q month bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfQMonthBillDate(this DateTime value)
        {
            return value.StartOfQMonth(1);
        }

        /// <summary>
        /// Ends the of Q month bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="qMonthOffset">The Q month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfQMonthBillDate(this DateTime value, int qMonthOffset)
        {
            return value.StartOfQMonth(qMonthOffset + 1);
        }

        /// <summary>
        /// Ends the of Q month work bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfQMonthWorkBillDate(this DateTime value)
        {
            value = value.StartOfQMonth(1);
            while (value.IsWeekend())
            {
                value = value.AddDays(1);
            }
            return value;
        }

        /// <summary>
        /// Ends the of Q month work bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="qMonthOffset">The Q month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfQMonthWorkBillDate(this DateTime value, int qMonthOffset)
        {
            value = value.StartOfQMonth(qMonthOffset + 1);
            while (value.IsWeekend())
            {
                value = value.AddDays(1);
            }
            return value;
        }

        #endregion QMonth Methods

        #region SemiMonth Methods

        /// <summary>
        /// Semis the month value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int SemiMonthValue(this DateTime value)
        {
            if (value.Day <= 15) return 1;
            return 2;
        }

        /// <summary>
        /// Semis the month code.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string SemiMonthCode(this DateTime value)
        {
            return value.Year + value.Month.ToString("00") + "SM" + SemiMonthValue(value).ToString("00");
        }

        /// <summary>
        /// Starts the of semi month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfSemiMonth(this DateTime value)
        {
            if (value.Day <= 15) return new DateTime(value.Year, value.Month, 1);
            return new DateTime(value.Year, value.Month, 16);
        }

        /// <summary>
        /// Starts the of semi month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="semiMonthOffset">The semi month offset.</param>
        /// <returns></returns>
        public static DateTime StartOfSemiMonth(this DateTime value, int semiMonthOffset)
        {
            if (semiMonthOffset > 0)
            {
                while (semiMonthOffset > 0)
                {
                    value = value.EndOfSemiMonth().AddDays(1);
                    semiMonthOffset--;
                }
            }
            else if (semiMonthOffset < 0)
            {
                while (semiMonthOffset < 0)
                {
                    value = value.StartOfSemiMonth().AddDays(-1);
                    semiMonthOffset++;
                }
            }
            return value.StartOfSemiMonth();
        }

        /// <summary>
        /// Starts the of previous semi month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousSemiMonth(this DateTime value)
        {
            return value.StartOfSemiMonth(-1);
        }

        /// <summary>
        /// Starts the of next semi month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextSemiMonth(this DateTime value)
        {
            return value.StartOfSemiMonth(1);
        }

        /// <summary>
        /// Ends the of semi month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfSemiMonth(this DateTime value)
        {
            if (value.Day <= 15) return new DateTime(value.Year, value.Month, 15);
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        /// <summary>
        /// Ends the of semi month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="semiMonthOffset">The semi month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfSemiMonth(this DateTime value, int semiMonthOffset)
        {
            if (semiMonthOffset > 0)
            {
                while (semiMonthOffset > 0)
                {
                    value = value.EndOfSemiMonth().AddDays(1);
                    semiMonthOffset--;
                }
            }
            else if (semiMonthOffset < 0)
            {
                while (semiMonthOffset < 0)
                {
                    value = value.StartOfSemiMonth().AddDays(-1);
                    semiMonthOffset++;
                }
            }
            return value.EndOfSemiMonth();
        }

        /// <summary>
        /// Ends the of semi month bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfSemiMonthBillDate(this DateTime value)
        {
            return value.StartOfSemiMonth(1);
        }

        /// <summary>
        /// Ends the of semi month bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="semiMonthOffset">The semi month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfSemiMonthBillDate(this DateTime value, int semiMonthOffset)
        {
            return value.StartOfSemiMonth(semiMonthOffset + 1);
        }

        /// <summary>
        /// Ends the of semi month work bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfSemiMonthWorkBillDate(this DateTime value)
        {
            value = value.StartOfSemiMonth(1);
            while (value.IsWeekend())
            {
                value = value.AddDays(1);
            }
            return value;
        }

        /// <summary>
        /// Ends the of semi month work bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="semiMonthOffset">The semi month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfSemiMonthWorkBillDate(this DateTime value, int semiMonthOffset)
        {
            value = value.StartOfSemiMonth(semiMonthOffset + 1);
            while (value.IsWeekend())
            {
                value = value.AddDays(1);
            }
            return value;
        }

        #endregion SemiMonth Methods

        #region Month Methods

        /// <summary>
        /// Monthes the code.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static string MonthCode(this DateTime value)
        {
            return value.Year + value.Month.ToString("00");
        }

        /// <summary>
        /// Starts the of month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, 1);
        }

        /// <summary>
        /// Starts the of month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="monthOffset">The month offset.</param>
        /// <returns></returns>
        public static DateTime StartOfMonth(this DateTime value, int monthOffset)
        {
            return new DateTime(value.Year, value.Month, 1).AddMonths(monthOffset);
        }

        /// <summary>
        /// Starts the of previous month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousMonth(this DateTime value)
        {
            return value.StartOfMonth(-1);
        }

        /// <summary>
        /// Starts the of next month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextMonth(this DateTime value)
        {
            return value.StartOfMonth(1);
        }

        /// <summary>
        /// Ends the of month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        /// <summary>
        /// Ends the of month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="monthOffset">The month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfMonth(this DateTime value, int monthOffset)
        {
            value = value.AddMonths(monthOffset);
            return new DateTime(value.Year, value.Month, DateTime.DaysInMonth(value.Year, value.Month));
        }

        /// <summary>
        /// Ends the of month bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfMonthBillDate(this DateTime value)
        {
            return value.StartOfMonth(1);
        }

        /// <summary>
        /// Ends the of month bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="monthOffset">The month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfMonthBillDate(this DateTime value, int monthOffset)
        {
            return value.StartOfMonth(monthOffset + 1);
        }

        /// <summary>
        /// Ends the of month work bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfMonthWorkBillDate(this DateTime value)
        {
            value = value.StartOfMonth(1);
            while (value.IsWeekend())
            {
                value = value.AddDays(1);
            }
            return value;
        }

        /// <summary>
        /// Ends the of month work bill date.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="monthOffset">The month offset.</param>
        /// <returns></returns>
        public static DateTime EndOfMonthWorkBillDate(this DateTime value, int monthOffset)
        {
            value = value.StartOfMonth(monthOffset + 1);
            while (value.IsWeekend())
            {
                value = value.AddDays(1);
            }
            return value;
        }

        /// <summary>
        /// Weeks the of month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int WeekOfMonth(this DateTime value)
        {
            return (int)(((value - value.StartOfMonth()).TotalDays + value.StartOfMonth().DayOfWeekInt()) / 7) + 1;
        }

        /// <summary>
        /// Weekses the in month.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static int WeeksInMonth(this DateTime value)
        {
            return value.EndOfMonth().WeekOfMonth();
        }

        #endregion Month Methods

        #region Year Methods

        /// <summary>
        /// Starts the of year.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfYear(this DateTime value)
        {
            return new DateTime(value.Year, 1, 1);
        }

        /// <summary>
        /// Starts the of year.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="yearOffset">The year offset.</param>
        /// <returns></returns>
        public static DateTime StartOfYear(this DateTime value, int yearOffset)
        {
            return value.StartOfYear().AddYears(yearOffset);
        }

        /// <summary>
        /// Ends the of year.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime EndOfYear(this DateTime value)
        {
            return new DateTime(value.Year, 12, 31);
        }

        /// <summary>
        /// Ends the of year.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="yearOffset">The year offset.</param>
        /// <returns></returns>
        public static DateTime EndOfYear(this DateTime value, int yearOffset)
        {
            return value.EndOfYear().AddYears(yearOffset);
        }

        /// <summary>
        /// Starts the of previous year.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfPreviousYear(this DateTime value)
        {
            return value.StartOfYear(-1);
        }

        /// <summary>
        /// Starts the of next year.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static DateTime StartOfNextYear(this DateTime value)
        {
            return value.StartOfYear(1);
        }

        #endregion Year Methods

        #region Date Collections

        #region Minute Collections

        #region List Methods

        /// <summary>
        /// Gets the minutes to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetMinutesToList(this DateTime value, DateTime to)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfMinute();
            if (to > value)
            {
                while (tempdt <= to.StartOfMinute())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddMinutes(1);
                }
            }
            else
            {
                while (tempdt >= to.StartOfMinute())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddMinutes(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the minutes from to list.
        /// </summary>
        /// <param name="startMinute">From.</param>
        /// <param name="endMinute">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetMinutesFromToList(DateTime startMinute, DateTime endMinute) // helper method
        {
            var rv = new List<DateTime>();
            var tempdt = startMinute.StartOfMinute();
            if (endMinute > startMinute)
            {
                while (tempdt <= endMinute.StartOfMinute())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddMinutes(1);
                }
            }
            else
            {
                while (tempdt >= endMinute.StartOfMinute())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddMinutes(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the minutes in hour list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetMinutesInHourList(this DateTime value)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfHour();
            while (tempdt <= value.EndOfHour())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddMinutes(1);
            }
            return rv;
        }

        /// <summary>
        /// Gets the minutes in hour list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetMinutesInHourList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfHour();
            while (tempdt <= value.EndOfHour())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddMinutes(1);
                if (!tempdt.IsInPast() && excludeFutureDates) break;
            }
            return rv;
        }

        /// <summary>
        /// Gets the N minutes to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static List<DateTime> GetNMinutesToList(this DateTime value, int minutes)
        {
            return value.GetMinutesToList(value.EndOfMinute().AddMinutes(minutes));
        }

        #endregion List Methods

        #region IEnumerable Methods

        /// <summary>
        /// Gets the minutes to IEnumerable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetMinutesToIEnumerable(this DateTime value, DateTime to)
        {
            return GetMinutesFromToIEnumerable(value, to);

            //var tempdt = value.StartOfMinute();
            //if (to > value)
            //{
            //    while (tempdt <= to.StartOfMinute())
            //    {
            //        yield return tempdt.AddMinutes(1);
            //    }
            //}
            //else
            //{
            //    while (tempdt >= to.StartOfMinute())
            //    {
            //        yield return tempdt.AddMinutes(-1);
            //    }
            //}
        }

        /// <summary>
        /// Gets the minutes from to IEnumerable.
        /// </summary>
        /// <param name="startMinute">From.</param>
        /// <param name="endMinute">To.</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetMinutesFromToIEnumerable(DateTime startMinute, DateTime endMinute) // helper method
        {
            if (endMinute > startMinute)
            {
                var tempdt = startMinute.StartOfMinute().AddMinutes(-1);
                endMinute = endMinute.StartOfMinute().AddMinutes(-1);
                while (tempdt <= endMinute)
                {
                    tempdt = tempdt.AddMinutes(1);
                    yield return tempdt;
                }
            }
            else
            {
                var tempdt = startMinute.StartOfMinute().AddMinutes(1);
                endMinute = endMinute.StartOfMinute().AddMinutes(1);
                while (tempdt >= endMinute)
                {
                    tempdt = tempdt.AddMinutes(-1);
                    yield return tempdt;
                }
            }
        }

        /// <summary>
        /// Gets the minutes in hour IEnumerable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <param name="excludePastDates">if set to <c>true</c> [exclude past dates].</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetMinutesInHourIEnumerable(this DateTime value, bool excludeFutureDates = false, bool excludePastDates = false)
        {
            var tempdt = excludePastDates ? value.StartOfMinute().AddMinutes(-1) : value.StartOfHour().AddMinutes(-1);
            while (tempdt <= value.EndOfHour())
            {
                tempdt = tempdt.AddMinutes(1);
                if (!tempdt.IsInPast() && excludeFutureDates) break;
                yield return tempdt;
            }
        }

        /// <summary>
        /// Gets the N minutes to IEnumerable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetNextNMinutesToIEnumerable(this DateTime value, int minutes)
        {
            return value.GetMinutesToIEnumerable(value.StartOfMinute().AddMinutes(minutes));
        }

        /// <summary>
        /// Gets the N minutes to IEnumerable.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minutes">The minutes.</param>
        /// <returns></returns>
        public static IEnumerable<DateTime> GetPreviousNMinutesToIEnumerable(this DateTime value, int minutes)
        {
            return value.GetMinutesToIEnumerable(value.StartOfMinute().AddMinutes(-minutes));
        }

        #endregion IEnumerable Methods

        #endregion Minute Collections

        #region QHour Collections

        /// <summary>
        /// Adds the start of Q hours.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static DateTime AddStartOfQHours(this DateTime value, int offset)
        {
            return value.StartOfQHour().AddMinutes(offset * 15);
        }

        /// <summary>
        /// Gets the start of Q hours to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetStartOfQHoursToList(this DateTime value, DateTime to)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfQHour();
            if (to > value)
            {
                while (tempdt <= to.StartOfQHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddStartOfQHours(1);
                }
            }
            else
            {
                while (tempdt >= to.StartOfQHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddStartOfQHours(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the start of Q hours from to list.
        /// </summary>
        /// <param name="startDateTime">From.</param>
        /// <param name="endDateTime">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetStartOfQHoursFromToList(DateTime startDateTime, DateTime endDateTime) // helper method
        {
            var rv = new List<DateTime>();
            var tempdt = startDateTime.StartOfQHour();
            if (endDateTime > startDateTime)
            {
                while (tempdt <= endDateTime.StartOfQHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddStartOfQHours(1);
                }
            }
            else
            {
                while (tempdt >= endDateTime.StartOfQHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddStartOfQHours(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the start of Q hours in day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetStartOfQHoursInDayList(this DateTime value)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfDay();
            while (tempdt <= value.EndOfDay())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddStartOfQHours(1);
            }
            return rv;
        }

        /// <summary>
        /// Gets the start of Q hours in day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetStartOfQHoursInDayList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfDay();
            while (tempdt <= value.EndOfDay())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddStartOfQHours(1);
                if (!tempdt.IsInPast() && excludeFutureDates) break;
            }
            return rv;
        }

        /// <summary>
        /// Gets the N start of Q hours to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="qHours">The Q hours.</param>
        /// <returns></returns>
        public static List<DateTime> GetNStartOfQHoursToList(this DateTime value, int qHours)
        {
            return value.GetStartOfQHoursToList(value.AddStartOfQHours(qHours));
        }

        /// <summary>
        /// Adds the end of Q hours.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        public static DateTime AddEndOfQHours(this DateTime value, int offset)
        {
            return value.EndOfQHour().AddMinutes(offset * 15);
        }

        /// <summary>
        /// Gets the end of Q hours to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetEndOfQHoursToList(this DateTime value, DateTime to)
        {
            var rv = new List<DateTime>();
            var tempdt = value.EndOfQHour();
            if (to > value)
            {
                while (tempdt <= to.EndOfQHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddEndOfQHours(1);
                }
            }
            else
            {
                while (tempdt >= to.EndOfQHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddEndOfQHours(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the end of Q hours from to list.
        /// </summary>
        /// <param name="startDateTime">From.</param>
        /// <param name="endDateTime">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetEndOfQHoursFromToList(DateTime startDateTime, DateTime endDateTime) // helper method
        {
            var rv = new List<DateTime>();
            var tempdt = startDateTime.EndOfQHour();
            if (endDateTime > startDateTime)
            {
                while (tempdt <= endDateTime.EndOfQHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddEndOfQHours(1);
                }
            }
            else
            {
                while (tempdt >= endDateTime.EndOfQHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddEndOfQHours(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the end of Q hours in day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetEndOfQHoursInDayList(this DateTime value)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfDay().EndOfQHour();
            while (tempdt <= value.EndOfDay())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddEndOfQHours(1);
            }
            return rv;
        }

        /// <summary>
        /// Gets the end of Q hours in day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetEndOfQHoursInDayList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfDay().EndOfQHour();
            while (tempdt <= value.EndOfDay())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddEndOfQHours(1);
                if (!tempdt.IsInPast() && excludeFutureDates) break;
            }
            return rv;
        }

        /// <summary>
        /// Gets the N end of Q hours to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="qHours">The Q hours.</param>
        /// <returns></returns>
        public static List<DateTime> GetNEndOfQHoursToList(this DateTime value, int qHours)
        {
            return value.GetEndOfQHoursToList(value.AddEndOfQHours(qHours));
        }

        #endregion QHour Collections

        #region Hour Collections

        /// <summary>
        /// Gets the start of hours to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetStartOfHoursToList(this DateTime value, DateTime to)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfHour();
            if (to > value)
            {
                while (tempdt <= to.StartOfHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddHours(1);
                }
            }
            else
            {
                while (tempdt >= to.StartOfHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddHours(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the start of hours from to list.
        /// </summary>
        /// <param name="startDateTime">From.</param>
        /// <param name="endDateTime">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetStartOfHoursFromToList(DateTime startDateTime, DateTime endDateTime) // helper method
        {
            var rv = new List<DateTime>();
            var tempdt = startDateTime.StartOfHour();
            if (endDateTime > startDateTime)
            {
                while (tempdt <= endDateTime.StartOfHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddHours(1);
                }
            }
            else
            {
                while (tempdt >= endDateTime.StartOfHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddHours(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the start of hours in day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetStartOfHoursInDayList(this DateTime value)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfDay();
            while (tempdt <= value.EndOfDay())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddHours(1);
            }
            return rv;
        }

        /// <summary>
        /// Gets the start of hours in day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetStartOfHoursInDayList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfDay();
            while (tempdt <= value.EndOfDay())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddHours(1);
                if (!tempdt.IsInPast() && excludeFutureDates) break;
            }
            return rv;
        }

        /// <summary>
        /// Gets the N start of hours to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <returns></returns>
        public static List<DateTime> GetNStartOfHoursToList(this DateTime value, int hours)
        {
            return value.GetStartOfHoursToList(value.EndOfHour().AddHours(hours));
        }

        /// <summary>
        /// Gets the end of hours to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetEndOfHoursToList(this DateTime value, DateTime to)
        {
            var rv = new List<DateTime>();
            var tempdt = value.EndOfHour();
            if (to > value)
            {
                while (tempdt <= to.EndOfHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddHours(1);
                }
            }
            else
            {
                while (tempdt >= to.EndOfHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddHours(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the end of hours from to list.
        /// </summary>
        /// <param name="startDateTime">From.</param>
        /// <param name="endDateTime">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetEndOfHoursFromToList(DateTime startDateTime, DateTime endDateTime) // helper method
        {
            var rv = new List<DateTime>();
            var tempdt = startDateTime.EndOfHour();
            if (endDateTime > startDateTime)
            {
                while (tempdt <= endDateTime.EndOfHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddHours(1);
                }
            }
            else
            {
                while (tempdt >= endDateTime.EndOfHour())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddHours(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the end of hours in day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetEndOfHoursInDayList(this DateTime value)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfDay();
            while (tempdt <= value.EndOfDay())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddHours(1);
            }
            return rv;
        }

        /// <summary>
        /// Gets the end of hours in day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetEndOfHoursInDayList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfDay();
            while (tempdt <= value.EndOfDay())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddHours(1);
                if (!tempdt.IsInPast() && excludeFutureDates) break;
            }
            return rv;
        }

        /// <summary>
        /// Gets the N end of hours to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="hours">The hours.</param>
        /// <returns></returns>
        public static List<DateTime> GetNEndOfHoursToList(this DateTime value, int hours)
        {
            return value.GetEndOfHoursToList(value.EndOfHour().AddHours(hours));
        }

        #endregion Hour Collections

        #region Day Collections

        /// <summary>
        /// Gets the dates to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetDatesToList(this DateTime value, DateTime to)
        {
            var rv = new List<DateTime>();
            var tempdt = value.Date;
            if (to > value)
            {
                while (tempdt <= to.Date)
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddDays(1);
                }
            }
            else
            {
                while (tempdt >= to.Date)
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddDays(-1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the dates to list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="to">To.</param>
        /// <param name="dayOffset">The day offset.</param>
        /// <returns></returns>
        public static List<DateTime> GetDatesToList(this DateTime value, DateTime to, int dayOffset)
        {
            return value.AddDays(dayOffset).GetDatesToList(to);
        }

        /// <summary>
        /// Gets the dates from to list.
        /// </summary>
        /// <param name="startDateTime">From.</param>
        /// <param name="endDateTime">To.</param>
        /// <returns></returns>
        public static List<DateTime> GetDatesFromToList(DateTime startDateTime, DateTime endDateTime) // helper method
        {
            var rv = new List<DateTime>();
            var tempdt = startDateTime.Date;
            if (endDateTime > startDateTime)
            {
                while (tempdt <= endDateTime.Date)
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddDays(1);
                }
            }
            else
            {
                while (tempdt >= endDateTime.Date)
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddDays(-1);
                }
            }
            return rv;
        }

        #endregion Day Collections

        #region Week Collections

        /// <summary>
        /// Gets the week list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetWeekList(this DateTime value)
        {
            var rv = new List<DateTime>();
            for (var i = 0; i < 7; i++)
            {
                rv.Add(value.StartOfWeek().AddDays(i));
            }
            return rv;
        }

        /// <summary>
        /// Gets the week list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetWeekList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            for (var i = 0; i < 7; i++)
            {
                var tempdate = value.StartOfWeek().AddDays(i);
                if (!tempdate.IsInPast() && excludeFutureDates) break;
                rv.Add(tempdate);
            }
            return rv;
        }

        /// <summary>
        /// Gets the week list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="weekOffset">The week offset.</param>
        /// <returns></returns>
        public static List<DateTime> GetWeekList(this DateTime value, int weekOffset)
        {
            var rv = new List<DateTime>();
            for (var i = 0; i < 7; i++)
            {
                rv.Add(value.StartOfWeek().AddDays(i + weekOffset * 7));
            }
            return rv;
        }

        /// <summary>
        /// Gets the work week list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetWorkWeekList(this DateTime value)
        {
            var rv = new List<DateTime>();
            for (var i = 1; i < 6; i++)
            {
                rv.Add(value.StartOfWeek().AddDays(i));
            }
            return rv;
        }

        /// <summary>
        /// Gets the work week list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetWorkWeekList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            for (var i = 1; i < 6; i++)
            {
                var tempdate = value.StartOfWeek().AddDays(i);
                if (!tempdate.IsInPast() && excludeFutureDates) break;
                rv.Add(tempdate);
            }
            return rv;
        }

        /// <summary>
        /// Gets the work week list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="weekOffset">The week offset.</param>
        /// <returns></returns>
        public static List<DateTime> GetWorkWeekList(this DateTime value, int weekOffset)
        {
            var rv = new List<DateTime>();
            for (var i = 1; i < 6; i++)
            {
                rv.Add(value.StartOfWeek().AddDays(i + weekOffset * 7));
            }
            return rv;
        }

        #endregion Week Collections

        #region QMonth Collections

        /// <summary>
        /// Gets the Q month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetQMonthList(this DateTime value)
        {
            return value.GetQMonthList(false);
        }

        /// <summary>
        /// Gets the Q month work day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetQMonthWorkDayList(this DateTime value)
        {
            return value.GetQMonthList().Where(x => x.IsWeekDay()).ToList();
        }

        /// <summary>
        /// Gets the Q month weekend list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetQMonthWeekendList(this DateTime value)
        {
            return value.GetQMonthList().Where(x => x.IsWeekend()).ToList();
        }

        /// <summary>
        /// Gets the Q month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetQMonthList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            if (excludeFutureDates)
            {
                DateTime tempdate;
                if (value.Day <= 8)
                {
                    for (var i = 1; i <= 8; i++)
                    {
                        tempdate = new DateTime(value.Year, value.Month, i);
                        if (!tempdate.IsInPast()) break;
                        rv.Add(tempdate);
                    }
                }
                else if (value.Day >= 24)
                {
                    for (var i = 24; i <= DateTime.DaysInMonth(value.Year, value.Month); i++)
                    {
                        tempdate = new DateTime(value.Year, value.Month, i);
                        if (!tempdate.IsInPast()) break;
                        rv.Add(tempdate);
                    }
                }
                else if (value.Day >= 9 && value.Day <= 15)
                {
                    for (var i = 9; i <= 15; i++)
                    {
                        tempdate = new DateTime(value.Year, value.Month, i);
                        if (!tempdate.IsInPast()) break;
                        rv.Add(tempdate);
                    }
                }
                else
                {
                    for (var i = 16; i <= 23; i++)
                    {
                        tempdate = new DateTime(value.Year, value.Month, i);
                        if (!tempdate.IsInPast()) break;
                        rv.Add(tempdate);
                    }
                }
            }
            else
            {
                if (value.Day <= 8)
                {
                    for (var i = 1; i <= 8; i++)
                    {
                        rv.Add(new DateTime(value.Year, value.Month, i));
                    }
                }
                else if (value.Day >= 24)
                {
                    for (var i = 24; i <= DateTime.DaysInMonth(value.Year, value.Month); i++)
                    {
                        rv.Add(new DateTime(value.Year, value.Month, i));
                    }
                }
                else if (value.Day >= 9 && value.Day <= 15)
                {
                    for (var i = 9; i <= 15; i++)
                    {
                        rv.Add(new DateTime(value.Year, value.Month, i));
                    }
                }
                else
                {
                    for (var i = 16; i <= 23; i++)
                    {
                        rv.Add(new DateTime(value.Year, value.Month, i));
                    }
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the Q month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="qMonthOffset">The Q month offset.</param>
        /// <returns></returns>
        public static List<DateTime> GetQMonthList(this DateTime value, int qMonthOffset)
        {
            return GetQMonthList(value.StartOfQMonth(qMonthOffset));
        }

        /// <summary>
        /// Gets the Q month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="qMonthOffset">The Q month offset.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetQMonthList(this DateTime value, int qMonthOffset, bool excludeFutureDates)
        {
            return GetQMonthList(value.StartOfQMonth(qMonthOffset), excludeFutureDates);
        }

        #endregion QMonth Collections

        #region Semi-Month Collections

        /// <summary>
        /// Gets the semi month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetSemiMonthList(this DateTime value)
        {
            var rv = new List<DateTime>();

            if (value.Day < 16)
            {
                for (var i = 1; i <= 15; i++)
                {
                    rv.Add(new DateTime(value.Year, value.Month, i));
                }
            }
            else
            {
                for (var i = 16; i <= DateTime.DaysInMonth(value.Year, value.Month); i++)
                {
                    rv.Add(new DateTime(value.Year, value.Month, i));
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the semi month work day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetSemiMonthWorkDayList(this DateTime value)
        {
            return value.GetSemiMonthList().Where(x => x.IsWeekDay()).ToList();
        }

        /// <summary>
        /// Gets the semi month weekend list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetSemiMonthWeekendList(this DateTime value)
        {
            return value.GetSemiMonthList().Where(x => x.IsWeekend()).ToList();
        }

        /// <summary>
        /// Gets the semi month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetSemiMonthList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            DateTime tempdate;
            if (value.Day < 16)
            {
                for (var i = 1; i <= 15; i++)
                {
                    tempdate = new DateTime(value.Year, value.Month, i);
                    if (!tempdate.IsInPast() && excludeFutureDates) break;
                    rv.Add(tempdate);
                }
            }
            else
            {
                for (var i = 16; i <= DateTime.DaysInMonth(value.Year, value.Month); i++)
                {
                    tempdate = new DateTime(value.Year, value.Month, i);
                    if (!tempdate.IsInPast() && excludeFutureDates) break;
                    rv.Add(tempdate);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the semi month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="semiMonthOffset">The semi month offset.</param>
        /// <returns></returns>
        public static List<DateTime> GetSemiMonthList(this DateTime value, int semiMonthOffset)
        {
            if (semiMonthOffset > 0)
            {
                for (var i = semiMonthOffset; i > 0; i--)
                {
                    value = value.Day < 16 ? value.StartOfMonth().AddDays(16) : value.StartOfNextMonth();
                }
            }
            else
            {
                for (var i = semiMonthOffset; i < 0; i++)
                {
                    value = value.Day < 16 ? value.StartOfPreviousMonth().AddDays(16) : value.StartOfMonth();
                }
            }
            return value.GetSemiMonthList();
        }

        /// <summary>
        /// Gets the semi month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="semiMonthOffset">The semi month offset.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetSemiMonthList(this DateTime value, int semiMonthOffset, bool excludeFutureDates)
        {
            if (semiMonthOffset > 0)
            {
                for (var i = semiMonthOffset; i > 0; i--)
                {
                    value = value.Day < 16 ? value.StartOfMonth().AddDays(16) : value.StartOfNextMonth();
                }
            }
            else
            {
                for (var i = semiMonthOffset; i < 0; i++)
                {
                    value = value.Day < 16 ? value.StartOfPreviousMonth().AddDays(16) : value.StartOfMonth();
                }
            }
            return value.GetSemiMonthList(excludeFutureDates);
        }

        #endregion Semi-Month Collections

        #region Month Collections

        /// <summary>
        /// Gets the month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetMonthList(this DateTime value)
        {
            var rv = new List<DateTime>();
            for (var i = 1; i <= DateTime.DaysInMonth(value.Year, value.Month); i++)
            {
                rv.Add(new DateTime(value.Year, value.Month, i));
            }
            return rv;
        }

        /// <summary>
        /// Gets the month work day list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetMonthWorkDayList(this DateTime value)
        {
            return value.GetMonthList().Where(x => x.IsWeekDay()).ToList();
        }

        /// <summary>
        /// Gets the month weekend list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetMonthWeekendList(this DateTime value)
        {
            return value.GetMonthList().Where(x => x.IsWeekend()).ToList();
        }

        /// <summary>
        /// Gets the month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetMonthList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            for (var i = 1; i <= DateTime.DaysInMonth(value.Year, value.Month); i++)
            {
                var tempdate = new DateTime(value.Year, value.Month, i);
                if (!tempdate.IsInPast() && excludeFutureDates) break;
                rv.Add(tempdate);
            }
            return rv;
        }

        /// <summary>
        /// Gets the month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="monthOffset">The month offset.</param>
        /// <returns></returns>
        public static List<DateTime> GetMonthList(this DateTime value, int monthOffset)
        {
            value = value.AddMonths(monthOffset);
            return value.GetMonthList();
        }

        /// <summary>
        /// Gets the month list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="monthOffset">The month offset.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetMonthList(this DateTime value, int monthOffset, bool excludeFutureDates)
        {
            value = value.AddMonths(monthOffset);
            return value.GetMonthList(excludeFutureDates);
        }

        #endregion Month Collections

        #region Year Collections

        /// <summary>
        /// Gets the year list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public static List<DateTime> GetYearList(this DateTime value)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfYear();
            while (tempdt <= value.EndOfYear())
            {
                rv.Add(tempdt);
                tempdt = tempdt.AddDays(1);
            }
            return rv;
        }

        /// <summary>
        /// Gets the year list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="excludeFutureDates">if set to <c>true</c> [exclude future dates].</param>
        /// <returns></returns>
        public static List<DateTime> GetYearList(this DateTime value, bool excludeFutureDates)
        {
            var rv = new List<DateTime>();
            var tempdt = value.StartOfYear();
            if (excludeFutureDates)
            {
                while (tempdt <= value.EndOfYear())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddDays(1);
                    if (!tempdt.IsInPast()) break;
                }
            }
            else
            {
                while (tempdt <= value.EndOfYear())
                {
                    rv.Add(tempdt);
                    tempdt = tempdt.AddDays(1);
                }
            }
            return rv;
        }

        /// <summary>
        /// Gets the year list.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="yearOffset">The year offset.</param>
        /// <returns></returns>
        public static List<DateTime> GetYearList(this DateTime value, int yearOffset)
        {
            value = value.AddYears(yearOffset);
            return value.GetYearList();
        }

        #endregion Year Collections

        #endregion Date Collections
    }
}