namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// For binding validation errors.
    /// </summary>
    [ValueConversion(typeof(ValidationError), typeof(string))]
    public sealed class ErrorContentToStringConverter : IValueConverter
    {
        /// <summary>The default instance.</summary>
        public static readonly ErrorContentToStringConverter Default = new ErrorContentToStringConverter();

        /// <inheritdoc/>
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ValidationError error)
            {
                if (error.ErrorContent is ValidationResult result)
                {
                    return result.ErrorContent?.ToString();
                }

                return error.ErrorContent?.ToString();
            }

            return value?.ToString();
        }

        /// <inheritdoc/>
        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{typeof(ErrorContentToStringConverter).Name} does not support use in bindings with Mode = TwoWay.");
        }
    }
}
