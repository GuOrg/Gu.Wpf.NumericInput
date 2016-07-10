namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Controls;

    public class IsMatchValidationResult : ValidationResult
    {
        public IsMatchValidationResult(string text, string pattern, IFormatProvider culture, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            this.Text = text;
            this.Pattern = pattern;
            this.Culture = culture;
        }

        /// <summary>Gets the text that was found invalid.</summary>
        public string Text { get; }

        /// <summary>Gets the regex pattern that was used for validation.</summary>
        public string Pattern { get; }

        /// <summary>Gets the culture of the numeric box.</summary>
        public IFormatProvider Culture { get; set; }
    }
}