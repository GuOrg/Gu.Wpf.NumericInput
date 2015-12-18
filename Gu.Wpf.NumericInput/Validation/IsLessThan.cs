namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class IsLessThan<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable
    {
        private readonly Func<string, T> parser;
        private readonly Func<T?> maxGetter;

        public IsLessThan(Func<string, T> parser, Func<T?> maxGetter)
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

            if (v.CompareTo(max.Value) > 0)
            {
                return new IsLessThanValidationResult(v, max.Value, false, $"{v} > max ({max})");
            }

            return ValidationResult.ValidResult;
        }
    }
}