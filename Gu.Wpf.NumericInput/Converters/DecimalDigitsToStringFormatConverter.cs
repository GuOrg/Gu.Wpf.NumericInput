namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(int?), typeof(string))]
    internal sealed class DecimalDigitsToStringFormatConverter : IValueConverter
    {
        internal static readonly DecimalDigitsToStringFormatConverter Default = new();
        private static readonly Dictionary<int, string> Cache = new();

        private DecimalDigitsToStringFormatConverter()
        {
        }

        public object? Convert(object value, Type? targetType, object? parameter, CultureInfo? culture)
        {
            if (value is int i)
            {
                if (Cache.TryGetValue(i, out string? format))
                {
                    return format;
                }

                if (i >= 0)
                {
                    format = "F" + i;
                    Cache[i] = format;
                }
                else
                {
                    format = "0." + new string('#', -i);
                    Cache[i] = format;
                }

                return format;
            }

            return null;
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(DecimalDigitsToStringFormatConverter)} does not support use in bindings with Mode = TwoWay.");
        }
    }
}
