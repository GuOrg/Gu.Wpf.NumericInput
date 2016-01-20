namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class IsLessThanOrEqualToMaxRule<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly IsLessThanOrEqualToMaxRule<T> FromText = new IsLessThanOrEqualToMaxRule<T>(true);
        internal static readonly IsLessThanOrEqualToMaxRule<T> FromValue = new IsLessThanOrEqualToMaxRule<T>(false);

        private IsLessThanOrEqualToMaxRule(bool validatesOnTargetUpdated)
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

            if (box.MaxValue == null)
            {
                return ValidationResult.ValidResult;
            }

            if (value == null)
            {
                return new IsLessThanValidationResult(null, box.MaxValue, false, $"Value cannot be null when {nameof(NumericBox<T>.MaxValue)} is set");
            }

            var max = box.MaxValue.Value;
            var v = (T)value;
            if (v.CompareTo(max) > 0)
            {
                return new IsLessThanValidationResult(v, max, false, $"{v} > max ({max})");
            }

            return ValidationResult.ValidResult;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException("Should not get here");
        }
    }
}