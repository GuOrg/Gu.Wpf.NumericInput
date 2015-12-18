namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;

    internal class CanParse<T> : ValidationRule
    {
        private readonly Func<string, bool> tryParser;

        public CanParse(Func<string, bool> tryParser)
        {
            this.tryParser = tryParser;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var s = (string)value;
            if (this.tryParser(s))
            {
                return ValidationResult.ValidResult;
            }

            return new CanParseValidationResult(typeof(T), s, false, $"Cannot parse '{s}' to a {typeof(T).Name}");
        }
    }
}
