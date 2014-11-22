namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class IsLessThan<T> : ValidationRule
        where T : IComparable<T>, IFormattable
    {
        private readonly Func<string, T> _parser;
        private readonly Func<T> _max;

        public IsLessThan(Func<string, T> parser, Func<T> max)
        {
            _parser = parser;
            _max = max;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var v = _parser((string)value);
            var max = _max();
            if (v.CompareTo(max) > 0)
            {
                return new IsLessThanValidationResult(v, max, false, string.Format("{0} > max ({1})", v, max));
            }
            return ValidationResult.ValidResult;
        }
    }
}