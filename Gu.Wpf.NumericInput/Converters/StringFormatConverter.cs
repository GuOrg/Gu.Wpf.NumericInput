namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Gu.Wpf.NumericInput.Validation;

    internal class StringFormatConverter<T> : IValueConverter
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly StringFormatConverter<T> Default = new StringFormatConverter<T>();

        private StringFormatConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.GetFormattedText((NumericBox<T>)parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.GetValue((NumericBox<T>)parameter);
        }

        internal string GetFormattedText(NumericBox<T> box)
        {
            return box.Value?.ToString(box.StringFormat, box.Culture) ?? string.Empty;
        }

        private object GetValue(NumericBox<T> box)
        {
            var text = box.Text;
            var textProxy = box.GetTextProxy();
            if (!box.CanParse(text))
            {
                return null;
            }

            if (textProxy.HasMoreDecimalDigitsThan(text, box as DecimalDigitsBox<T>))
            {
                return box.Parse(textProxy);
            }

            return box.Parse(text);
        }
    }
}