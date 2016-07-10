namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Controls;

    public class CanParseValidationResult : ValidationResult
    {
        public CanParseValidationResult(Type type, string text, IFormatProvider culture, bool isValid, object errorContent)
            : base(isValid, errorContent)
        {
            this.Type = type;
            this.Text = text;
            this.Culture = culture;
        }

        /// <summary>Gets the type of the box, i.e. <see cref="double"/> for a <see cref="DoubleBox"/>.</summary>
        public Type Type { get; }

        /// <summary>Gets the text that was found invalid.</summary>
        public string Text { get; }

        /// <summary>Gets the culture of the numeric box.</summary>
        public IFormatProvider Culture { get;  }
    }
}