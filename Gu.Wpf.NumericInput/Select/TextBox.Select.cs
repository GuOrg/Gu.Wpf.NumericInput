namespace Gu.Wpf.NumericInput.Select
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Threading;

    public static class TextBox
    {
        public static readonly DependencyProperty SelectAllOnGotKeyboardFocusProperty = DependencyProperty.RegisterAttached(
                "SelectAllOnGotKeyboardFocus",
                typeof(bool),
                typeof(TextBox),
                new FrameworkPropertyMetadata(
                    BooleanBoxes.False,
                    FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty SelectAllOnClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnClick",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty SelectAllOnDoubleClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnDoubleClick",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty MoveFocusOnEnterProperty = DependencyProperty.RegisterAttached(
            "MoveFocusOnEnter",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        private static readonly DependencyPropertyKey IsSelectingPropertyKey = DependencyProperty.RegisterAttachedReadOnly(
            "IsSelecting",
            typeof(bool),
            typeof(TextBox),
            new PropertyMetadata(BooleanBoxes.False));

        private static readonly DependencyProperty IsSelectingProperty = IsSelectingPropertyKey.DependencyProperty;

        static TextBox()
        {
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.KeyDownEvent, new KeyEventHandler(OnKeyDownMoveFocusOnEnter));
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.MouseUpEvent, new RoutedEventHandler(OnMouseUpSelectAllText), true);
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(OnGotKeyboardFocusSelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler(OnMouseClickSelectAllText), true);
            EventManager.RegisterClassHandler(typeof(TextBoxBase), Control.MouseDoubleClickEvent, new RoutedEventHandler(OnMouseDoubleClickSelectAllText));
        }

        public static void SetSelectAllOnGotKeyboardFocus(this UIElement element, bool value)
        {
            element.SetValue(SelectAllOnGotKeyboardFocusProperty, BooleanBoxes.Box(value));
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnGotKeyboardFocus(this UIElement element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(SelectAllOnGotKeyboardFocusProperty));
        }

        public static void SetSelectAllOnClick(this UIElement o, bool value)
        {
            o.SetValue(SelectAllOnClickProperty, BooleanBoxes.Box(value));
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnClick(this UIElement o)
        {
            return Equals(BooleanBoxes.True, o.GetValue(SelectAllOnClickProperty));
        }

        public static void SetSelectAllOnDoubleClick(this UIElement element, bool value)
        {
            element.SetValue(SelectAllOnDoubleClickProperty, BooleanBoxes.Box(value));
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnDoubleClick(this UIElement element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(SelectAllOnDoubleClickProperty));
        }

        public static void SetMoveFocusOnEnter(this UIElement element, bool value)
        {
            element.SetValue(MoveFocusOnEnterProperty, BooleanBoxes.Box(value));
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetMoveFocusOnEnter(this UIElement element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(MoveFocusOnEnterProperty));
        }

        private static void SetIsSelecting(this DependencyObject element, bool value)
        {
            element.SetValue(IsSelectingPropertyKey, BooleanBoxes.Box(value));
        }

        private static bool GetIsSelecting(this DependencyObject element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(IsSelectingProperty));
        }

        private static void OnKeyDownMoveFocusOnEnter(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                if ((sender as TextBoxBase)?.GetMoveFocusOnEnter() != true)
                {
                    return;
                }

                // MoveFocus takes a TraversalRequest as its argument.
                var request = new TraversalRequest(FocusNavigationDirection.Next);

                // Gets the element with keyboard focus.
                var elementWithFocus = Keyboard.FocusedElement as UIElement;

                // Change keyboard focus.
                if (elementWithFocus != null)
                {
                    if (elementWithFocus.MoveFocus(request))
                    {
                        e.Handled = true;
                    }
                }
            }
        }

        private static void OnMouseClickSelectAllText(object sender, RoutedEventArgs e)
        {
            var textBoxBase = (TextBoxBase)sender;
            if (!textBoxBase.GetSelectAllOnClick())
            {
                return;
            }

            textBoxBase.SetIsSelecting(true);
            textBoxBase.SelectAllText();
        }

        private static void OnMouseDoubleClickSelectAllText(object sender, RoutedEventArgs e)
        {
            var textBoxBase = (TextBoxBase)sender;
            if (!textBoxBase.GetSelectAllOnDoubleClick())
            {
                return;
            }

            textBoxBase.SetIsSelecting(true);
            textBoxBase.SelectAllText();
        }

        private static void OnGotKeyboardFocusSelectAllText(object sender, RoutedEventArgs e)
        {
            var textBoxBase = (TextBoxBase)sender;
            if (!textBoxBase.GetSelectAllOnGotKeyboardFocus())
            {
                return;
            }

            if (Mouse.LeftButton == MouseButtonState.Pressed ||
                Mouse.RightButton == MouseButtonState.Pressed)
            {
                textBoxBase.SelectAllText();
                textBoxBase.SetIsSelecting(true);
            }
            else
            {
                textBoxBase.SelectAllText();
            }
        }

        private static void OnMouseUpSelectAllText(object sender, RoutedEventArgs e)
        {
            var textBoxBase = (TextBoxBase)sender;
            if (textBoxBase.GetIsSelecting())
            {
                textBoxBase.SetIsSelecting(false);
                textBoxBase.SelectAllText();
                textBoxBase.Dispatcher.BeginInvoke(DispatcherPriority.DataBind, new Action(() => textBoxBase.SelectAllText()));
            }
        }

        private static void SelectAllText(this TextBoxBase textBoxBase)
        {
            var textBox = textBoxBase as System.Windows.Controls.TextBox;
            if (textBox != null)
            {
                if (textBox.SelectedText == textBox.Text)
                {
                    return;
                }

                textBox.SelectAll();
                return;
            }

            if (!textBoxBase.IsSelectionActive)
            {
                textBoxBase.SelectAll();
            }
        }
    }
}
