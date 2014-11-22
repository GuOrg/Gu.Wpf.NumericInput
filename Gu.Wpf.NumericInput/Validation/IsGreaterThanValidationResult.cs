namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Windows.Controls;

    public class IsGreaterThanValidationResult : ValidationResult
    {
        public IsGreaterThanValidationResult(IFormattable value, IFormattable min, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            Value = value;
            Min = min;
        }

        public IFormattable Value { get; private set; }
        
        public IFormattable Min { get; private set; }
    }
}