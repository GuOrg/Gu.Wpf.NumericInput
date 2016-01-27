namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows;

    public static partial class NumericBox
    {
        public static readonly DependencyProperty CultureProperty = DependencyProperty.RegisterAttached(
            "Culture",
            typeof(IFormatProvider),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(
                Thread.CurrentThread.CurrentUICulture,
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty ValidationTriggerProperty = DependencyProperty.RegisterAttached(
            "ValidationTrigger",
            typeof(ValidationTrigger),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(
                ValidationTrigger.LostFocus,
                FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty CanValueBeNullProperty = DependencyProperty.RegisterAttached(
            "CanValueBeNull",
            typeof(bool),
            typeof(NumericBox),
            new PropertyMetadata(BooleanBoxes.False));

        public static readonly DependencyProperty NumberStylesProperty = DependencyProperty.RegisterAttached(
            "NumberStyles",
            typeof(NumberStyles),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(NumberStyles.None, FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.RegisterAttached(
            "StringFormat",
            typeof(string),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty DecimalDigitsProperty = DependencyProperty.RegisterAttached(
            "DecimalDigits",
            typeof(int?),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(default(int?), FrameworkPropertyMetadataOptions.Inherits));

        public static readonly DependencyProperty AllowSpinnersProperty = DependencyProperty.RegisterAttached(
            "AllowSpinners",
            typeof(bool),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(BooleanBoxes.False, FrameworkPropertyMetadataOptions.Inherits));

        internal static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(NumericBox),
            new PropertyMetadata(default(string)));

        public static void SetCulture(this UIElement element, CultureInfo value)
        {
            element.SetValue(CultureProperty, value);
        }

        public static CultureInfo GetCulture(this UIElement element)
        {
            return (CultureInfo)element.GetValue(CultureProperty);
        }

        public static void SetValidationTrigger(this UIElement element, ValidationTrigger value)
        {
            element.SetValue(ValidationTriggerProperty, value);
        }

        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ValidationTrigger GetValidationTrigger(this UIElement element)
        {
            return (ValidationTrigger)element.GetValue(ValidationTriggerProperty);
        }

        public static void SetCanValueBeNull(this UIElement element, bool value)
        {
            element.SetValue(CanValueBeNullProperty, BooleanBoxes.Box(value));
        }

        public static bool GetCanValueBeNull(this UIElement element)
        {
            return (bool)element.GetValue(CanValueBeNullProperty);
        }

        public static void SetNumberStyles(this UIElement element, NumberStyles value)
        {
            element.SetValue(NumberStylesProperty, value);
        }

        public static NumberStyles GetNumberStyles(this UIElement element)
        {
            return (NumberStyles)element.GetValue(NumberStylesProperty);
        }

        public static void SetStringFormat(this UIElement element, string value)
        {
            element.SetValue(StringFormatProperty, value);
        }

        public static string GetStringFormat(this UIElement element)
        {
            return (string)element.GetValue(StringFormatProperty);
        }

        public static void SetDecimalDigits(this UIElement element, int? value)
        {
            element.SetValue(DecimalDigitsProperty, value);
        }

        public static int? GetDecimalDigits(this UIElement element)
        {
            return (int?)element.GetValue(DecimalDigitsProperty);
        }

        public static void SetAllowSpinners(this UIElement element, bool value)
        {
            element.SetValue(AllowSpinnersProperty, BooleanBoxes.Box(value));
        }

        public static bool GetAllowSpinners(this UIElement element)
        {
            return (bool)element.GetValue(AllowSpinnersProperty);
        }
    }
}
