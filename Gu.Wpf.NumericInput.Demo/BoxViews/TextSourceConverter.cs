namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class TextSourceConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var baseBox = (BaseBox)values[0];
            return baseBox.TextSource.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}