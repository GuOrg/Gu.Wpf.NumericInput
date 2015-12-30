namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Controls;

    public class CanParseValidationResult : ValidationResult
    {
        public CanParseValidationResult(Type type, string text, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            this.Type = type;
            this.Text = text;
        }

        public Type Type { get; }

        public string Text { get; }
    }
}