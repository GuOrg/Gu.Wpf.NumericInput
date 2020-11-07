namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// DependencyProperties for <see cref="BaseBox"/>.
    /// </summary>
    public abstract partial class BaseBox
    {
        private static readonly DependencyPropertyKey HasFormattedViewPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(HasFormattedView),
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(default(bool)));

        /// <summary>Identifies the <see cref="HasFormattedView"/> dependency property.</summary>
        public static readonly DependencyProperty HasFormattedViewProperty = HasFormattedViewPropertyKey.DependencyProperty;

        /// <summary>Identifies the <see cref="TextValueConverter"/> dependency property.</summary>
        public static readonly DependencyProperty TextValueConverterProperty = DependencyProperty.Register(
            nameof(TextValueConverter),
            typeof(IValueConverter),
            typeof(BaseBox),
            new PropertyMetadata(default(IValueConverter), OnTextValueConverterChanged));

        /// <summary>Identifies the <see cref="ValidationTrigger"/> dependency property.</summary>
        public static readonly DependencyProperty ValidationTriggerProperty = NumericBox.ValidationTriggerProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(ValidationTrigger.LostFocus, FrameworkPropertyMetadataOptions.Inherits, OnValidationTriggerChanged));

        /// <summary>Identifies the <see cref="ValidationRules"/> dependency property.</summary>
        public static readonly DependencyProperty ValidationRulesProperty = DependencyProperty.Register(
            nameof(ValidationRules),
            typeof(IReadOnlyList<ValidationRule>),
            typeof(BaseBox),
            new PropertyMetadata(default(IReadOnlyList<ValidationRule>), OnValidationRulesChanged));

        private static readonly DependencyPropertyKey FormattedTextPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(FormattedText),
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(default(string)));

        /// <summary>Identifies the <see cref="FormattedText"/> dependency property.</summary>
        public static readonly DependencyProperty FormattedTextProperty = FormattedTextPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsValidationDirtyPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IsValidationDirty),
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(
                BooleanBoxes.False,
                OnIsValidationDirtyChanged));

        /// <summary>Identifies the <see cref="IsValidationDirty"/> dependency property.</summary>
        internal static readonly DependencyProperty IsValidationDirtyProperty = IsValidationDirtyPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsFormattingDirtyPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IsFormattingDirty),
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(
                BooleanBoxes.False,
                OnIsFormattingDirtyChanged));

        /// <summary>Identifies the <see cref="IsFormattingDirty"/> dependency property.</summary>
        internal static readonly DependencyProperty IsFormattingDirtyProperty = IsFormattingDirtyPropertyKey.DependencyProperty;

        /// <summary>Identifies the <see cref="StringFormat"/> dependency property.</summary>
        public static readonly DependencyProperty StringFormatProperty = NumericBox.StringFormatProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits, OnStringFormatChanged));

        /// <summary>Identifies the <see cref="Culture"/> dependency property.</summary>
        public static readonly DependencyProperty CultureProperty = NumericBox.CultureProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                CultureInfo.CurrentUICulture,
                FrameworkPropertyMetadataOptions.Inherits,
                OnCultureChanged));

        /// <summary>Identifies the <see cref="RegexPattern"/> dependency property.</summary>
        public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.Register(
            nameof(RegexPattern),
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(default(string), OnRegexPatternChanged));

        /// <summary>Identifies the <see cref="TextSource"/> dependency property.</summary>
        protected static readonly DependencyPropertyKey TextSourcePropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(TextSource),
            typeof(TextSource),
            typeof(BaseBox),
            new PropertyMetadata(TextSource.None));

        /// <summary>Identifies the <see cref="TextSource"/> dependency property.</summary>
        public static readonly DependencyProperty TextSourceProperty = TextSourcePropertyKey.DependencyProperty;

        /// <summary>Identifies the <see cref="Status"/> dependency property.</summary>
        internal static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(Status),
            typeof(Status),
            typeof(BaseBox),
            new PropertyMetadata(Status.Idle));

        /// <summary>Identifies the <see cref="Status"/> dependency property.</summary>
        internal static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        static BaseBox()
        {
            TextProperty.OverrideMetadataWithOptions(typeof(BaseBox), FrameworkPropertyMetadataOptions.NotDataBindable);
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseBox), new FrameworkPropertyMetadata(typeof(BaseBox)));
        }

        /// <summary>
        /// Gets or sets a value indicating whether this instance has a formatted view.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public bool HasFormattedView
        {
            get => (bool)this.GetValue(HasFormattedViewProperty);
            protected internal set => this.SetValue(HasFormattedViewPropertyKey, value);
        }

        /// <summary>
        /// Gets or sets the <see cref="IValueConverter"/>.
        /// </summary>
        public IValueConverter? TextValueConverter
        {
            get => (IValueConverter?)this.GetValue(TextValueConverterProperty);
            set => this.SetValue(TextValueConverterProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating when validation is performed, the default is LostFocus to be consistent with vanilla WPF TextBox
        /// Setting ValidationTrigger="PropertyChanged" validates as you type even if the binding has UpdateSourceTrigger=LostFocus.
        /// Inherits so can be set on for example a Window.
        /// </summary>
        public ValidationTrigger ValidationTrigger
        {
            get => (ValidationTrigger)this.GetValue(ValidationTriggerProperty);
            set => this.SetValue(ValidationTriggerProperty, value);
        }

        /// <summary>
        /// Gets or sets the validation rules.
        /// </summary>
        public IReadOnlyList<ValidationRule>? ValidationRules
        {
            get => (IReadOnlyList<ValidationRule>?)this.GetValue(ValidationRulesProperty);
            set => this.SetValue(ValidationRulesProperty, value);
        }

        /// <summary>
        /// Gets or sets the formatted text that is displayed when not focused.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string? FormattedText
        {
            get => (string?)this.GetValue(FormattedTextProperty);
            protected set => this.SetValue(FormattedTextPropertyKey, value);
        }

        /// <summary>
        /// Gets or sets the  culture for the control.
        /// The control has an explicit culture and does not use <see cref="System.Threading.Thread.CurrentUICulture"/>.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        [TypeConverter(typeof(CultureInfoConverter))]
        public IFormatProvider Culture
        {
            get => (IFormatProvider)this.GetValue(CultureProperty);
            set => this.SetValue(CultureProperty, value);
        }

        /// <summary>
        /// Gets or sets the string format to use when formatting the value.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string? StringFormat
        {
            get => (string?)this.GetValue(StringFormatProperty);
            set => this.SetValue(StringFormatProperty, value);
        }

        /// <summary>
        /// Gets or sets a <see cref="System.Text.RegularExpressions.Regex"/> pattern for validation.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string? RegexPattern
        {
            get => (string?)this.GetValue(RegexPatternProperty);
            set => this.SetValue(RegexPatternProperty, value);
        }

        /// <summary>
        /// Gets or sets the source of the current text.
        /// </summary>
        public TextSource TextSource
        {
            get => (TextSource)this.GetValue(TextSourceProperty);
            protected set => this.SetValue(TextSourcePropertyKey, value);
        }

        internal bool IsFormattingDirty
        {
            get => (bool)this.GetValue(IsFormattingDirtyProperty);
            set => this.SetValue(IsFormattingDirtyPropertyKey, value ? BooleanBoxes.True : BooleanBoxes.False);
        }

        internal bool IsValidationDirty
        {
            get => (bool)this.GetValue(IsValidationDirtyProperty);
            set => this.SetValue(IsValidationDirtyPropertyKey, value ? BooleanBoxes.True : BooleanBoxes.False);
        }

        internal Status Status
        {
            get => (Status)this.GetValue(StatusProperty);
            set => this.SetValue(StatusPropertyKey, value);
        }

        private static void OnIsValidationDirtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None && Equals(e.NewValue, BooleanBoxes.True))
            {
                box.RaiseEvent(ValidationDirtyEventArgs);
            }
        }

        private static void OnIsFormattingDirtyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
            if (Equals(e.NewValue, BooleanBoxes.True))
            {
                ((BaseBox)d).RaiseEvent(FormatDirtyEventArgs);
            }
        }

        private static void OnTextValueConverterChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsFormattingDirty = true;
                box.IsValidationDirty = true;
            }
        }

        private static void OnValidationTriggerChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                switch ((ValidationTrigger)e.NewValue)
                {
                    case ValidationTrigger.PropertyChanged:
                        box.UpdateValidation();
                        break;
                    case ValidationTrigger.LostFocus:
                        if (box.IsKeyboardFocused)
                        {
                            box.IsValidationDirty = true;
                        }
                        else
                        {
                            box.UpdateValidation();
                        }

                        break;
                    case ValidationTrigger.Explicit:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(d), box.TextSource, "Unhandled text source.");
                }
            }
        }

        private static void OnValidationRulesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsValidationDirty = true;
            }
        }

        private static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.OnStringFormatChanged((string?)e.OldValue, (string?)e.NewValue);
                box.IsFormattingDirty = true;
                box.IsValidationDirty = true;
            }
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.OnCultureChanged((IFormatProvider)e.OldValue, (IFormatProvider)e.NewValue);
                box.IsFormattingDirty = true;
                box.IsValidationDirty = true;
            }
        }

        private static void OnRegexPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsValidationDirty = true;
            }
        }
    }
}
