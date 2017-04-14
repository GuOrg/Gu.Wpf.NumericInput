namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when <see cref="NumericBox{T}.Value"/> is greater than <see cref="NumericBox{T}.MaxValue"/></summary>
    public class IsGreaterThanValidationResult : OutOfRangeValidationResult
    {
        public static readonly OneParameterFormatAndCulture PleaseEnterAValueLessThanOrEqualToFormatAndCulture = OneParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_less_than_or_equal_to__0__));

        public IsGreaterThanValidationResult(
            IFormattable value,
            IFormattable min,
            IFormattable max,
            IFormatProvider currentBoxCulture,
            IFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(value, min, max, currentBoxCulture, formatAndCulture, isValid, errorContent)
        {
        }

        public static IsGreaterThanValidationResult CreateErrorResult<T>(T value, NumericBox<T> box)
            where T : struct, IFormattable, IComparable<T>, IConvertible, IEquatable<T>
        {
            if (box.MinValue == null)
            {
                var formatAndCulture = PleaseEnterAValueLessThanOrEqualToFormatAndCulture.GetOrCreate(box.Culture);
                var message = formatAndCulture.FormatMessage(box.MaxValue);
                return new IsGreaterThanValidationResult(
                    value: value,
                    min: box.MinValue,
                    max: box.MaxValue,
                    currentBoxCulture: box.Culture,
                    formatAndCulture: formatAndCulture,
                    isValid: false,
                    errorContent: message);
            }
            else
            {
                var formatAndCulture = PleaseEnterAValueBetweenFormatAndCulture.GetOrCreate(box.Culture);
                var message = formatAndCulture.FormatMessage(box.MinValue, box.MaxValue);
                return new IsGreaterThanValidationResult(
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