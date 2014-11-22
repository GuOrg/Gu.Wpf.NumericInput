namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Windows.Controls;

    public class CanParseValidationResult : ValidationResult
    {
        public CanParseValidationResult(Type type, string text, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            Type = type;
            Text = text;
        }
        public Type Type { get; private set; }
        public string Text { get; private set; }
    }
}