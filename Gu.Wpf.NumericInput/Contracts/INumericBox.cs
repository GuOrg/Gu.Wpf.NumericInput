namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public interface INumericBox
    {
        string Text { get; set; }

        IFormattable Value { get; }

        IFormattable MaxValue { get; }

        IFormattable MinValue { get; }

        IFormattable Increment { get; }

        /// <summary>
        /// Gets the culture for the control.
        /// The control has an explicit culture and does not use <see cref="System.Threading.Thread.CurrentUICulture"/>
        /// </summary>
        IFormatProvider Culture { get; }

        /// <summary>
        /// Gets the suffix is text placed after the value.
        /// </summary>
        string Suffix { get;  }

        /// <summary>
        /// Gets the stringformat. Changing <see cref="IDecimals.DecimalDigits"/> updates the value.
        /// Exposed for binding in template.
        /// </summary>
        string StringFormat { get; }

        /// <summary>
        /// Gets a value that indicates if spinbuttons should be visible
        /// </summary>
        bool AllowSpinners { get; }

        /// <summary>
        /// Gets the command that increases <see cref="Value"/> by <see cref="Increment"/>
        /// </summary>
        ICommand IncreaseCommand { get; }

        /// <summary>
        /// Gets the command the decreases <see cref="Value"/> by <see cref="Increment"/>
        /// </summary>
        ICommand DecreaseCommand { get; }

        IFormattable Parse(string s);

        bool CanParse(string text);

        void SetValue(DependencyProperty property, object value);

        object GetValue(DependencyProperty property);
    }
}