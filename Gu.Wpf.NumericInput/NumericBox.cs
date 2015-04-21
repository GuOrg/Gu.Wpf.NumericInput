namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;

    public static class NumericBox
    {
        public static readonly DependencyProperty SelectAllOnClickProperty = DependencyProperty.RegisterAttached(
            "SelectAllOnClick",
            typeof(bool),
            typeof(NumericBox),
            new PropertyMetadata(false, ActivePropertyChanged));

        public static readonly DependencyProperty CultureProperty = DependencyProperty.RegisterAttached(
            "Culture",
            typeof(IFormatProvider),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(CultureInfo.InvariantCulture, FrameworkPropertyMetadataOptions.Inherits, OnCultureChanged));

        private static readonly string GotKeyboardFocusEventName = "GotKeyboardFocus";
        private static readonly string PreviewMouseLeftButtonDownEventName = "PreviewMouseLeftButtonDown";

        [AttachedPropertyBrowsableForChildrenAttribute(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(TextBox))]
        public static bool GetSelectAllOnClick(this DependencyObject o)
        {
            return (bool)o.GetValue(SelectAllOnClickProperty);
        }

        public static void SetSelectAllOnClick(this DependencyObject o, bool value)
        {
            o.SetValue(SelectAllOnClickProperty, value);
        }

        public static void SetCulture(this BaseBox element, CultureInfo value)
        {
            element.SetValue(CultureProperty, value);
        }

        public static CultureInfo GetCulture(this BaseBox element)
        {
            return (CultureInfo)element.GetValue(CultureProperty);
        }

        private static void ActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = d as TextBox;
            if (box != null)
            {
                var isSelecting = (e.NewValue as bool?).GetValueOrDefault(false);
                if (isSelecting)
                {
                    WeakEventManager<TextBox, KeyboardFocusChangedEventArgs>.AddHandler(box, GotKeyboardFocusEventName, OnKeyboardFocusSelectText);
                    WeakEventManager<TextBox, MouseButtonEventArgs>.AddHandler(box, PreviewMouseLeftButtonDownEventName, OnMouseLeftButtonDown);
                }
                else
                {
                    WeakEventManager<TextBox, KeyboardFocusChangedEventArgs>.RemoveHandler(box, GotKeyboardFocusEventName, OnKeyboardFocusSelectText);
                    WeakEventManager<TextBox, MouseButtonEventArgs>.RemoveHandler(box, PreviewMouseLeftButtonDownEventName, OnMouseLeftButtonDown);
                }
            }
        }

        private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DependencyObject dependencyObject = GetParentFromVisualTree(e.OriginalSource);

            if (dependencyObject == null)
            {
                return;
            }

            var box = (TextBox)dependencyObject;
            if (!box.IsKeyboardFocusWithin)
            {
                box.Focus();
                e.Handled = true;
            }
        }

        private static DependencyObject GetParentFromVisualTree(object source)
        {
            DependencyObject parent = source as UIElement;
            while (parent != null && !(parent is TextBox))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent;
        }

        private static void OnKeyboardFocusSelectText(object sender, KeyboardFocusChangedEventArgs e)
        {
            var box = e.OriginalSource as TextBox;
            if (box != null)
            {
                box.SelectAll();
            }
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var baseBox = d as BaseBox;
            if (baseBox != null)
            {
                baseBox.Culture =(IFormatProvider) e.NewValue;
            }
        }
    }
}
