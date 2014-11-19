namespace Gu.Wpf.NumericControls.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class IsLessThan<T> : ValidationRule
        where T : IComparable<T>
    {
        private readonly Func<string, IFormatProvider, T> _parser;
        private readonly Func<T> _max;

        public IsLessThan(Func<string, IFormatProvider, T> parser, Func<T> max)
        {
            _parser = parser;
            _max = max;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var v = _parser((string)value, cultureInfo);
            var max = _max();
            if (v.CompareTo(max) > 0)
            {
                return new ValidationResult(false, string.Format("{0} > max ({1})", v, max));
            }
            return ValidationResult.ValidResult;
        }
    }
}