namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [ValueConversion(typeof(object), typeof(Thickness))]
    internal sealed class FormattedTextBlockMarginConverter : IValueConverter
    {
        internal static readonly FormattedTextBlockMarginConverter Default = new FormattedTextBlockMarginConverter();

        private FormattedTextBlockMarginConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is ScrollContentPresenter { Content: FrameworkElement { Margin: { } textMargin }, Margin: { } presenterMargin })
            {
                return new Thickness(
                    presenterMargin.Left + textMargin.Left,
                    presenterMargin.Top + textMargin.Top,
                    presenterMargin.Right + textMargin.Right,
                    presenterMargin.Bottom + textMargin.Bottom);
            }

#if DEBUG
            throw new InvalidOperationException("Failed getting formatted text margin.");
#else
                return new Thickness(2, 0, 2, 0);
#endif
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException($"{nameof(FormattedTextBlockMarginConverter)} does not support use in bindings with Mode = TwoWay.");
        }
    }
}
