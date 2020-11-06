namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Controls;

    /// <summary>This <see cref="System.Windows.Controls.ValidationResult"/> is returned when <see cref="NumericBox{T}.Value"/> is less than <see cref="NumericBox{T}.MinValue"/>.</summary>
    public abstract class NumericValidationResult : ValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericValidationResult"/> class.
        /// </summary>
        /// <param name="currentCulture">The <see cref="IFormatProvider"/>.</param>
        /// <param name="formatAndCulture">The <see cref="IFormatAndCulture"/>.</param>
        /// <param name="isValid">The <see cref="bool"/>.</param>
        /// <param name="errorContent">The <see cref="object"/>.</param>
        protected NumericValidationResult(
            IFormatProvider currentCulture,
            IFormatAndCulture formatAndCulture,
            bool isValid,
            object errorContent)
            : base(isValid, errorContent)
        {
            this.CurrentCulture = currentCulture;
            this.FormatAndCulture = formatAndCulture;
        }

        /// <summary>Gets the current culture used in the <see cref="NumericBox{T}"/>.</summary>
        public IFormatProvider CurrentCulture { get; }

        /// <summary>Gets the culture and format used when formatting the <see cref="ValidationResult.ErrorContent"/>.</summary>
        public IFormatAndCulture FormatAndCulture { get; }

        /// <inheritdoc/>
        public override string? ToString() => this.ErrorContent.ToString();
    }
}
