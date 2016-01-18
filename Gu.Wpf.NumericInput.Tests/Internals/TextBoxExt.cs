namespace Gu.Wpf.NumericInput.Tests.Internals
{
    using System.Windows;
    using System.Windows.Controls;

    public static class TextBoxExt
    {
        public static void RaiseLostFocus(this TextBox box)
        {
            box.RaiseEvent(new RoutedEventArgs(UIElement.LostFocusEvent));
        }
    }
}
