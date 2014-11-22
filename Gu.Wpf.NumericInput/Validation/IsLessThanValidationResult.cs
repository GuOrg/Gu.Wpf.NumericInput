namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Windows.Controls;

    public class IsLessThanValidationResult : ValidationResult
    {
        public IsLessThanValidationResult(IFormattable value, IFormattable max, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            Value = value;
            Max = max;
        }

        public IFormattable Value { get; private set; }

        public IFormattable Max { get; private set; }
    }
}