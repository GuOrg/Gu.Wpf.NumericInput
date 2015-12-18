namespace Gu.Wpf.NumericInput.Validation
{
    using System.Windows.Controls;

    public class IsMatchValidationResult : ValidationResult
    {
        public IsMatchValidationResult(string text, string pattern, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            this.Text = text;
            this.Pattern = pattern;
        }

        public string Text { get; }

        public string Pattern { get; }
    }
}