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
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange)
                {
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    CoerceValueCallback = OnSuffixCoerce 
                });

        private static readonly DependencyPropertyKey StringFormatPropertyKey = DependencyProperty.RegisterReadOnly(
            "StringFormat",
            typeof(string),
            typeof(BaseBox),
            new PropertyMetadata(""));

        /// <summary>
        /// Identifies the Culture property
        /// </summary>
        public static readonly DependencyProperty CultureProperty = DependencyProperty.Register(
            "Culture", 
            typeof (IFormatProvider), 
            typeof (BaseBox), 
            new FrameworkPropertyMetadata(
                CultureInfo.GetCultureInfo("en-US"), // Think this is the default in WPF
                FrameworkPropertyMetadataOptions.AffectsMeasure|FrameworkPropertyMetadataOptions.Inherits));

        /// <summary>
        /// Identifies the RegexPattern property
        /// </summary>
        public static readonly DependencyProperty RegexPatternProperty = DependencyProperty.Register(
            "RegexPattern", 
            typeof(string), 
            typeof(BaseBox), 
            new PropertyMetadata(default(string)));

        /// <summary>
        /// Identifies the StringFormat property
        /// </summary>
        public static readonly DependencyProperty StringFormatProperty = StringFormatPropertyKey.DependencyProperty;

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
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(BaseBox),
                new FrameworkPropertyMetadata(typeof(BaseBox)));
        }

        protected BaseBox()
        {
            IncreaseCommand = new ManualRelayCommand(Increase, CanIncrease);
            DecreaseCommand = new ManualRelayCommand(Decrease, CanDecrease);
        }

        [Description(""), Category("BaseBox"), Browsable(true)]
        public string Suffix
        {
            get
            {
                return (string)GetValue(SuffixProperty);
            }
            set
            {
                SetValue(SuffixProperty, value);
            }
        }

        /// <summary>
        /// The Culture used for T -> string
        /// </summary>
        public IFormatProvider Culture
        {
            get { return (IFormatProvider)GetValue(CultureProperty); }
            set { SetValue(CultureProperty, value); }
        }

        /// <summary>
        /// The stringformat is set using the Decimals property, exposed for binding in template.
        /// </summary>
        public string StringFormat
        {
            get
            {
                return (string)GetValue(StringFormatProperty);
            }
            protected set
            {
                SetValue(StringFormatPropertyKey, value);
            }
        }


        /// <summary>
        /// Regex pattern for validation
        /// </summary>
        public string RegexPattern
        {
            get
            {
                return (string)GetValue(RegexPatternProperty);
            }
            set
            {
                SetValue(RegexPatternProperty, value);
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public bool AllowSpinners
        {
            get
            {
                return (bool)GetValue(AllowSpinnersProperty);
            }
            set
            {
                SetValue(AllowSpinnersProperty, value);
            }
        }

        /// <summary>
        /// Increases the value by increment
        /// </summary>
        public ICommand IncreaseCommand
        {
            get
            {
                return (ICommand)GetValue(IncreaseCommandProperty);
            }
            protected set
            {
                SetValue(IncreaseCommandPropertyKey, value);
            }
        }

        /// <summary>
        /// Decreases the value by increment
        /// </summary>
        public ICommand DecreaseCommand
        {
            get
            {
                return (ICommand)GetValue(DecreaseCommandProperty);
            }
            private set
            {
                SetValue(DecreaseCommandPropertyKey, value);
            }
        }

        /// <summary>
        /// Invoked when IncreaseCommand.Execute() is executed
        /// </summary>
        /// <returns></returns>
        protected abstract void Increase(object parameter);

        /// <summary>
        /// Invoked when IncreaseCommand.CanExecute() is executed
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanIncrease(object parameter)
        {
            if (IsReadOnly)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Invoked when DecreaseCommand.Execute() is executed
        /// </summary>
        /// <returns></returns>
        protected abstract void Decrease(object parameter);

        /// <summary>
        /// Invoked when DecreaseCommand.CanExecute() is executed
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanDecrease(object parameter)
        {
            if (IsReadOnly)
            {
                return false;
            }
            return true;
        }

        protected virtual void CheckSpinners()
        {
            // Not nice to cast like this but want to have ManualRelayCommand as internal
            ((ManualRelayCommand)IncreaseCommand).RaiseCanExecuteChanged();
            ((ManualRelayCommand)DecreaseCommand).RaiseCanExecuteChanged();
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