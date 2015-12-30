namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using Gu.Wpf.NumericInput.Validation;

    internal class IsLessThanOrEqualToMaxRule<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        private readonly Func<string, T?> parser;
        private readonly Func<T?> maxGetter;

        public IsLessThanOrEqualToMaxRule(Func<string, T?> parser, Func<T?> maxGetter)
        {
            this.parser = parser;
            this.maxGetter = maxGetter;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var max = this.maxGetter();
            if (max == null)
            {
                return ValidationResult.ValidResult;
            }

            var v = this.parser((string)value);
            if (v == null)
            {
                return new IsLessThanValidationResult(null, max.Value, false, $"Value cannot be null when {nameof(NumericBox<T>.MaxValue)} is set");
            }

            if (v.Value.CompareTo(max.Value) > 0)
            {
                return new IsLessThanValidationResult(v, max.Value, false, $"{v} > max ({max})");
            }

            return ValidationResult.ValidResult;
        }
    }
}