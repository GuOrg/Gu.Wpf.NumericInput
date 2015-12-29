namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    public interface IBoxVm : INotifyPropertyChanged
    {
        Type Type { get; }
        bool Configurable { get; }

        CultureInfo[] Cultures { get; }
        CultureInfo Culture { get; set; }
        IFormattable Min { get; set; }
        IFormattable Max { get; set; }
        IFormattable Value { get; set; }
        int? DecimalDigits { get; set; }
        bool AllowSpinners { get; set; }
        bool IsReadonly { get; set; }
        string Suffix { get; set; }
        string RegexPattern { get; set; }
        IFormattable Increment { get; set; }
    }
}