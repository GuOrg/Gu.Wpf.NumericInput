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
        /// <summary>Identifies the <see cref="SelectAllOnGotKeyboardFocus"/> dependency property.</summary>
        public static readonly DependencyProperty SelectAllOnGotKeyboardFocusProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnGotKeyboardFocus",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the <see cref="SelectAllOnClick"/> dependency property.</summary>
        public static readonly DependencyProperty SelectAllOnClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnClick",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the <see cref="SelectAllOnDoubleClick"/> dependency property.</summary>
        public static readonly DependencyProperty SelectAllOnDoubleClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnDoubleClick",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the <see cref="MoveFocusOnEnter"/> dependency property.</summary>
        public static readonly DependencyProperty MoveFocusOnEnterProperty = DependencyProperty.RegisterAttached(
            "MoveFocusOnEnter",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the <see cref="LoseFocusOnEnter"/> dependency property.</summary>
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

        public static void SetLoseFocusOnEnter(this DependencyObject element, bool value)
        {
            element.SetValue(LoseFocusOnEnterProperty, value);
        }

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
