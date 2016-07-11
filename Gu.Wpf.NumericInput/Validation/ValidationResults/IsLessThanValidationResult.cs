namespace Gu.Wpf.NumericInput
{
    using System;

    public class IsLessThanValidationResult : OutOfRangeValidationResult
    {
        public static readonly OneParameterFormatAndCulture DefaultFormatAndCulture = OneParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_greater_than_or_equal_to__0__));

        public IsLessThanValidationResult(
            IFormattable value,
            IFormattable min,
            IFormattable max,
            IFormatProvider currentBoxCulture,
            OneParameterFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(value, min, max, currentBoxCulture, formatAndCulture, isValid, errorContent)
        {
        }

        public static IsLessThanValidationResult CreateErrorResult<T>(T value, NumericBox<T> box)
             where T : struct, IFormattable, IComparable<T>, IConvertible, IEquatable<T>
        {
            var formatAndCulture = DefaultFormatAndCulture.GetOrCreate(box.Culture);
            var message = formatAndCulture.FormatMessage(box.MinValue);
            return new IsLessThanValidationResult(value, box.MinValue, box.MaxValue, box.Culture, formatAndCulture, false, message);
        }
    }
}