namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Controls;

    public abstract class NumericValidationResult : ValidationResult
    {
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

        /// <summary>Gets the current culture used in the <see cref="NumericBox{T}"/></summary>
        public IFormatProvider CurrentCulture { get; }

        /// <summary>Gets the culture and format used when formatting the <see cref="ValidationResult.ErrorContent"/>.</summary>
        public IFormatAndCulture FormatAndCulture { get; }

        public override string ToString() => this.ErrorContent.ToString();
    }
}