namespace Gu.Wpf.NumericInput.Demo
{
    using System;
    using System.Collections;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class FirstOrDefaultErrorConverter : IValueConverter
    {
        public static readonly FirstOrDefaultErrorConverter Default = new FirstOrDefaultErrorConverter();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var enumerable = value as IEnumerable;
            var firstOrDefault = enumerable?.Cast<object>()
                                            .FirstOrDefault();
            if (firstOrDefault == null)
            {
                return "";
            }

            var validationError = firstOrDefault as ValidationError;
            if (validationError != null)
            {
                var result = validationError.ErrorContent as ValidationResult;
                if (result != null)
                {
                    return $"{validationError.GetType().Name}.{result.GetType().Name} '{result.ErrorContent}'";
                }
                return $"{validationError.GetType().Name} '{validationError.ErrorContent}'";
            }

            return firstOrDefault;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
