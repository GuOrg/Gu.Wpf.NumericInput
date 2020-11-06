namespace Gu.Wpf.NumericInput
{
    using System;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when <see cref="Value"/> in the valid range of <see cref="Max"/> and <see cref="Min"/>.</summary>
    public abstract class OutOfRangeValidationResult : NumericValidationResult
    {
        /// <summary>Message when user typed in too small or too large value.</summary>
        public static readonly TwoParameterFormatAndCulture PleaseEnterAValueBetweenFormatAndCulture = TwoParameterFormatAndCulture.CreateDefault(nameof(Properties.Resources.Please_enter_a_value_between__0__and__1__));

        /// <summary>
        /// Initializes a new instance of the <see cref="OutOfRangeValidationResult"/> class.
        /// </summary>
        /// <param name="value">The <see cref="IFormattable"/>.</param>
        /// <param name="min">The min <see cref="IFormattable"/>.</param>
        /// <param name="max">The max <see cref="IFormattable"/>.</param>
        /// <param name="currentBoxCulture">The <see cref="IFormatProvider"/>.</param>
        /// <param name="formatAndCulture">The <see cref="IFormatAndCulture"/>.</param>
        /// <param name="isValid">The <see cref="bool"/>.</param>
        /// <param name="errorContent">The <see cref="object"/>.</param>
        protected OutOfRangeValidationResult(
            IFormattable value,
            IFormattable? min,
            IFormattable? max,
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
        public IFormattable? Min { get; }

        /// <summary>Gets the maximum allowed value, can be null if no upper limit..</summary>
        public IFormattable? Max { get; }
    }
}
