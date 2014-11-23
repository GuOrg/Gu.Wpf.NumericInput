namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows.Data;

    using Gu.Wpf.NumericInput.Validation;

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

        public string FormattedText
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

        public object Value
        {
            get
            {
                INumericBox box;
                if (_weakReference.TryGetTarget(out box))
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

            //INumericBox box;
            //    if (_weakReference.TryGetTarget(out box))
            //    {
            //        return box.Value;
            //        //var baseBox = box as BaseBox;

            //        //if (baseBox != null)
            //        //{
            //        //    var textProxy = ExplicitBinding.GetTextProxy(baseBox);
            //        //    return box.Parse(textProxy);
            //        //    if (!System.Windows.Controls.Validation.GetHasError(baseBox))
            //        //    {
            //        //        return box.Value;
            //        //    }
            //        //}
            //        //return box.Parse(box.Text);
            //        //return box.Value;


            //        //if (!System.Windows.Controls.Validation.GetHasError(baseBox))
            //        //{
            //        //    return box.Value;
            //        //}
            //        //var textProxy = ExplicitBinding.GetTextProxy(baseBox);
            //        //return box.CanParse(textProxy) ? box.Parse(textProxy) : null;
            //    }
            //    return null;
            //}
        }
    }
}

