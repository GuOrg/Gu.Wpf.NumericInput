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
            var validationError = value as ValidationError;
            if (validationError != null)
            {
                var result = validationError.ErrorContent as ValidationResult;
                if (result != null)
                {
                    return $"{validationError.GetType().Name}.{result.GetType().Name} '{result.ErrorContent}'";
                }
                return $"{validationError.GetType().Name} '{validationError.ErrorContent}'";
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
