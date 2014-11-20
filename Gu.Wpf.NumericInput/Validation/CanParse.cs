namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class CanParse<T> : ValidationRule
    {
        private readonly Func<string, IFormatProvider, bool> _tryParser;
        public CanParse(Func<string,  IFormatProvider, bool> tryParser)
        {
            this._tryParser = tryParser;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var s = (string)value;
            if (this._tryParser(s, cultureInfo))
            {
                return ValidationResult.ValidResult;
            }
            return new ValidationResult(false, string.Format("Cannot parse '{0}' to a {1}", s, typeof(T).Name));
        }
    }
}
