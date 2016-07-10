namespace Gu.Wpf.NumericInput
{
    using System;

    public class IsGreaterThanValidationResult : NumericValidationResult
    {
        public static readonly OneParameterFormatAndCulture DefaultFormatAndCulture = OneParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_less_than_or_equal_to__0_));

        public IsGreaterThanValidationResult(
            IFormattable value,
            IFormattable min,
            IFormatProvider culture,
            OneParameterFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(culture, formatAndCulture, isValid, errorContent)
        {
            this.Value = value;
            this.Min = min;
        }

        /// <summary>Gets the current value at the time of validation.</summary>
        public IFormattable Value { get; }

        /// <summary>Gets the minimum allowed value at the time of validation.</summary>
        public IFormattable Min { get; }

        public static IsGreaterThanValidationResult CreateErrorResult<T>(T value, T min, IFormatProvider culture)
            where T : struct, IFormattable
        {
            var formatAndCulture = DefaultFormatAndCulture.GetOrCreate(culture);
            var message = formatAndCulture.FormatMessage(min);
            return new IsGreaterThanValidationResult(value, min, culture, formatAndCulture, false, message);
        }
    }
}