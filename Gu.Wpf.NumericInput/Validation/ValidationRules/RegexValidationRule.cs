namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal sealed class RegexValidationRule : ValidationRule
    {
        internal static readonly RegexValidationRule FromText = new(validatesOnTargetUpdated: true);
        internal static readonly RegexValidationRule FromValue = new(validatesOnTargetUpdated: false);

        private RegexValidationRule(bool validatesOnTargetUpdated)
            : base(ValidationStep.RawProposedValue, validatesOnTargetUpdated)
        {
        }

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var box = (BaseBox)((BindingExpression)owner).Target;
            if (box.TextSource == TextSource.None)
            {
                return ValidationResult.ValidResult;
            }

            var text = (string)value;
            var pattern = box.RegexPattern;
            if (string.IsNullOrEmpty(pattern))
            {
                return ValidationResult.ValidResult;
            }

            if (string.IsNullOrEmpty(text))
            {
                return RegexValidationResult.CreateErrorResult(text, box);
            }

            try
            {
                if (Regex.IsMatch(text, pattern))
                {
                    return ValidationResult.ValidResult;
                }

                return RegexValidationResult.CreateErrorResult(text, box);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                return RegexValidationResult.CreateMalformedPatternErrorResult(text, e, box);
            }
        }

        /// <summary> This should never be called.</summary>
        /// <param name="value">_.</param>
        /// <param name="cultureInfo">__.</param>
        /// <returns>___.</returns>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException("Should not get here");
        }
    }
}
