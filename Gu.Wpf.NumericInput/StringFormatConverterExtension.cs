namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Markup;

    public class StringFormatConverterExtension : MarkupExtension, IValueConverter
    {
        private readonly string _format;
        public StringFormatConverterExtension(string format)
        {
            _format = format;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return string.Format(_format, value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("This will be tricky");
        }
    }
}
