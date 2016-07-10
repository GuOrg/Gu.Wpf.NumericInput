namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ValidationErrorToInfoStringConverter : IValueConverter
    {
        public static readonly ValidationErrorToInfoStringConverter Default = new ValidationErrorToInfoStringConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }

            var error = value as ValidationError;
            if (error != null)
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
