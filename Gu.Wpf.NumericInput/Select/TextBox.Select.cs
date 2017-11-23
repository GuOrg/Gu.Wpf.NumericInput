namespace Gu.Wpf.NumericInput.Select
{
    using System;
    using System.Linq;
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

        public static readonly DependencyProperty LoseFocusOnEnterProperty = DependencyProperty.RegisterAttached(
            "LoseFocusOnEnter",
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
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.KeyDownEvent, new KeyEventHandler(OnKeyDown));
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.MouseUpEvent, new RoutedEventHandler(OnMouseUpSelectAllText), handledEventsToo: true);
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(OnGotKeyboardFocusSelectAllText));
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler(OnMouseClickSelectAllText), handledEventsToo: true);
            EventManager.RegisterClassHandler(typeof(TextBoxBase), Control.MouseDoubleClickEvent, new RoutedEventHandler(OnMouseDoubleClickSelectAllText));
        }

        /// <summary>
        /// Helper for setting SelectAllOnGotKeyboardFocus property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set SelectAllOnGotKeyboardFocus property on.</param>
        /// <param name="value">SelectAllOnGotKeyboardFocus property value.</param>
        public static void SetSelectAllOnGotKeyboardFocus(this UIElement element, bool value)
        {
            element.SetValue(SelectAllOnGotKeyboardFocusProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Helper for reading SelectAllOnGotKeyboardFocus property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read SelectAllOnGotKeyboardFocus property from.</param>
        /// <returns>SelectAllOnGotKeyboardFocus property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnGotKeyboardFocus(this UIElement element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(SelectAllOnGotKeyboardFocusProperty));
        }

        /// <summary>
        /// Helper for setting SelectAllOnClick property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set SelectAllOnClick property on.</param>
        /// <param name="value">SelectAllOnClick property value.</param>
        public static void SetSelectAllOnClick(this UIElement element, bool value)
        {
            element.SetValue(SelectAllOnClickProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Helper for reading SelectAllOnClick property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read SelectAllOnClick property from.</param>
        /// <returns>SelectAllOnClick property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnClick(this UIElement element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(SelectAllOnClickProperty));
        }

        /// <summary>
        /// Helper for setting SelectAllOnDoubleClick property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set SelectAllOnDoubleClick property on.</param>
        /// <param name="value">SelectAllOnDoubleClick property value.</param>
        public static void SetSelectAllOnDoubleClick(this UIElement element, bool value)
        {
            element.SetValue(SelectAllOnDoubleClickProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Helper for reading SelectAllOnDoubleClick property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read SelectAllOnDoubleClick property from.</param>
        /// <returns>SelectAllOnDoubleClick property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnDoubleClick(this UIElement element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(SelectAllOnDoubleClickProperty));
        }

        /// <summary>
        /// Helper for setting MoveFocusOnEnter property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set MoveFocusOnEnter property on.</param>
        /// <param name="value">MoveFocusOnEnter property value.</param>
        public static void SetMoveFocusOnEnter(this UIElement element, bool value)
        {
            element.SetValue(MoveFocusOnEnterProperty, BooleanBoxes.Box(value));
        }

        /// <summary>
        /// Helper for reading MoveFocusOnEnter property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read MoveFocusOnEnter property from.</param>
        /// <returns>MoveFocusOnEnter property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetMoveFocusOnEnter(this UIElement element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(MoveFocusOnEnterProperty));
        }

        /// <summary>
        /// Helper for setting LoseFocusOnEnter property on a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to set LoseFocusOnEnter property on.</param>
        /// <param name="value">LoseFocusOnEnter property value.</param>
        public static void SetLoseFocusOnEnter(this DependencyObject element, bool value)
        {
            element.SetValue(LoseFocusOnEnterProperty, value);
        }

        /// <summary>
        /// Helper for reading LoseFocusOnEnter property from a DependencyObject.
        /// </summary>
        /// <param name="element">DependencyObject to read LoseFocusOnEnter property from.</param>
        /// <returns>LoseFocusOnEnter property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetLoseFocusOnEnter(this DependencyObject element)
        {
            return (bool)element.GetValue(LoseFocusOnEnterProperty);
        }

        private static void SetIsSelecting(this DependencyObject element, bool value)
        {
            element.SetValue(IsSelectingPropertyKey, BooleanBoxes.Box(value));
        }

        private static bool GetIsSelecting(this DependencyObject element)
        {
            return Equals(BooleanBoxes.True, element.GetValue(IsSelectingProperty));
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
            {
                var textBox = sender as TextBoxBase;
                if (textBox == null)
                {
                    return;
                }

                if (textBox.GetMoveFocusOnEnter())
                {
                    // MoveFocus takes a TraversalRequest as its argument.
                    var request = new TraversalRequest(FocusNavigationDirection.Next);

                    // Gets the element with keyboard focus.
                    // Change keyboard focus.
                    if (Keyboard.FocusedElement is UIElement elementWithFocus)
                    {
                        if (elementWithFocus.MoveFocus(request))
                        {
                            e.Handled = true;
                        }
                    }
                }

                if (textBox.IsFocused && textBox.GetLoseFocusOnEnter())
                {
                    var focusableAncestor = textBox.Ancestors()
                        .OfType<IInputElement>()
                        .FirstOrDefault(x => x.Focusable);
                    var scope = FocusManager.GetFocusScope(textBox);
                    FocusManager.SetFocusedElement(scope, focusableAncestor);
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

            textBoxBase.SetIsSelecting(value: true);
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
            if (textBoxBase is System.Windows.Controls.TextBox textBox)
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
