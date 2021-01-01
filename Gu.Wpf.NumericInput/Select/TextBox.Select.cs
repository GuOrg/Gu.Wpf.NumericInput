#pragma warning disable CA1716 // Identifiers should not match keywords
namespace Gu.Wpf.NumericInput.Select
#pragma warning restore CA1716 // Identifiers should not match keywords
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    /// <summary>
    /// Attached properties for <see cref="System.Windows.Controls.TextBox"/>.
    /// </summary>
    public static class TextBox
    {
        /// <summary>Identifies the SelectAllOnGotKeyboardFocus attached dependency property.</summary>
        public static readonly DependencyProperty SelectAllOnGotKeyboardFocusProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnGotKeyboardFocus",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the SelectAllOnClick attached dependency property.</summary>
        public static readonly DependencyProperty SelectAllOnClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnClick",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the SelectAllOnDoubleClick attached dependency property.</summary>
        public static readonly DependencyProperty SelectAllOnDoubleClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnDoubleClick",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the MoveFocusOnEnter attached dependency property.</summary>
        public static readonly DependencyProperty MoveFocusOnEnterProperty = DependencyProperty.RegisterAttached(
            "MoveFocusOnEnter",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>Identifies the LoseFocusOnEnter attached dependency property.</summary>
        public static readonly DependencyProperty LoseFocusOnEnterProperty = DependencyProperty.RegisterAttached(
            "LoseFocusOnEnter",
            typeof(bool),
            typeof(TextBox),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits));

        static TextBox()
        {
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.KeyDownEvent, new KeyEventHandler(OnKeyDown));
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.PreviewMouseLeftButtonDownEvent, new RoutedEventHandler(OnPreviewMouseLeftButtonDown), handledEventsToo: true);
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.GotKeyboardFocusEvent, new RoutedEventHandler(OnGotKeyboardFocus));
            EventManager.RegisterClassHandler(typeof(TextBoxBase), UIElement.MouseLeftButtonDownEvent, new RoutedEventHandler(OnMouseLeftButtonDown), handledEventsToo: true);
            EventManager.RegisterClassHandler(typeof(TextBoxBase), Control.MouseDoubleClickEvent, new RoutedEventHandler(OnMouseDoubleClick));
        }

        /// <summary>Helper for setting <see cref="SelectAllOnGotKeyboardFocusProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="SelectAllOnGotKeyboardFocusProperty"/> on.</param>
        /// <param name="value">SelectAllOnGotKeyboardFocus property value.</param>
        public static void SetSelectAllOnGotKeyboardFocus(this UIElement element, bool value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(SelectAllOnGotKeyboardFocusProperty, BooleanBoxes.Box(value));
        }

        /// <summary>Helper for getting <see cref="SelectAllOnGotKeyboardFocusProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="SelectAllOnGotKeyboardFocusProperty"/> from.</param>
        /// <returns>SelectAllOnGotKeyboardFocus property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnGotKeyboardFocus(this UIElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(SelectAllOnGotKeyboardFocusProperty);
        }

        /// <summary>Helper for setting <see cref="SelectAllOnClickProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="SelectAllOnClickProperty"/> on.</param>
        /// <param name="value">SelectAllOnClick property value.</param>
        public static void SetSelectAllOnClick(this UIElement element, bool value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(SelectAllOnClickProperty, BooleanBoxes.Box(value));
        }

        /// <summary>Helper for getting <see cref="SelectAllOnClickProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="SelectAllOnClickProperty"/> from.</param>
        /// <returns>SelectAllOnClick property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnClick(this UIElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(SelectAllOnClickProperty);
        }

        /// <summary>Helper for setting <see cref="SelectAllOnDoubleClickProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="SelectAllOnDoubleClickProperty"/> on.</param>
        /// <param name="value">SelectAllOnDoubleClick property value.</param>
        public static void SetSelectAllOnDoubleClick(this UIElement element, bool value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(SelectAllOnDoubleClickProperty, BooleanBoxes.Box(value));
        }

        /// <summary>Helper for getting <see cref="SelectAllOnDoubleClickProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="SelectAllOnDoubleClickProperty"/> from.</param>
        /// <returns>SelectAllOnDoubleClick property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetSelectAllOnDoubleClick(this UIElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(SelectAllOnDoubleClickProperty);
        }

        /// <summary>Helper for setting <see cref="MoveFocusOnEnterProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="MoveFocusOnEnterProperty"/> on.</param>
        /// <param name="value">MoveFocusOnEnter property value.</param>
        public static void SetMoveFocusOnEnter(this UIElement element, bool value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(MoveFocusOnEnterProperty, BooleanBoxes.Box(value));
        }

        /// <summary>Helper for getting <see cref="MoveFocusOnEnterProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="MoveFocusOnEnterProperty"/> from.</param>
        /// <returns>MoveFocusOnEnter property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetMoveFocusOnEnter(this UIElement element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(MoveFocusOnEnterProperty);
        }

        /// <summary>Helper for setting <see cref="LoseFocusOnEnterProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="LoseFocusOnEnterProperty"/> on.</param>
        /// <param name="value">LoseFocusOnEnter property value.</param>
        public static void SetLoseFocusOnEnter(this DependencyObject element, bool value)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            element.SetValue(LoseFocusOnEnterProperty, value);
        }

        /// <summary>Helper for getting <see cref="LoseFocusOnEnterProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="LoseFocusOnEnterProperty"/> from.</param>
        /// <returns>LoseFocusOnEnter property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetLoseFocusOnEnter(this DependencyObject element)
        {
            if (element is null)
            {
                throw new ArgumentNullException(nameof(element));
            }

            return (bool)element.GetValue(LoseFocusOnEnterProperty);
        }

        private static void OnKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Return || e.Key == Key.Enter) &&
                sender is TextBoxBase textBox)
            {
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

        private static void OnMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase textBoxBase &&
                textBoxBase.GetSelectAllOnClick())
            {
                textBoxBase.SelectAll();
            }
        }

        private static void OnMouseDoubleClick(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase textBoxBase &&
                textBoxBase.GetSelectAllOnDoubleClick())
            {
                textBoxBase.SelectAll();
            }
        }

        private static void OnGotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase textBoxBase &&
                HasText() &&
                textBoxBase.GetSelectAllOnGotKeyboardFocus())
            {
                textBoxBase.SelectAll();
            }

            bool HasText()
            {
                if (sender is System.Windows.Controls.TextBox textBox)
                {
                    return !string.IsNullOrEmpty(textBox.Text);
                }

                return true;
            }
        }

        private static void OnPreviewMouseLeftButtonDown(object sender, RoutedEventArgs e)
        {
            if (sender is TextBoxBase { IsKeyboardFocusWithin: false, IsEnabled: true, Focusable: true } textBoxBase &&
                textBoxBase.GetSelectAllOnGotKeyboardFocus() &&
                HasText() &&
                textBoxBase.Focus())
            {
                e.Handled = true;
            }

            bool HasText()
            {
                if (sender is System.Windows.Controls.TextBox textBox)
                {
                    return !string.IsNullOrEmpty(textBox.Text);
                }

                return true;
            }
        }
    }
}
