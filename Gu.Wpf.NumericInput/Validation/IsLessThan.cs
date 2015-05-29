namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class IsLessThan<T> : ValidationRule
        where T :struct, IComparable<T>, IFormattable
    {
        private readonly Func<string, T> _parser;
        private readonly Func<T?> _max;

        public IsLessThan(Func<string, T> parser, Func<T?> max)
        {
            _parser = parser;
            _max = max;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var max = _max();
            if (max == null)
            {
                return ValidationResult.ValidResult;
            }
            var v = _parser((string)value);

            if (v.CompareTo(max.Value) > 0)
            {
                return new IsLessThanValidationResult(v, max.Value, false, string.Format("{0} > max ({1})", v, max));
            }
            return ValidationResult.ValidResult;
        }
    }
}