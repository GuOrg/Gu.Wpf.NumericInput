#pragma warning disable SA1202 // Elements must be ordered by access. Reason: this does not work with dependency properties
namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Base class that adds a couple of dependency properties to TextBox
    /// </summary>
    public abstract class BaseBox : TextBox
    {
        /// <summary>
        /// Identifies the Suffix property
        /// </summary>
        public static readonly DependencyProperty SuffixProperty = DependencyProperty.Register(
            "Suffix",
            typeof(string),
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange,
                OnSuffixChanged,
                OnSuffixCoerce)
            {
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

        private static readonly DependencyPropertyKey HasSuffixPropertyKey = DependencyProperty.RegisterReadOnly(
            "HasSuffix",
            typeof(bool),
            typeof(BaseBox),
            new PropertyMetadata(false));

        public static readonly DependencyProperty HasSuffixProperty = HasSuffixPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey StringFormatPropertyKey = DependencyProperty.RegisterReadOnly(
            "StringFormat",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(string.Empty));

        /// <summary>
        /// Identifies the StringFormat property
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty = StringFormatPropertyKey.DependencyProperty;

        /// <summary>
        /// Identifies the Culture property
        /// </summary>
        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            "Culture",
            typeof(IFormatProvider),
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                CultureInfo.GetCultureInfo("en-US"), // Think this is the default in WPF
                FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the RegexPattern property
        /// </summary>
        public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.Register(
            "RegexPattern",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(default(string)));

        /// <summary>
        /// Identifies the AllowSpinners property
        /// </summary>
        public static readonly DependencyProperty AllowSpinnersProperty = DependencyProperty.Register(
            "AllowSpinners",
            typeof(bool),
            typeof(BaseBox),
            new FrameworkPropertyMetadata(
                false,
                FrameworkPropertyMetadataOptions.AffectsArrange)
            {
                BindsTwoWayByDefault = true,
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged
            });

        private static readonly DependencyPropertyKey IncreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "IncreaseCommand",
            typeof(ICommand),
            typeof(BaseBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the IncreaseCommand property
        /// </summary>
        public static readonly DependencyProperty IncreaseCommandProperty = IncreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey DecreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "DecreaseCommand",
            typeof(ICommand),
            typeof(BaseBox),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the DecreaseCommand property
        /// </summary>
        public static readonly DependencyProperty DecreaseCommandProperty = DecreaseCommandPropertyKey.DependencyProperty;

        static BaseBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(BaseBox), new FrameworkPropertyMetadata(typeof(BaseBox)));
        }

        /// <summary>
        /// Creates an instance of <see cref="T:Gu.NumericInput.BaseBox"/>
        /// </summary>
        protected BaseBox()
        {
            this.IncreaseCommand = new ManualRelayCommand(this.Increase, this.CanIncrease);
            this.DecreaseCommand = new ManualRelayCommand(this.Decrease, this.CanDecrease);
        }

        [Description("")]
        [Category("BaseBox")]
        [Browsable(true)]
        public string Suffix
        {
            get { return (string)this.GetValue(SuffixProperty); }
            set { this.SetValue(SuffixProperty, value); }
        }

        public bool HasSuffix
        {
            get { return (bool)this.GetValue(HasSuffixProperty); }
            protected set { this.SetValue(HasSuffixPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets the  culture for the control.
        /// The control has an explicit culture and does not use <see cref="System.Threading.Thread.CurrentUICulture"/>
        /// </summary>
        public IFormatProvider Culture
        {
            get { return (IFormatProvider)this.GetValue(CultureProperty); }
            set { this.SetValue(CultureProperty, value); }
        }

        /// <inheritdoc/>
        public string StringFormat
        {
            get { return (string)this.GetValue(StringFormatProperty); }
            protected set { this.SetValue(StringFormatPropertyKey, value); }
        }

        /// <summary>
        /// Gets or sets a regex pattern for validation
        /// </summary>
        public string RegexPattern
        {
            get { return (string)this.GetValue(RegexPatternProperty); }
            set { this.SetValue(RegexPatternProperty, value); }
        }

        [Description("")]
        [Category("NumericBox")]
        [Browsable(true)]
        public bool AllowSpinners
        {
            get { return (bool)this.GetValue(AllowSpinnersProperty); }
            set { this.SetValue(AllowSpinnersProperty, value); }
        }

        /// <summary>
        /// Gets or sets increases the value by increment
        /// </summary>
        public ICommand IncreaseCommand
        {
            get { return (ICommand)this.GetValue(IncreaseCommandProperty); }
            protected set { this.SetValue(IncreaseCommandPropertyKey, value); }
        }

        /// <summary>
        /// Gets decreases the value by increment
        /// </summary>
        public ICommand DecreaseCommand
        {
            get { return (ICommand)this.GetValue(DecreaseCommandProperty); }
            private set { this.SetValue(DecreaseCommandPropertyKey, value); }
        }

        /// <summary>
        /// Invoked when IncreaseCommand.Execute() is executed
        /// </summary>
        /// <param name="parameter">The inner <see cref="TextBox"/> showing the value in the controltemplate</param>
        protected abstract void Increase(object parameter);

        /// <summary>
        /// Invoked when IncreaseCommand.CanExecute() is executed
        /// </summary>
        /// <param name="parameter">The inner <see cref="TextBox"/> showing the value in the controltemplate</param>
        /// <returns>True if the value can be increased</returns>
        protected virtual bool CanIncrease(object parameter)
        {
            if (this.IsReadOnly)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Invoked when DecreaseCommand.Execute() is executed
        /// </summary>
        /// <param name="parameter">The inner <see cref="TextBox"/> showing the value in the controltemplate</param>
        protected abstract void Decrease(object parameter);

        /// <summary>
        /// Invoked when DecreaseCommand.CanExecute() is executed
        /// </summary>
        /// <param name="parameter">The inner <see cref="TextBox"/> showing the value in the controltemplate</param>
        /// <returns>True if the value can be decreased</returns>
        protected virtual bool CanDecrease(object parameter)
        {
            if (this.IsReadOnly)
            {
                return false;
            }

            return true;
        }

        protected virtual void CheckSpinners()
        {
            // Not nice to cast like this but want to have ManualRelayCommand as internal
            ((ManualRelayCommand)this.IncreaseCommand).RaiseCanExecuteChanged();
            ((ManualRelayCommand)this.DecreaseCommand).RaiseCanExecuteChanged();
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Equals(e.NewValue, false))
            {
                // this is needed because the inner textbox gets focus
                this.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
            }

            base.OnIsKeyboardFocusWithinChanged(e);
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
    }
}