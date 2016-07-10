namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class CanParse<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly CanParse<T> FromText = new CanParse<T>(true);
        internal static readonly CanParse<T> FromValue = new CanParse<T>(false);

        private CanParse(bool validatesOnTargetUpdated)
            : base(ValidationStep.RawProposedValue, validatesOnTargetUpdated)
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var box = (NumericBox<T>)((BindingExpression)owner).Target;
            if (box.TextSource == TextSource.None)
            {
                return ValidationResult.ValidResult;
            }

            var text = (string)value;
            if (string.IsNullOrWhiteSpace(text) && box.CanValueBeNull)
            {
                return ValidationResult.ValidResult;
            }

            if (box.CanParse(text))
            {
                return ValidationResult.ValidResult;
            }

            return CanParseValidationResult.CreateErrorResult(typeof(T), text, box.Culture);
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException("Should not get here");
        }
    }
}
