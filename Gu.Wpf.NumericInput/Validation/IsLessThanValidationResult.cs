namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Windows.Controls;

    /// <summary>
    /// This <see cref="ValidationResult"/> is returned when <see cref="Value"/> is not less than <see cref="Max"/>
    /// </summary>
    public class IsLessThanValidationResult : ValidationResult
    {
        public IsLessThanValidationResult(IFormattable value, IFormattable max, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            this.Value = value;
            this.Max = max;
        }

        public IFormattable Value { get; }

        public IFormattable Max { get; }
    }
}