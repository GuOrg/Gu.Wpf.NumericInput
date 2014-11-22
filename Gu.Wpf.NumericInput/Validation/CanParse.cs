namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    public class CanParse<T> : ValidationRule
    {
        private readonly Func<string, bool> _tryParser;
        public CanParse(Func<string, bool> tryParser)
        {
            _tryParser = tryParser;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var s = (string)value;
            if (_tryParser(s))
            {
                return ValidationResult.ValidResult;
            }
            return new CanParseValidationResult(typeof(T), s, false, string.Format("Cannot parse '{0}' to a {1}", s, typeof(T).Name));
        }
    }
}
