namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class IsGreaterThanOrEqualToMinRule<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly IsGreaterThanOrEqualToMinRule<T> FromText = new IsGreaterThanOrEqualToMinRule<T>(true);
        internal static readonly IsGreaterThanOrEqualToMinRule<T> FromValue = new IsGreaterThanOrEqualToMinRule<T>(false);

        private IsGreaterThanOrEqualToMinRule(bool validatesOnTargetUpdated)
            : base(ValidationStep.ConvertedProposedValue, validatesOnTargetUpdated)
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var box = (NumericBox<T>)((BindingExpression)owner).ResolvedSource;
            if (box.TextSource == TextSource.None)
            {
                return ValidationResult.ValidResult;
            }

            if (box.MinValue == null)
            {
                return ValidationResult.ValidResult;
            }

            if (value == null)
            {
                return new IsGreaterThanValidationResult(null, box.MinValue, false, $"Value cannot be null when {nameof(NumericBox<T>.MinValue)} is set");
            }

            var min = box.MinValue.Value;
            var v = (T)value;
            if (v.CompareTo(min) < 0)
            {
                return new IsGreaterThanValidationResult(v, min, false, $"{v} < min ({min})");
            }

            return ValidationResult.ValidResult;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException("Should not get here");
        }
    }
}