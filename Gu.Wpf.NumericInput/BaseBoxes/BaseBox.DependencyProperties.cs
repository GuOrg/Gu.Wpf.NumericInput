#pragma warning disable SA1202 // Elements must be ordered by access. Reason: this does not work with dependency properties
namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Base class that adds a couple of dependency properties to TextBox
    /// </summary>
    public abstract partial class BaseBox : TextBox
    {
        public static readonly DependencyProperty SuffixProperty = DependencyProperty.Register(
            "Suffix",
            typeof(string),
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                OnSuffixChanged,
                OnSuffixCoerce));

        private static readonly DependencyPropertyKey HasSuffixPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasSuffix",
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(false));

        public static readonly DependencyProperty HasSuffixProperty = HasSuffixPropertyKey.DependencyProperty;

        public static readonly DependencyProperty StringFormatProperty = DependencyProperty.Register(
            "StringFormat",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(string.Empty, OnStringFormatChanged));

        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            "Culture",
            typeof(IFormatProvider),
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                CultureInfo.GetCultureInfo("en-US"), // Think this is the default in WPF
                FrameworkPropertyMetadataOptions.Inherits,
                OnCultureChanged));

        public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.Register(
            "RegexPattern",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(default(string), OnPatternChanged));

        public static readonly DependencyProperty AllowSpinnersProperty = DependencyProperty.Register(
            "AllowSpinners",
            typeof(bool),
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsArrange));

        private static readonly DependencyPropertyKey IncreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "IncreaseCommand",
            typeof(ICommand),
            typeof(BaseBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IncreaseCommandProperty = IncreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey DecreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "DecreaseCommand",
            typeof(ICommand),
            typeof(BaseBox),
            new PropertyMetadata(null));

        public static readonly DependencyProperty DecreaseCommandProperty = DecreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyProperty TextProxyProperty = DependencyProperty.Register(
            "TextProxy",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(
                default(string),
                OnTextProxyChanged));

        internal static readonly DependencyProperty TextBindableProperty = DependencyProperty.Register(
            "TextBindable",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(
                default(string),
                OnTextBindableChanged));

        static BaseBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseBox), new FrameworkPropertyMetadata(typeof(BaseBox)));
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public string Suffix
        {
            get { return (string)this.GetValue(SuffixProperty); }
            set { this.SetValue(SuffixProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public bool HasSuffix
        {
            get { return (bool)this.GetValue(HasSuffixProperty); }
            private set { this.SetValue(HasSuffixPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the  culture for the control.
        /// The control has an explicit culture and does not use <see cref="System.Threading.Thread.CurrentUICulture"/>
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
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

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public bool AllowSpinners
        {
            get { return (bool)this.GetValue(AllowSpinnersProperty); }
            set { this.SetValue(AllowSpinnersProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public ICommand IncreaseCommand
        {
            get { return (ICommand)this.GetValue(IncreaseCommandProperty); }
            private set { this.SetValue(IncreaseCommandPropertyKey, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public ICommand DecreaseCommand
        {
            get { return (ICommand)this.GetValue(DecreaseCommandProperty); }
            private set { this.SetValue(DecreaseCommandPropertyKey, value); }
        }

        private static void OnStringFormatChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            box.RaiseEvent(FormatDirtyEventArgs);
            box.RaiseEvent(ValidationDirtyEventArgs);
        }

        private static void OnCultureChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            box.RaiseEvent(FormatDirtyEventArgs);
            box.RaiseEvent(ValidationDirtyEventArgs);
        }

        private static void OnPatternChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (BaseBox)d;
            box.RaiseEvent(ValidationDirtyEventArgs);
        }

        private static void OnSuffixChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BaseBox)d).HasSuffix = !string.IsNullOrEmpty(e.NewValue as string);
        }

        private static object OnSuffixCoerce(DependencyObject dependencyObject, object baseValue)
        {
            var value = (string)baseValue;
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return baseValue;
        }

        private static void OnTextProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(TextBindableProperty, e.NewValue);
        }

        private static void OnTextBindableChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            d.SetCurrentValue(TextProperty, e.NewValue);
        }
    }
}