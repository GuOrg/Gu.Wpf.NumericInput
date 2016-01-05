namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    internal class StringConverter<T> : IValueConverter
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly StringConverter<T> Default = new StringConverter<T>();

        private StringConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("This is meant for OneWayToSource only");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;
            if (string.IsNullOrWhiteSpace(text))
            {
                return null;
            }

            var box = (NumericBox<T>)parameter;
            T result;
            if (box.TryParse(text, out result))
            {
                return result;
            }

            return Binding.DoNothing;
        }
    }
}