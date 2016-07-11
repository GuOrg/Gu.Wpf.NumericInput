namespace Gu.Wpf.NumericInput
{
    using System;

    public class IsGreaterThanValidationResult : OutOfRangeValidationResult
    {
        private static readonly OneParameterFormatAndCulture PleaseEnterAValueLessThanOrEqualToFormatAndCulture = OneParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_less_than_or_equal_to__0_));

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
                return new IsGreaterThanValidationResult(value, box.MinValue, box.MaxValue, box.Culture, formatAndCulture, false, message);
            }
            else
            {
                var formatAndCulture = PleaseEnterAValueBetweenFormatAndCulture.GetOrCreate(box.Culture);
                var message = formatAndCulture.FormatMessage(box.MinValue, box.MaxValue);
                return new IsGreaterThanValidationResult(value, box.MinValue, box.MaxValue, box.Culture, formatAndCulture, false, message);
            }
        }
    }
}