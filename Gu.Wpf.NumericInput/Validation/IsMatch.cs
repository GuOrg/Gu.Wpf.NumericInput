namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;

    public class IsMatch : ValidationRule
    {
        private readonly Func<string> patternGetter;

        public IsMatch(Func<string> patternGetter)
        {
            this.patternGetter = patternGetter;
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var text = (string)value;
            var pattern = this.patternGetter();
            if (string.IsNullOrEmpty(pattern))
            {
                return ValidationResult.ValidResult;
            }

            if (!string.IsNullOrEmpty(text) && Regex.IsMatch(text, pattern))
            {
                return ValidationResult.ValidResult;
            }

            return new IsMatchValidationResult(text, pattern, false, $"{text} does not match pattern: {pattern}");
        }
    }
}
