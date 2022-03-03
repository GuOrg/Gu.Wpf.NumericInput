namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(ValidationError), typeof(string))]
    public sealed class ValidationErrorToInfoStringConverter : IValueConverter
    {
        public static readonly ValidationErrorToInfoStringConverter Default = new();

        public object? Convert(object? value, Type targetType, object parameter, CultureInfo culture)
        {
            return value switch
            {
                ValidationError { ErrorContent: ValidationResult result } error
                    => $"{error.GetType().Name}.{result.GetType().Name} '{result.ErrorContent}'",
                ValidationError error => $"{error.GetType().Name} '{error.ErrorContent}'",
                null => string.Empty,
                _ => value.ToString(),
            };
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(ValidationErrorToInfoStringConverter)} can only be used in OneWay bindings");
        }
    }
}
