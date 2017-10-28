namespace Gu.Wpf.NumericInput.Touch
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    public static class TextBox
    {
        /// <summary>Identifies the <see cref="ShowTouchKeyboardOnTouchEnter"/> dependency property.</summary>
        public static readonly DependencyProperty ShowTouchKeyboardOnTouchEnterProperty = DependencyProperty.RegisterAttached(
            "ShowTouchKeyboardOnTouchEnter",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                default(bool),
                FrameworkPropertyMetadataOptions.Inherits));

        static TextBox()
        {
            EventManager.RegisterClassHandler(
                typeof(System.Windows.Controls.TextBox),
                UIElement.TouchEnterEvent,
                new RoutedEventHandler(OnTouchEnter));

            EventManager.RegisterClassHandler(
                typeof(System.Windows.Controls.TextBox),
                UIElement.LostFocusEvent,
                new RoutedEventHandler(OnLostFocus));
            var mainWindow = Application.Current?.MainWindow;
            if (mainWindow != null)
            {
                mainWindow.Closed += OnClosed;
            }
        }

        public static void SetShowTouchKeyboardOnTouchEnter(UIElement element, bool value)
        {
            element.SetValue(ShowTouchKeyboardOnTouchEnterProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetShowTouchKeyboardOnTouchEnter(UIElement element)
        {
            return (bool)element.GetValue(ShowTouchKeyboardOnTouchEnterProperty);
        }

        private static void OnTouchEnter(object sender, RoutedEventArgs e)
        {
            var textBox = (System.Windows.Controls.TextBox)sender;
            if (GetShowTouchKeyboardOnTouchEnter(textBox))
            {
                TouchKeyboard.Show();
            }
        }

        private static void OnLostFocus(object sender, RoutedEventArgs e)
        {
            var textBox = Keyboard.FocusedElement as System.Windows.Controls.TextBox;
            if (textBox == null || !GetShowTouchKeyboardOnTouchEnter(textBox))
            {
                TouchKeyboard.Hide();
            }
        }

        private static void OnClosed(object sender, EventArgs eventArgs)
        {
            ((Window)sender).Closed -= OnClosed;
            TouchKeyboard.Hide();
        }
    }
}