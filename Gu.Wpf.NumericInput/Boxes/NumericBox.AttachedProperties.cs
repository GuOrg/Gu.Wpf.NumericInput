namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// Attached properties for <see cref="NumericBox"/>.
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

        /// <summary>Helper for getting <see cref="CultureProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="CultureProperty"/> from.</param>
        /// <returns>Culture property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        [TypeConverter(typeof(CultureInfoConverter))]
        public static IFormatProvider GetCulture(this UIElement element) => (IFormatProvider)element.GetValue(CultureProperty);

        /// <summary>Helper for setting <see cref="CultureProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="CultureProperty"/> on.</param>
        /// <param name="value">Culture property value.</param>
        public static void SetCulture(this UIElement element, IFormatProvider value) => element.SetValue(CultureProperty, value);

        /// <summary>Helper for getting <see cref="ValidationTriggerProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="ValidationTriggerProperty"/> from.</param>
        /// <returns>ValidationTrigger property value.</returns>
        [AttachedPropertyBrowsableForChildren(IncludeDescendants = false)]
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ValidationTrigger GetValidationTrigger(this UIElement element) => (ValidationTrigger)element.GetValue(ValidationTriggerProperty);

        /// <summary>Helper for setting <see cref="ValidationTriggerProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="ValidationTriggerProperty"/> on.</param>
        /// <param name="value">ValidationTrigger property value.</param>
        public static void SetValidationTrigger(this UIElement element, ValidationTrigger value) => element.SetValue(ValidationTriggerProperty, value);

        /// <summary>Helper for setting <see cref="CanValueBeNullProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="CanValueBeNullProperty"/> on.</param>
        /// <param name="value">CanValueBeNull property value.</param>
        public static void SetCanValueBeNull(this UIElement element, bool value) => element.SetValue(CanValueBeNullProperty, BooleanBoxes.Box(value));

        /// <summary>Helper for getting <see cref="CanValueBeNullProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="CanValueBeNullProperty"/> from.</param>
        /// <returns>CanValueBeNull property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetCanValueBeNull(this UIElement element) => (bool)element.GetValue(CanValueBeNullProperty);

        /// <summary>Helper for setting <see cref="NumberStylesProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="NumberStylesProperty"/> on.</param>
        /// <param name="value">NumberStyles property value.</param>
        public static void SetNumberStyles(this UIElement element, NumberStyles value) => element.SetValue(NumberStylesProperty, value);

        /// <summary>Helper for getting <see cref="NumberStylesProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="NumberStylesProperty"/> from.</param>
        /// <returns>NumberStyles property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static NumberStyles GetNumberStyles(this UIElement element) => (NumberStyles)element.GetValue(NumberStylesProperty);

        /// <summary>Helper for setting <see cref="StringFormatProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="StringFormatProperty"/> on.</param>
        /// <param name="value">StringFormat property value.</param>
        public static void SetStringFormat(this UIElement element, string value) => element.SetValue(StringFormatProperty, value);

        /// <summary>Helper for getting <see cref="StringFormatProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="StringFormatProperty"/> from.</param>
        /// <returns>StringFormat property value.</returns>
        public static string GetStringFormat(this UIElement element) => (string)element.GetValue(StringFormatProperty);

        /// <summary>Helper for setting <see cref="DecimalDigitsProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="DecimalDigitsProperty"/> on.</param>
        /// <param name="value">DecimalDigits property value.</param>
        public static void SetDecimalDigits(this UIElement element, int? value) => element.SetValue(DecimalDigitsProperty, value);

        /// <summary>Helper for getting <see cref="DecimalDigitsProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="DecimalDigitsProperty"/> from.</param>
        /// <returns>DecimalDigits property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static int? GetDecimalDigits(this UIElement element) => (int?)element.GetValue(DecimalDigitsProperty);

        /// <summary>Helper for getting <see cref="AllowSpinnersProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to read <see cref="AllowSpinnersProperty"/> from.</param>
        /// <returns>AllowSpinners property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static bool GetAllowSpinners(this UIElement element) => (bool)element.GetValue(AllowSpinnersProperty);

        /// <summary>Helper for setting <see cref="AllowSpinnersProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="UIElement"/> to set <see cref="AllowSpinnersProperty"/> on.</param>
        /// <param name="value">AllowSpinners property value.</param>
        public static void SetAllowSpinners(this UIElement element, bool value) => element.SetValue(AllowSpinnersProperty, BooleanBoxes.Box(value));

        /// <summary>Helper for getting <see cref="SpinUpdateModeProperty"/> from <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to read <see cref="SpinUpdateModeProperty"/> from.</param>
        /// <returns>SpinUpdateMode property value.</returns>
        [AttachedPropertyBrowsableForType(typeof(DependencyObject))]
        public static SpinUpdateMode GetSpinUpdateMode(DependencyObject element)
        {
            return (SpinUpdateMode)element.GetValue(SpinUpdateModeProperty);
        }

        /// <summary>Helper for setting <see cref="SpinUpdateModeProperty"/> on <paramref name="element"/>.</summary>
        /// <param name="element"><see cref="DependencyObject"/> to set <see cref="SpinUpdateModeProperty"/> on.</param>
        /// <param name="value">SpinUpdateMode property value.</param>
        public static void SetSpinUpdateMode(DependencyObject element, SpinUpdateMode value)
        {
            element.SetValue(SpinUpdateModeProperty, value);
        }
    }
}
