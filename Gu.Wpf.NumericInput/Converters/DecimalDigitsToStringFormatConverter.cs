namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(int?), typeof(string))]
    internal sealed class DecimalDigitsToStringFormatConverter : IValueConverter
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

            if (Cache.TryGetValue(i.Value, out string format))
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
            throw new NotSupportedException($"{this.GetType().Name} does not support use in bindings with Mode = TwoWay.");
        }
    }
}
