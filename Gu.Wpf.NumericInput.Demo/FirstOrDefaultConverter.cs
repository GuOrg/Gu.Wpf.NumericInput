namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    public class FirstOrDefaultConverter : IValueConverter
    {
        public static readonly FirstOrDefaultConverter Default = new FirstOrDefaultConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumerable = value as IEnumerable;
            return enumerable?.Cast<object>()
                              .FirstOrDefault();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
