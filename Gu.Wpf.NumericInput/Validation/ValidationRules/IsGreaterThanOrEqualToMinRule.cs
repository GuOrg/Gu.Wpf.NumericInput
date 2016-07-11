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

        public override ValidationResult Validate(object o, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var box = (NumericBox<T>)((BindingExpression)owner).Target;
            if (box.TextSource == TextSource.None)
            {
                return ValidationResult.ValidResult;
            }

            if (box.MinValue == null || o == null)
            {
                return ValidationResult.ValidResult;
            }

            var min = box.MinValue.Value;
            var value = (T)o;
            if (value.CompareTo(min) < 0)
            {
                return IsLessThanValidationResult.CreateErrorResult(value, box);
            }

            return ValidationResult.ValidResult;
        }

        public override ValidationResult Validate(object o, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException("Should not get here");
        }
    }
}