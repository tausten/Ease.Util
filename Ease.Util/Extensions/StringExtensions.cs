//
// Copyright (c) 2019 Tyler Austen. See LICENSE file at top of repository for details.
//

using System;
using System.Globalization;

namespace Ease.Util.Extensions
{
    /// <summary>
    /// Extension methods for the <see cref="string"/> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Attempts to convert the string to a value of type <typeparamref name="T"/>, and if this fails, returns the 
        /// passed <paramref name="defaultValue"/> instead.
        /// 
        /// NOTE: A null / empty / whitespace string causes fall-back to the <paramref name="defaultValue"/>.
        /// 
        /// <code>
        ///     // Old-style AppSettings help (i.e. get the value or fall back to default)
        ///     TimeSpan myConfiguredSpan 
        ///         = ConfigurationManager.AppSettings["some.config.param"]
        ///             .ToValueOr(TimeSpan.FromSeconds(30));
        ///     
        ///     // Likewise for new-style IConfiguration...
        ///     IConfiguration config; // injected from Asp.Net Core stack
        ///     decimal myConfiguredDecimal = config["Some:Nested:Decimal:Parameter"].ToValueOr(12.34M);
        /// </code>
        /// </summary>
        /// <typeparam name="T">The target type</typeparam>
        /// <param name="theString">The input value to convert</param>
        /// <param name="defaultValue">The default to return on failure to convert</param>
        /// <param name="formatProvider">[optional] The format provider to use during conversion. Default is <see cref="CultureInfo.InvariantCulture"/></param>
        /// <returns>The value of <paramref name="theString"/> converted to <typeparamref name="T"/> or the passed <paramref name="defaultValue"/></returns>
        public static T ToValueOr<T>(this string theString, T defaultValue, IFormatProvider formatProvider = null)
        {
            var result = defaultValue;
            try
            {
                // Explicitly check for null/whitespace so we don't waste an Exception throw cycle for these well-known failure cases.
                if (!string.IsNullOrWhiteSpace(theString))
                {
                    // Handle Nullable types by getting the underlying for conversion.
                    var underlyingType = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
                    result = (T)Convert.ChangeType(theString, underlyingType, formatProvider ?? CultureInfo.InvariantCulture);
                }
            }
            catch
            {
                // Intentionally gobble failures to convert so that we gracefully return the specified default.
            }

            return result;
        }
    }
}
