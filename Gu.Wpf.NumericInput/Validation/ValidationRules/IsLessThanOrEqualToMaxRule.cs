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

        public override ValidationResult Validate(object o, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var box = (NumericBox<T>)((BindingExpression)owner).Target;
            if (box.TextSource == TextSource.None)
            {
                return ValidationResult.ValidResult;
            }

            if (box.MaxValue == null || o == null)
            {
                return ValidationResult.ValidResult;
            }

            var max = box.MaxValue.Value;
            var value = (T)o;
            if (value.CompareTo(max) > 0)
            {
                return IsGreaterThanValidationResult.CreateErrorResult(value, max, box.Culture);
            }

            return ValidationResult.ValidResult;
        }

        public override ValidationResult Validate(object o, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException("Should not get here");
        }
    }
}