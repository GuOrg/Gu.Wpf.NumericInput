namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using Gu.Wpf.NumericInput.Validation;

    internal class IsGreaterThanOrEqualToMinRule<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        private readonly Func<string, T?> parser;
        private readonly Func<T?> minGetter;

        public IsGreaterThanOrEqualToMinRule(Func<string, T?> parser, Func<T?> minGetter)
        {
            this.parser = parser;
            this.minGetter = minGetter;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var min = this.minGetter();
            if (min == null)
            {
                return ValidationResult.ValidResult;
            }

            var v = this.parser((string)value);
            if (v == null)
            {
                return new IsGreaterThanValidationResult(null, min.Value, false, $"Value cannot be null when {nameof(NumericBox<T>.MinValue)} is set");
            }

            if (v.Value.CompareTo(min.Value) < 0)
            {
                return new IsGreaterThanValidationResult(v, min.Value, false, $"{v} < min ({min})");
            }

            return ValidationResult.ValidResult;
        }
    }
}