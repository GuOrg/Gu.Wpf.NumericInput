namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;

    internal class DecimalDigitsToStringFormatConverter : IValueConverter
    {
        internal static readonly DecimalDigitsToStringFormatConverter Default = new DecimalDigitsToStringFormatConverter();
        private static readonly Dictionary<int, string> Cache = new Dictionary<int, string>();

        private DecimalDigitsToStringFormatConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var i = value as int?;
            if (i == null)
            {
                return null;
            }

            string format;
            if (Cache.TryGetValue(i.Value, out format))
            {
                return format;
            }

            if (i >= 0)
            {
                format = "F" + i.Value;
                Cache[i.Value] = format;
            }
            else
            {
                format = "0." + new string('#', -i.Value);
                Cache[i.Value] = format;
            }

            return format;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
