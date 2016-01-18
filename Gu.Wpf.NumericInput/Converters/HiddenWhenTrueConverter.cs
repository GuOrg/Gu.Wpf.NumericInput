namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    internal class HiddenWhenTrueConverter : IValueConverter
    {
        internal static readonly HiddenWhenTrueConverter Default = new HiddenWhenTrueConverter();

        private HiddenWhenTrueConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? Visibility.Hidden : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only for one way bindings");
        }
    }
}