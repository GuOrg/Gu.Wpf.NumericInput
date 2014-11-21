namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Input;

    public interface INumericBox
    {
        string Text { get; set; }

        IFormattable Value { get; }

        IFormattable MaxValue { get; }

        IFormattable MinValue { get; }

        IFormattable Increment { get; }

        int? Decimals { get; }
        
        IFormatProvider Culture { get; }

        string Suffix { get;  }
        
        /// <summary>
        /// The stringformat is set using the Decimals property, exposed for binding in template.
        /// </summary>
        string StringFormat { get; }

        bool AllowSpinners { get; }
        
        /// <summary>
        /// Increases the value by increment
        /// </summary>
        ICommand IncreaseCommand { get; }
        
        /// <summary>
        /// Decreases the value by increment
        /// </summary>
        ICommand DecreaseCommand { get; }
        
        string FormattedText { get; }

        IFormattable Parse(string s);
    }
}