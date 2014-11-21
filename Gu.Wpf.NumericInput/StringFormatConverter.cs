namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    public class StringFormatConverter : IValueConverter
    {
        private readonly WeakReference<INumericBox> _weakReference = new WeakReference<INumericBox>(null);
        public StringFormatConverter(INumericBox box)
        {
            _weakReference.SetTarget(box);
        }
        
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return FormattedText;
        }
        
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Value;
        }

        private string FormattedText
        {
            get
            {
                INumericBox box;
                if (_weakReference.TryGetTarget(out box))
                {
                    return box.FormattedText;
                }
                return "";
            }
        }

        private object Value
        {
            get
            {
                INumericBox box;
                if (_weakReference.TryGetTarget(out box))
                {
                    return box.Parse(box.Text);
                }
                return null;
            }
        }
    }
}