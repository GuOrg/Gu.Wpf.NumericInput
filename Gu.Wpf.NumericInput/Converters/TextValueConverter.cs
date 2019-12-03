namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    [ValueConversion(typeof(string), typeof(IFormattable))]
    internal sealed class TextValueConverter<T> : IValueConverter
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly TextValueConverter<T> Default = new TextValueConverter<T>();

        private TextValueConverter()
        {
        }

        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var box = (NumericBox<T>)parameter;
            T result = default;
            if (box?.TryParse(text, out result) == true)
            {
                return result;
            }

            return Binding.DoNothing;
        }

        public object? ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var box = (NumericBox<T>)parameter;
            return box?.ToRawText(value as T?);
        }
    }
}
