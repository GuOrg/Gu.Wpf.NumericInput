namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class IsGreaterThanValidationResult : ValidationResult
    {
        public IsGreaterThanValidationResult(IFormattable value, IFormattable min, IFormatProvider culture, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            this.Value = value;
            this.Min = min;
            this.Culture = culture;
        }

        /// <summary>Gets the current value.</summary>
        public IFormattable Value { get; }

        /// <summary>Gets the minimum allowed value.</summary>
        public IFormattable Min { get; }

        /// <summary>Gets the culture of the numeric box.</summary>
        public IFormatProvider Culture { get; }

        public static IsGreaterThanValidationResult CreateErrorResult<T>(T value, T min, IFormatProvider culture)
            where T : struct, IFormattable
        {
            var format = Properties.Resources.ResourceManager.GetString(
                            nameof(Properties.Resources.Please_enter_a_value_less_than_or_equal_to__0_),
                            culture as CultureInfo);
            var message = format != null
                              ? string.Format(culture, format, min)
                              : string.Format(CultureInfo.InvariantCulture, Properties.Resources.Please_enter_a_value_less_than_or_equal_to__0_, min);
            return new IsGreaterThanValidationResult(value, min, culture, false, message);
        }

        public override string ToString() => this.ErrorContent.ToString();
    }
}