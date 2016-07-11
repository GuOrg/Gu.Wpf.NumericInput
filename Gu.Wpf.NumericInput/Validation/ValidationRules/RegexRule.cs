namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class IsMatch : ValidationRule
    {
        internal static readonly IsMatch FromText = new IsMatch(true);
        internal static readonly IsMatch FromValue = new IsMatch(false);

        private IsMatch(bool validatesOnTargetUpdated)
            : base(ValidationStep.RawProposedValue, validatesOnTargetUpdated)
        {
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            var box = (BaseBox)((BindingExpression)owner).Target;
            if (box.TextSource == TextSource.None)
            {
                return ValidationResult.ValidResult;
            }

            var text = (string)value;
            var pattern = box.RegexPattern;
            if (string.IsNullOrEmpty(pattern))
            {
                return ValidationResult.ValidResult;
            }

            if (string.IsNullOrEmpty(text))
            {
                var formatted = text == null ? "null" : "string.Empty";
                return new IsMatchValidationResult(text, pattern, box.Culture, false, $"{formatted} does not match pattern: {pattern}");
            }

            try
            {
                if (Regex.IsMatch(text, pattern))
                {
                    return ValidationResult.ValidResult;
                }

                return new IsMatchValidationResult(text, pattern, box.Culture, false, $"{text} does not match pattern: {pattern}");
            }
            catch (Exception e)
            {
                return new IsMatchValidationResult(text, pattern, box.Culture, false, $"{text} does not match pattern: {pattern}. Threw exception: {e.Message}");
            }
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            throw new InvalidOperationException("Should not get here");
        }
    }
}