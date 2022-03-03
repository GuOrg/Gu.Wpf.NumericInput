namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal sealed class CanParse<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly CanParse<T> FromText = new(validatesOnTargetUpdated: true);
        internal static readonly CanParse<T> FromValue = new(validatesOnTargetUpdated: false);

        private CanParse(bool validatesOnTargetUpdated)
            : base(ValidationStep.RawProposedValue, validatesOnTargetUpdated)
        {
        }

        /// <inheritdoc/>
        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var box = (NumericBox<T>)((BindingExpression)owner).Target;
            if (box.TextSource == TextSource.None)
            {
                return ValidationResult.ValidResult;
            }

            var text = (string)value;
            if (string.IsNullOrWhiteSpace(text))
            {
                if (box.CanValueBeNull)
                {
                    return ValidationResult.ValidResult;
                }

                return RequiredButMissingValidationResult.CreateErrorResult(text, box);
            }

            if (box.CanParse(text))
            {
                return ValidationResult.ValidResult;
            }

            return CanParseValidationResult.CreateErrorResult(text, box);
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
