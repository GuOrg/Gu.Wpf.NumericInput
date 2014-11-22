namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;

    public class IsMatch : ValidationRule
    {
        private readonly Func<string> _pattern;

        public IsMatch(Func<string> pattern)
        {
            this._pattern = pattern;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var s = (string)value;
            var pattern = _pattern();
            if (string.IsNullOrEmpty(pattern) || Regex.IsMatch(s, pattern))
            {
                return ValidationResult.ValidResult;
            }
            return new IsMatchValidationResult(s, pattern, false, string.Format("{0} does not match pattern: {1}", s, pattern));
        }
    }
}
