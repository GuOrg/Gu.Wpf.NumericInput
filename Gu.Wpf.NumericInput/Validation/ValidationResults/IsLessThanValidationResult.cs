namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// This <see cref="ValidationResult"/> is returned when <see cref="Value"/> is not less than <see cref="Max"/>
    /// </summary>
    public class IsLessThanValidationResult : NumericValidationResult
    {
        public static readonly OneParameterFormatAndCulture DefaultFormatAndCulture = OneParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_greater_than_or_equal_to__0__));

        public IsLessThanValidationResult(
            IFormattable value,
            IFormattable max,
            IFormatProvider currentBoxCulture,
            OneParameterFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(currentBoxCulture, formatAndCulture, isValid, errorContent)
        {
            this.Value = value;
            this.Max = max;
        }

        /// <summary>Gets the current value.</summary>
        public IFormattable Value { get; }

        /// <summary>Gets the minimum allowed value.</summary>
        public IFormattable Max { get; }

        public static IsLessThanValidationResult CreateErrorResult<T>(T value, T min, IFormatProvider culture)
             where T : struct, IFormattable
        {
            var formatAndCulture = DefaultFormatAndCulture.GetOrCreate(culture);
            var message = formatAndCulture.FormatMessage(min);
            return new IsLessThanValidationResult(value, min, culture, formatAndCulture, false, message);
        }
    }
}