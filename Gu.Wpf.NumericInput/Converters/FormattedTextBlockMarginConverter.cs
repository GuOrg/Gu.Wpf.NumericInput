namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class FormattedTextBlockMarginConverter : IValueConverter
    {
        internal static readonly FormattedTextBlockMarginConverter Default = new FormattedTextBlockMarginConverter();

        private FormattedTextBlockMarginConverter()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var presenter = (ScrollContentPresenter)parameter;
            var presenterMargin = presenter?.Margin;
            var textMargin = ((FrameworkElement)presenter?.Content)?.Margin;
            if (presenterMargin == null || textMargin == null)
            {
#if DEBUG
                throw new InvalidOperationException("Failed getting formatted text margin.");
#else
                return new Thickness(2, 0, 2, 0);
#endif
            }

            var result = new Thickness(
                presenterMargin.Value.Left + textMargin.Value.Left,
                presenterMargin.Value.Top + textMargin.Value.Top,
                presenterMargin.Value.Right + textMargin.Value.Right,
                presenterMargin.Value.Bottom + textMargin.Value.Bottom);
            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException("Only for one way bindings");
        }
    }
}