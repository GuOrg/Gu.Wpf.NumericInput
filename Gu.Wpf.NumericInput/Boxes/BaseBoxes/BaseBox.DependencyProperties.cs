namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Threading;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// DependencyProperties for <see cref="BaseBox"/>
    /// </summary>
    public abstract partial class BaseBox
    {
        private static readonly DependencyPropertyKey HasFormattedViewPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasFormattedView",
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(default(bool)));

        public static readonly DependencyProperty HasFormattedViewProperty = HasFormattedViewPropertyKey.DependencyProperty;

        public static readonly DependencyProperty TextValueConverterProperty = DependencyProperty.Register(
            "TextValueConverter",
            typeof(IValueConverter),
            typeof(BaseBox),
            new PropertyMetadata(default(IValueConverter), OnTextValueConverterChanged));

        public static readonly DependencyProperty ValidationTriggerProperty = NumericBox.ValidationTriggerProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(ValidationTrigger.LostFocus, FrameworkPropertyMetadataOptions.Inherits, OnValidationTriggerChanged));

        public static readonly DependencyProperty ValidationRulesProperty = DependencyProperty.Register(
            "ValidationRules",
            typeof(IReadOnlyList<ValidationRule>),
            typeof(BaseBox),
            new PropertyMetadata(default(IReadOnlyList<ValidationRule>), OnValidationRulesChanged));

        private static readonly DependencyPropertyKey FormattedTextPropertyKey = DependencyProperty.RegisterReadOnly(
            "FormattedText",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(default(string)));

        public static readonly DependencyProperty FormattedTextProperty = FormattedTextPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsValidationDirtyPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsValidationDirty",
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(
                BooleanBoxes.False,
                OnIsValidationDirtyChanged));

        internal static readonly DependencyProperty IsValidationDirtyProperty = IsValidationDirtyPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey IsFormattingDirtyPropertyKey = DependencyProperty.RegisterReadOnly(
            "IsFormattingDirty",
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(
                BooleanBoxes.False,
                OnIsFormattingDirtyChanged));

        internal static readonly DependencyProperty IsFormattingDirtyProperty = IsFormattingDirtyPropertyKey.DependencyProperty;

        public static readonly DependencyProperty StringFormatProperty = NumericBox.StringFormatProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(string.Empty, FrameworkPropertyMetadataOptions.Inherits, OnStringFormatChanged));

        public static readonly DependencyProperty CultureProperty = NumericBox.CultureProperty.AddOwner(
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                Thread.CurrentThread.CurrentUICulture,
                FrameworkPropertyMetadataOptions.Inherits,
                OnCultureChanged));

        public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.Register(
            "RegexPattern",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(default(string), OnPatternChanged));

        protected static readonly DependencyPropertyKey TextSourcePropertyKey = DependencyProperty.RegisterReadOnly(
            "TextSource",
            typeof(TextSource),
            typeof(BaseBox),
            new PropertyMetadata(TextSource.None, OnTextSourceChanged));

        public static readonly DependencyProperty TextSourceProperty = TextSourcePropertyKey.DependencyProperty;

        internal static readonly DependencyProperty StatusProperty = DependencyProperty.Register(
            "Status",
            typeof(Status),
            typeof(BaseBox),
            new PropertyMetadata(Status.Idle, OnStatusChanged));

        static BaseBox()
        {
            TextProperty.OverrideMetadataWithOptions(typeof(BaseBox), FrameworkPropertyMetadataOptions.NotDataBindable);
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseBox), new FrameworkPropertyMetadata(typeof(BaseBox)));
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public bool HasFormattedView
        {
            get { return (bool)this.GetValue(HasFormattedViewProperty); }
            protected internal set { this.SetValue(HasFormattedViewPropertyKey, value); }
        }

        public IValueConverter TextValueConverter
        {
            get { return (IValueConverter)this.GetValue(TextValueConverterProperty); }
            set { this.SetValue(TextValueConverterProperty, value); }
        }

        public ValidationTrigger ValidationTrigger
        {
            get { return (ValidationTrigger)this.GetValue(ValidationTriggerProperty); }
            set { this.SetValue(ValidationTriggerProperty, value); }
        }

        public IReadOnlyList<ValidationRule> ValidationRules
        {
            get { return (IReadOnlyList<ValidationRule>)this.GetValue(ValidationRulesProperty); }
            set { this.SetValue(ValidationRulesProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string FormattedText
        {
            get { return (string)this.GetValue(FormattedTextProperty); }
            protected set { this.SetValue(FormattedTextPropertyKey, value); }
        }

        internal bool IsFormattingDirty
        {
            get { return (bool)this.GetValue(IsFormattingDirtyProperty); }
            set { this.SetValue(IsFormattingDirtyPropertyKey, value ? BooleanBoxes.True : BooleanBoxes.False); }
        }

        internal bool IsValidationDirty
        {
            get { return (bool)this.GetValue(IsValidationDirtyProperty); }
            set { this.SetValue(IsValidationDirtyPropertyKey, value ? BooleanBoxes.True : BooleanBoxes.False); }
        }

        /// <summary>
        /// Gets or sets the  culture for the control.
        /// The control has an explicit culture and does not use <see cref="System.Threading.Thread.CurrentUICulture"/>
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        [TypeConverter(typeof(CultureInfoConverter))]
        public IFormatProvider Culture
        {
            get { return (IFormatProvider)this.GetValue(CultureProperty); }
            set { this.SetValue(CultureProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string StringFormat
        {
            get { return (string)this.GetValue(StringFormatProperty); }
            set { this.SetValue(StringFormatProperty, value); }
        }

        /// <summary>
        /// Gets or sets a regex pattern for validation
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string RegexPattern
        {
            get { return (string)this.GetValue(RegexPatternProperty); }
            set { this.SetValue(RegexPatternProperty, value); }
        }

        public TextSource TextSource
        {
            get { return (TextSource)this.GetValue(TextSourceProperty); }
            protected set { this.SetValue(TextSourcePropertyKey, value); }
        }

        internal Status Status
        {
            get { return (Status)this.GetValue(StatusProperty); }
            set { this.SetValue(StatusProperty, value); }
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
                        throw new ArgumentOutOfRangeException();
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
                box.OnStringFormatChanged((string)e.OldValue, (string)e.NewValue);
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

        private static void OnPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsValidationDirty = true;
            }
        }

        private static void OnStatusChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
        }

        private static void OnTextSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
        }
    }
}