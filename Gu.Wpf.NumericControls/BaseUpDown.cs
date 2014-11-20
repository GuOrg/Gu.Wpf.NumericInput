namespace Gu.Wpf.NumericControls
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    public abstract class BaseUpDown : TextBox
    {
        /// <summary>
        /// Identifies the Suffix property
        /// </summary>
        public static readonly DependencyProperty SuffixProperty = DependencyProperty.Register(
            "Suffix",
            typeof(string),
            typeof(BaseUpDown),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.AffectsMeasure | FrameworkPropertyMetadataOptions.AffectsArrange)
                {
                    DefaultUpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                    CoerceValueCallback = OnSuffixCoerce 
                    
                });

        /// <summary>
        /// Identifies the AllowSpinners property
        /// </summary>
        public static readonly DependencyProperty AllowSpinnersProperty = DependencyProperty.Register(
            "AllowSpinners",
            typeof(bool),
            typeof(BaseUpDown),
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
            typeof(BaseUpDown),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the IncreaseCommand property
        /// </summary>
        public static readonly DependencyProperty IncreaseCommandProperty = IncreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey DecreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "DecreaseCommand",
            typeof(ICommand),
            typeof(BaseUpDown),
            new PropertyMetadata(null));

        /// <summary>
        /// Identifies the DecreaseCommand property
        /// </summary>
        public static readonly DependencyProperty DecreaseCommandProperty = DecreaseCommandPropertyKey.DependencyProperty;

        static BaseUpDown()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(BaseUpDown),
                new FrameworkPropertyMetadata(typeof(BaseUpDown)));
        }

        protected BaseUpDown()
        {
            IncreaseCommand = new ManualRelayCommand(Increase, CanIncrease);
            DecreaseCommand = new ManualRelayCommand(this.Decrease, this.CanDecrease);
        }

        [Description(""), Category("BaseUpDown"), Browsable(true)]
        public string Suffix
        {
            get
            {
                return (string)this.GetValue(SuffixProperty);
            }
            set
            {
                this.SetValue(SuffixProperty, value);
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public bool AllowSpinners
        {
            get
            {
                return (bool)this.GetValue(AllowSpinnersProperty);
            }
            set
            {
                this.SetValue(AllowSpinnersProperty, value);
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
        protected abstract void Increase();

        /// <summary>
        /// Invoked when IncreaseCommand.CanExecute() is executed
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanIncrease()
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
        protected abstract void Decrease();

        /// <summary>
        /// Invoked when DecreaseCommand.CanExecute() is executed
        /// </summary>
        /// <returns></returns>
        protected virtual bool CanDecrease()
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
            ((ManualRelayCommand)IncreaseCommand).TryRaiseCanExecuteChanged();
            ((ManualRelayCommand)DecreaseCommand).TryRaiseCanExecuteChanged();
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