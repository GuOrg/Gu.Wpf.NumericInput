namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(ValidationError), typeof(string))]
    public sealed class ValidationErrorToInfoStringConverter : IValueConverter
    {
        public static readonly ValidationErrorToInfoStringConverter Default = new ValidationErrorToInfoStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            if (value is ValidationError error)
            {
                var result = error.ErrorContent as ValidationResult;
                if (result != null)
                {
                    return $"{error.GetType().Name}.{result.GetType().Name} '{result.ErrorContent}'";
                }

                return $"{error.GetType().Name} '{error.ErrorContent}'";
            }

            return value.ToString();
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(ValidationErrorToInfoStringConverter)} can only be used in OneWay bindings");
        }
    }
}
