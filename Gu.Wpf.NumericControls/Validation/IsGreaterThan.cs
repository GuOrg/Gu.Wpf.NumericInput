namespace Gu.Wpf.NumericControls.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class IsGreaterThan<T> : ValidationRule
        where T : IComparable<T>
    {
        private readonly Func<string, IFormatProvider, T> _parser;
        private readonly Func<T> _min;
        
        public IsGreaterThan(Func<string, IFormatProvider, T> parser, Func<T> min)
        {
            _parser = parser;
            _min = min;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var v = _parser((string)value, cultureInfo);
            var min = _min();
            if (v.CompareTo(min) < 0)
            {
                return new ValidationResult(false, string.Format("{0} < min ({1})", v, min));
            }
            return ValidationResult.ValidResult;
        }
    }
}