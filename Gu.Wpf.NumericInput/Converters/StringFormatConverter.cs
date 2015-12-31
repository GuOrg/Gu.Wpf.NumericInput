namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Gu.Wpf.NumericInput.Validation;

    internal class StringFormatConverter : IValueConverter
    {
        private readonly WeakReference<INumericBox> weakReference = new WeakReference<INumericBox>(null);

        public StringFormatConverter(INumericBox box)
        {
            this.weakReference.SetTarget(box);
        }

        public string FormattedText
        {
            get
            {
                INumericBox box;
                if (this.weakReference.TryGetTarget(out box))
                {
                    return box.Value?.ToString(box.StringFormat, box.Culture) ?? string.Empty;
                }

                return string.Empty;
            }
        }

        private object Value
        {
            get
            {
                INumericBox box;
                if (this.weakReference.TryGetTarget(out box))
                {
                    var text = box.Text;
                    var textProxy = ExplicitBinding.GetTextProxy(box);
                    if (!box.CanParse(text))
                    {
                        return null;
                    }

                    if (textProxy.HasMoreDecimalDigitsThan(text, box))
                    {
                        return box.Parse(textProxy);
                    }

                    return box.Parse(text);
                }

                return null;
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.FormattedText;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return this.Value;
        }
    }
}