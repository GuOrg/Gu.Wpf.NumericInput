namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Attached properties for <see cref="NumericBox"/>
    /// </summary>
    public static partial class NumericBox
    {
        public static readonly DependencyProperty CultureProperty = DependencyProperty.RegisterAttached(
            "Culture",
            typeof(IFormatProvider),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(
                CultureInfo.CurrentUICulture,
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

        /// <summary>Identifies the <see cref="SpinUpdateMode"/> dependency property.</summary>
        public static readonly DependencyProperty SpinUpdateModeProperty = DependencyProperty.RegisterAttached(
            "SpinUpdateMode",
            typeof(SpinUpdateMode),
            typeof(NumericBox),
            new FrameworkPropertyMetadata(SpinUpdateMode.AsBinding, FrameworkPropertyMetadataOptions.Inherits));

        internal static readonly DependencyProperty TextProperty = DependencyProperty.RegisterAttached(
            "Text",
            typeof(string),
            typeof(NumericBox),
            new PropertyMetadata(default(string)));

        /// <summary>
        /// Helper for reading Culture property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read Culture property from.</param>
        /// <returns>Culture property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        [TypeConverter(typeof(CultureInfoConverter))]
        public static IFormatProvider GetCulture(this UIElement element) => (IFormatProvider)element.GetValue(CultureProperty);

        /// <summary>
        /// Helper for setting Culture property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set Culture property on.</param>
        /// <param name="value">Culture property value.</param>
        public static void SetCulture(this UIElement element, IFormatProvider value) => element.SetValue(CultureProperty, value);

        /// <summary>
        /// Gets a value indicating when validation is performed, the default is LostFocus to be consistent with vanilla WPF TextBox
        /// Setting ValidationTrigger="PropertyChanged" validates as you type even if the binding has UpdateSourceTrigger=LostFocus.
        /// Inherits so can be set on for example a Window.
        /// </summary>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ValidationTrigger GetValidationTrigger(this UIElement element) => (ValidationTrigger)element.GetValue(ValidationTriggerProperty);

        /// <summary>
        /// Gets a value indicating when validation is performed, the default is LostFocus to be consistent with vanilla WPF TextBox
        /// Setting ValidationTrigger="PropertyChanged" validates as you type even if the binding has UpdateSourceTrigger=LostFocus.
        /// Inherits so can be set on for example a Window.
        /// </summary>
        public static void SetValidationTrigger(this UIElement element, ValidationTrigger value) => element.SetValue(ValidationTriggerProperty, value);

        /// <summary>
        /// Helper for setting CanValueBeNull property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set CanValueBeNull property on.</param>
        /// <param name="value">CanValueBeNull property value.</param>
        public static void SetCanValueBeNull(this UIElement element, bool value) => element.SetValue(CanValueBeNullProperty, BooleanBoxes.Box(value));

        /// <summary>
        /// Helper for reading CanValueBeNull property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read CanValueBeNull property from.</param>
        /// <returns>CanValueBeNull property value.</returns>
        public static bool GetCanValueBeNull(this UIElement element) => (bool)element.GetValue(CanValueBeNullProperty);

        /// <summary>
        /// Helper for setting NumberStyles property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set NumberStyles property on.</param>
        /// <param name="value">NumberStyles property value.</param>
        public static void SetNumberStyles(this UIElement element, NumberStyles value) => element.SetValue(NumberStylesProperty, value);

        /// <summary>
        /// Helper for reading NumberStyles property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read NumberStyles property from.</param>
        /// <returns>NumberStyles property value.</returns>
        public static NumberStyles GetNumberStyles(this UIElement element) => (NumberStyles)element.GetValue(NumberStylesProperty);

        /// <summary>
        /// Helper for setting StringFormat property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set StringFormat property on.</param>
        /// <param name="value">StringFormat property value.</param>
        public static void SetStringFormat(this UIElement element, string value) => element.SetValue(StringFormatProperty, value);

        /// <summary>
        /// Helper for reading StringFormat property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read StringFormat property from.</param>
        /// <returns>StringFormat property value.</returns>
        public static string GetStringFormat(this UIElement element) => (string)element.GetValue(StringFormatProperty);

        /// <summary>
        /// Helper for setting DecimalDigits property on a UIElement.
        /// </summary>
        /// <param name="element">UIElement to set DecimalDigits property on.</param>
        /// <param name="value">DecimalDigits property value.</param>
        public static void SetDecimalDigits(this UIElement element, int? value) => element.SetValue(DecimalDigitsProperty, value);

        /// <summary>
        /// Helper for reading DecimalDigits property from a UIElement.
        /// </summary>
        /// <param name="element">UIElement to read DecimalDigits property from.</param>
        /// <returns>DecimalDigits property value.</returns>
        public static int? GetDecimalDigits(this UIElement element) => (int?)element.GetValue(DecimalDigitsProperty);

        /// <summary>
        /// Gets a value indicating whether spinners should be visible.
        /// </summary>
        public static bool GetAllowSpinners(this UIElement element) => (bool)element.GetValue(AllowSpinnersProperty);

        /// <summary>
        /// Sets a value indicating whether spinners should be visible.
        /// </summary>
        public static void SetAllowSpinners(this UIElement element, bool value) => element.SetValue(AllowSpinnersProperty, BooleanBoxes.Box(value));

        /// <summary>
        /// Get a value that specifies how the IncreaseCommand and DecreaseCommand behaves.
        /// The default is AsBinding meaning the value updates using the UpdateSourceTrigger specified in the binding. Default is LostFocus.
        /// If set to PropertyChanged the binding source will be updated at each click even if the binding has UpdateSourceTrigger = LostFocus
        /// </summary>
        public static SpinUpdateMode GetSpinUpdateMode(DependencyObject element)
        {
            return (SpinUpdateMode)element.GetValue(SpinUpdateModeProperty);
        }

        /// <summary>
        /// Set a value that specifies how the IncreaseCommand and DecreaseCommand behaves.
        /// The default is AsBinding meaning the value updates using the UpdateSourceTrigger specified in the binding. Default is LostFocus.
        /// If set to PropertyChanged the binding source will be updated at each click even if the binding has UpdateSourceTrigger = LostFocus
        /// </summary>
        public static void SetSpinUpdateMode(DependencyObject element, SpinUpdateMode value)
        {
            element.SetValue(SpinUpdateModeProperty, value);
        }
    }
}
