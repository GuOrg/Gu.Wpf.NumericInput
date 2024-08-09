namespace Gu.Wpf.NumericInput.Touch
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    /// <summary>
    /// Attached properties for <see cref="System.Windows.Controls.TextBox"/>.
    /// </summary>
    public static class TextBox
    {
        /// <summary>Identifies the ShowTouchKeyboardOnTouchEnter attached dependency property.</summary>
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
            Application.Current.Dispatcher.Invoke(() =>
            {
                var mainWindow = Application.Current?.MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.Closed += OnClosed;
                }
            });
        }

        /// <summary>Helper for setting <see cref="ShowTouchKeyboardOnTouchEnterProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="ShowTouchKeyboardOnTouchEnterProperty"/> on.</param>
        /// <param name="value">ShowTouchKeyboardOnTouchEnter property value.</param>
        public static void SetShowTouchKeyboardOnTouchEnter(UIElement element, bool value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(ShowTouchKeyboardOnTouchEnterProperty, value);
        }

        /// <summary>Helper for getting <see cref="ShowTouchKeyboardOnTouchEnterProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="ShowTouchKeyboardOnTouchEnterProperty"/> from.</param>
        /// <returns>ShowTouchKeyboardOnTouchEnter property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetShowTouchKeyboardOnTouchEnter(UIElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(ShowTouchKeyboardOnTouchEnterProperty);
        }

        private static void OnTouchEnter(object? sender, RoutedEventArgs e)
        {
            if (sender is System.Windows.Controls.TextBox textBox &&
                GetShowTouchKeyboardOnTouchEnter(textBox))
            {
                TouchKeyboard.Show();
            }
        }

        private static void OnLostFocus(object? sender, RoutedEventArgs e)
        {
            if (!IsShowTouch(Keyboard.FocusedElement) &&
                IsShowTouch(sender))
            {
                TouchKeyboard.Hide();
            }

            static bool IsShowTouch(object? o)
            {
                return o is System.Windows.Controls.TextBox focused &&
                       GetShowTouchKeyboardOnTouchEnter(focused);
            }
        }

        private static void OnClosed(object? sender, EventArgs eventArgs)
        {
            if (sender is Window window)
            {
                window.Closed -= OnClosed;
                TouchKeyboard.Hide();
            }
        }
    }
}
