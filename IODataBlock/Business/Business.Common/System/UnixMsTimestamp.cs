using System;
using System.ComponentModel.DataAnnotations;
using Business.Common.Extensions;
using Newtonsoft.Json;

namespace Business.Common.System
{
    /// <summary>
    /// Class for Unix formatted timestamp UTC DateTime values.
    /// </summary>
    public class UnixMsTimestamp
    {
        #region Class Initialization

        //[JsonConstructor]
        public UnixMsTimestamp(){}

        /// <summary>
        /// Initializes a new instance of the <see cref="UnixMsTimestamp"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public UnixMsTimestamp(DateTime? value)
        {
            var date1 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            Value = value.HasValue && value.Value > date1 ? (Int64?)Convert.ToInt64((value.Value.ToUniversalTime() - date1).TotalMilliseconds) : null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnixMsTimestamp"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public UnixMsTimestamp(string value)
        {
            if (value.IsNullOrWhiteSpace()) Value = null;
            else Value = Int64.Parse(value);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnixMsTimestamp"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        public UnixMsTimestamp(Int64? value)
        {
            Value = value;
        }

        #endregion Class Initialization

        #region Fields and Properties

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        [Required]
        public Int64? Value { get; set; }

        #endregion Fields and Properties

        #region Conversion Operators


        /// <summary>
        /// Performs an implicit conversion from <see cref="UnixMsTimestamp"/> to <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        static public implicit operator Int64?(UnixMsTimestamp value)
        {
            return value.Value;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="UnixMsTimestamp"/> to <see cref="Nullable{T}"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        static public implicit operator DateTime?(UnixMsTimestamp value)
        {
            if (value.Value.HasValue)
            {
                return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((double) value.Value);
            }
            return null;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Int64"/> to <see cref="UnixMsTimestamp"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        static public implicit operator UnixMsTimestamp(Int64? value)
        {
            return new UnixMsTimestamp() { Value = value };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="UnixMsTimestamp"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        static public implicit operator UnixMsTimestamp(DateTime? value)
        {
            var date1 = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            var val = value.HasValue && value.Value > date1 ? (Int64?)Convert.ToInt64((value.Value.ToUniversalTime() - date1).TotalMilliseconds) : null;

            return new UnixMsTimestamp() { Value = val };
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="string"/> to <see cref="UnixMsTimestamp"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The result of the conversion.
        /// </returns>
        static public implicit operator UnixMsTimestamp(string value)
        {
            long? val;
            if (value.IsNullOrWhiteSpace()) val = null;
            else val = long.Parse(value);
            return new UnixMsTimestamp() { Value = val };
        }

        #endregion Conversion Operators

    }
}