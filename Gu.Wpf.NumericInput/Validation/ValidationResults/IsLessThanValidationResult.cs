namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    /// <summary>
    /// This <see cref="ValidationResult"/> is returned when <see cref="Value"/> is not less than <see cref="Max"/>
    /// </summary>
    public class IsLessThanValidationResult : ValidationResult
    {
        public IsLessThanValidationResult(IFormattable value, IFormattable max, IFormatProvider culture, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            this.Value = value;
            this.Max = max;
            this.Culture = culture;
        }

        /// <summary>Gets the current value.</summary>
        public IFormattable Value { get; }

        /// <summary>Gets the minimum allowed value.</summary>
        public IFormattable Max { get; }

        /// <summary>Gets the culture of the numeric box.</summary>
        public IFormatProvider Culture { get; }

        public static IsLessThanValidationResult CreateErrorResult<T>(T value, T min, IFormatProvider culture)
             where T : struct, IFormattable
        {
            var format = Properties.Resources.ResourceManager.GetString(
                            nameof(Properties.Resources.Please_enter_a_value_greater_than_or_equal_to__0__),
                            culture as CultureInfo);
            var message = format != null
                              ? string.Format(culture, format, min)
                              : string.Format(CultureInfo.InvariantCulture, Properties.Resources.Please_enter_a_value_greater_than_or_equal_to__0__, min);
            return new IsLessThanValidationResult(value, min, culture, false, message);
        }

        public override string ToString() => this.ErrorContent.ToString();
    }
}