namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class CanParse<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly CanParse<T> OnTextChanged = new CanParse<T>(true);
        internal static readonly CanParse<T> OnValueChanged = new CanParse<T>(false);

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
            if (box.CanParse(text))
            {
                return ValidationResult.ValidResult;
            }

            return new CanParseValidationResult(typeof(T), text, false, $"Cannot parse '{text}' to a {typeof(T).Name}");
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException("Should not get here");
        }
    }
}
