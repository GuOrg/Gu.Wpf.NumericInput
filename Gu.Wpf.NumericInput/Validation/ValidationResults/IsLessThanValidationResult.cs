namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when <see cref="NumericBox{T}.Value"/> is less than <see cref="NumericBox{T}.MinValue"/>.</summary>
    public class IsLessThanValidationResult : OutOfRangeValidationResult
    {
        /// <summary>
        /// When user typed in too small value.
        /// </summary>
        public static readonly OneParameterFormatAndCulture PleaseEnterAValueGreaterThanOrEqualToFormatAndCulture = OneParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_greater_than_or_equal_to__0__));

        /// <summary>
        /// Initializes a new instance of the <see cref="IsLessThanValidationResult"/> class.
        /// </summary>
        /// <param name="value">The <see cref="IFormattable"/>.</param>
        /// <param name="min">The min <see cref="IFormattable"/>.</param>
        /// <param name="max">The max <see cref="IFormattable"/>.</param>
        /// <param name="currentBoxCulture">The <see cref="IFormatProvider"/>.</param>
        /// <param name="formatAndCulture">The <see cref="IFormatAndCulture"/>.</param>
        /// <param name="isValid">The <see cref="bool"/>.</param>
        /// <param name="errorContent">The <see cref="object"/>.</param>
        public IsLessThanValidationResult(
            IFormattable value,
            IFormattable? min,
            IFormattable? max,
            IFormatProvider currentBoxCulture,
            IFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(value, min, max, currentBoxCulture, formatAndCulture, isValid, errorContent)
        {
        }

        /// <summary>
        /// Creates a <see cref="IsLessThanValidationResult"/>.
        /// </summary>
        /// <typeparam name="T">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="box">The <see cref="NumericBox{T}"/>.</param>
        /// <returns>A <see cref="IsLessThanValidationResult"/>.</returns>
        public static IsLessThanValidationResult CreateErrorResult<T>(T value, NumericBox<T> box)
             where T : struct, IFormattable, IComparable<T>, IConvertible, IEquatable<T>
        {
            if (box is null)
            {
                throw new ArgumentNullException(nameof(box));
            }

            if (box.MaxValue is null)
            {
                var formatAndCulture = PleaseEnterAValueGreaterThanOrEqualToFormatAndCulture.GetOrCreate(box.Culture);
                var message = formatAndCulture.FormatMessage(box.MinValue);
                return new IsLessThanValidationResult(
                    value: value,
                    min: box.MinValue,
                    max: null,
                    currentBoxCulture: box.Culture,
                    formatAndCulture: formatAndCulture,
                    isValid: false,
                    errorContent: message);
            }
            else
            {
                var formatAndCulture = PleaseEnterAValueBetweenFormatAndCulture.GetOrCreate(box.Culture);
                var message = formatAndCulture.FormatMessage(box.MinValue, box.MaxValue);
                return new IsLessThanValidationResult(
                    value: value,
                    min: box.MinValue,
                    max: box.MaxValue,
                    currentBoxCulture: box.Culture,
                    formatAndCulture: formatAndCulture,
                    isValid: false,
                    errorContent: message);
            }
        }
    }
}
