namespace Gu.Wpf.NumericInput.Tests
{
    using System.Windows;

    public static class UiTestHelper
    {
        public static void Initialize<T>(this T element)
            where T : FrameworkElement
        {
            element.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            element.Arrange(new Rect(element.DesiredSize));
            element.UpdateLayout();
        }

        public static void Invalidate<T>(T element)
            where T : FrameworkElement
        {
            element.InvalidateVisual();
        }

        public static Window ShowInWindow<T>(this T element)
            where T : FrameworkElement
        {
            var window = new Window
            {
                Content = element,
            };
            window.Show();
            return window;
        }

        public static Window ShowInWindow<T>(this T element, object dataContext)
            where T : FrameworkElement
        {
            var window = new Window()
            {
                Content = element,
            };
            window.DataContext = dataContext;
            window.Show();
            return window;
        }
    }
}
