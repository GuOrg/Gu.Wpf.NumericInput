namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when <see cref="Value"/> in the valid range of <see cref="Max"/> and <see cref="Min"/></summary>
    public abstract class OutOfRangeValidationResult : NumericValidationResult
    {
        public static readonly TwoParameterFormatAndCulture PleaseEnterAValueBetweenFormatAndCulture = TwoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_between__0__and__1__));

        protected OutOfRangeValidationResult(
            IFormattable value,
            IFormattable min,
            IFormattable max,
            IFormatProvider currentBoxCulture,
            IFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(currentBoxCulture, formatAndCulture, isValid, errorContent)
        {
            this.Value = value;
            this.Min = min;
            this.Max = max;
        }

        /// <summary>Gets the current value at the time of validation.</summary>
        public IFormattable Value { get; }

        /// <summary>Gets the maximum allowed value, can be null if no lower limit.</summary>
        public IFormattable Min { get; }

        /// <summary>Gets the maximum allowed value, can be null if no upper limit..</summary>
        public IFormattable Max { get; }
    }
}