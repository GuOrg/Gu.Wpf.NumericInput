namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;
    using System.Windows.Media;

    public static partial class NumericBox
    {
        public static readonly DependencyProperty SelectAllOnClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnClick",
            typeof(bool),
            typeof(NumericBox),
            new PropertyMetadata(
                BooleanBoxes.False,
                OnSelectAllOnClickChanged));

        public static readonly DependencyProperty SelectAllOnDoubleClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnDoubleClick",
            typeof(bool),
            typeof(NumericBox),
            new PropertyMetadata(
                BooleanBoxes.False,
                OnSelectAllOnDoubleClickChanged));

        public static readonly DependencyProperty CultureProperty = DependencyProperty.RegisterAttached(
            "Culture",
            typeof(IFormatProvider),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(
                CultureInfo.GetCultureInfo("en-US"), // Think this is the default in WPF
                FrameworkPropertyMetadataOptions.Inherits,
                OnCultureChanged));

        private static readonly string GotKeyboardFocusEventName = "GotKeyboardFocus";
        private static readonly string PreviewMouseLeftButtonDownEventName = "PreviewMouseLeftButtonDown";
        private static readonly string MouseDoubleClickEventName = "MouseDoubleClick";

        [AttachedPropertyBrowsableForChildrenAttribute(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBoxBase))]
        public static bool GetSelectAllOnClick(this TextBoxBase o)
        {
            return (bool)o.GetValue(SelectAllOnClickProperty);
        }

        public static void SetSelectAllOnClick(this TextBoxBase o, bool value)
        {
            o.SetValue(SelectAllOnClickProperty, value);
        }

        public static void SetSelectAllOnDoubleClick(this TextBoxBase element, bool value)
        {
            element.SetValue(SelectAllOnDoubleClickProperty, value);
        }

        public static bool GetSelectAllOnDoubleClick(this TextBoxBase element)
        {
            return (bool)element.GetValue(SelectAllOnDoubleClickProperty);
        }

        public static void SetCulture(this FrameworkElement element, CultureInfo value)
        {
            element.SetValue(CultureProperty, value);
        }

        public static CultureInfo GetCulture(this FrameworkElement element)
        {
            return (CultureInfo)element.GetValue(CultureProperty);
        }

        private static void OnSelectAllOnClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as TextBoxBase;
            if (box != null)
            {
                var isSelecting = Equals(e.NewValue, BooleanBoxes.True);
                if (isSelecting)
                {
                    WeakEventManager<TextBoxBase, KeyboardFocusChangedEventArgs>.AddHandler(box, GotKeyboardFocusEventName, OnKeyboardFocusSelectText);
                    WeakEventManager<TextBoxBase, MouseButtonEventArgs>.AddHandler(box, PreviewMouseLeftButtonDownEventName, OnMouseLeftButtonDown);
                }
                else
                {
                    WeakEventManager<TextBoxBase, KeyboardFocusChangedEventArgs>.RemoveHandler(box, GotKeyboardFocusEventName, OnKeyboardFocusSelectText);
                    WeakEventManager<TextBoxBase, MouseButtonEventArgs>.RemoveHandler(box, PreviewMouseLeftButtonDownEventName, OnMouseLeftButtonDown);
                }
            }
        }

        private static void OnSelectAllOnDoubleClickChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as TextBoxBase;
            if (box != null)
            {
                var isSelecting = Equals(e.NewValue, BooleanBoxes.True);
                if (isSelecting)
                {
                    WeakEventManager<TextBoxBase, MouseButtonEventArgs>.AddHandler(box, MouseDoubleClickEventName, OnMouseDoubleClick);
                }
                else
                {
                    WeakEventManager<TextBoxBase, MouseButtonEventArgs>.RemoveHandler(box, MouseDoubleClickEventName, OnMouseDoubleClick);
                }
            }
        }

        private static void OnMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var textBoxBase = GetParentFromVisualTree(e.OriginalSource);
            textBoxBase?.SelectAll();
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var textBoxBase = GetParentFromVisualTree(e.OriginalSource);
            if (textBoxBase == null)
            {
                return;
            }

            var box = textBoxBase;
            if (!box.IsKeyboardFocusWithin)
            {
                box.Focus();
                e.Handled = true;
            }
        }

        private static TextBoxBase GetParentFromVisualTree(object source)
        {
            DependencyObject parent = source as UIElement;
            while (parent != null && !(parent is TextBoxBase))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as TextBoxBase;
        }

        private static void OnKeyboardFocusSelectText(object sender, KeyboardFocusChangedEventArgs e)
        {
            var box = e.OriginalSource as TextBox;
            box?.SelectAll();
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var baseBox = d as BaseBox;
            if (baseBox != null)
            {
                baseBox.Culture = (IFormatProvider)e.NewValue;
            }
        }
    }
}
