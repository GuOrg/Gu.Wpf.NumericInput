namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(ValidationError), typeof(string))]
    public sealed class ErrorContentToStringConverter : IValueConverter
    {
        public static readonly ErrorContentToStringConverter Default = new ErrorContentToStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ValidationError error)
            {
                var result = error.ErrorContent as ValidationResult;
                if (result != null)
                {
                    return result.ErrorContent?.ToString();
                }

                return error.ErrorContent?.ToString();
            }

            return value?.ToString();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{typeof(ErrorContentToStringConverter).Name} does not support use in bindings with Mode = TwoWay.");
        }
    }
}
