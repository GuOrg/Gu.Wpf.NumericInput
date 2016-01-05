namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    public class StatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var baseBox = (BaseBox)values[0];
            return baseBox.Status.ToString();
        }

        public object[] ConvertBack(object value, Type[] targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
