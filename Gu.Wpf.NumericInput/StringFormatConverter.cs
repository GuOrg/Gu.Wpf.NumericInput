namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class StringFormatConverter : IMultiValueConverter
    {
        private readonly WeakReference<INumericBox> _weakReference = new WeakReference<INumericBox>(null);
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            var numericBox = (INumericBox)values[0];
            _weakReference.SetTarget(numericBox);
            return FormattedText;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            INumericBox box;
            if (_weakReference.TryGetTarget(out box))
            {
                return new[] { box, value, box.StringFormat, box.Culture };
            }
            return null;
        }

        private string FormattedText
        {
            get
            {
                INumericBox box;
                if (_weakReference.TryGetTarget(out box))
                {
                    return box.Value.ToString(box.StringFormat, box.Culture);
                }
                return "";
            }
        }
    }
}