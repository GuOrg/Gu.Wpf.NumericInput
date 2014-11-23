namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public interface IDecimals
    {
        int? DecimalDigits { get; set; }
    }
    public interface INumericBox
    {
        string Text { get; set; }

        IFormattable Value { get; }

        IFormattable MaxValue { get; }

        IFormattable MinValue { get; }

        IFormattable Increment { get; }

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

        IFormattable Parse(string s);

        bool CanParse(string s);

        void SetValue(DependencyProperty property, object value);

        object GetValue(DependencyProperty property);
    }
}