namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when <see cref="NumericBox{T}.Value"/> is less than <see cref="NumericBox{T}.MinValue"/></summary>
    public class IsLessThanValidationResult : OutOfRangeValidationResult
    {
        public static readonly OneParameterFormatAndCulture PleaseEnterAValueGreaterThanOrEqualToFormatAndCulture = OneParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_greater_than_or_equal_to__0__));

        public IsLessThanValidationResult(
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

        public static IsLessThanValidationResult CreateErrorResult<T>(T value, NumericBox<T> box)
             where T : struct, IFormattable, IComparable<T>, IConvertible, IEquatable<T>
        {
            if (box.MaxValue == null)
            {
                var formatAndCulture = PleaseEnterAValueGreaterThanOrEqualToFormatAndCulture.GetOrCreate(box.Culture);
                var message = formatAndCulture.FormatMessage(box.MinValue);
                return new IsLessThanValidationResult(value, box.MinValue, box.MaxValue, box.Culture, formatAndCulture, false, message);
            }
            else
            {
                var formatAndCulture = PleaseEnterAValueBetweenFormatAndCulture.GetOrCreate(box.Culture);
                var message = formatAndCulture.FormatMessage(box.MinValue, box.MaxValue);
                return new IsLessThanValidationResult(value, box.MinValue, box.MaxValue, box.Culture, formatAndCulture, false, message);
            }
        }
    }
}