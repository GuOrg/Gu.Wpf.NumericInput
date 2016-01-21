namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Controls;

    public class IsGreaterThanValidationResult : ValidationResult
    {
        public IsGreaterThanValidationResult(IFormattable value, IFormattable min, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            this.Value = value;
            this.Min = min;
        }

        public IFormattable Value { get; }

        public IFormattable Min { get; }
    }
}