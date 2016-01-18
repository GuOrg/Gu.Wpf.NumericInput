namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    internal class VisibleWhenTrueConverter : IValueConverter
    {
        internal static readonly VisibleWhenTrueConverter Default = new VisibleWhenTrueConverter();

        private VisibleWhenTrueConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only for one way bindings");
        }
    }
}