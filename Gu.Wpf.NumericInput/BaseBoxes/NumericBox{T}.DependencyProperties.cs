namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Data;

    public abstract partial class NumericBox<T>
    {
        /// <summary>
        /// Identifies the Value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged,
                null,
                false,
                UpdateSourceTrigger.LostFocus));

        /// <summary>
        /// Identifies the MinValue property
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(T?),
            typeof(NumericBox<T>),
            new PropertyMetadata(
                null,
                OnMinValueChanged));

        /// <summary>
        /// Identifies the MaxValue property
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(T?),
            typeof(NumericBox<T>),
            new PropertyMetadata(
                null,
                OnMaxValueChanged));

        public static readonly DependencyProperty NumberStylesProperty = DependencyProperty.Register(
            "NumberStyles",
            typeof(NumberStyles),
            typeof(NumericBox<T>),
            new PropertyMetadata(NumberStyles.Any));

        /// <summary>
        /// Identifies the Increment property
        /// </summary>
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment",
            typeof(T?),
            typeof(NumericBox<T>),
            new PropertyMetadata(
                null,
                OnIncrementChanged));

        [Category("NumericBox")]
        [Browsable(true)]
        public T Value
        {
            get { return (T)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        [Category("NumericBox")]
        [Browsable(true)]
        public T? MinValue
        {
            get { return (T?)this.GetValue(MinValueProperty); }
            set { this.SetValue(MinValueProperty, value); }
        }

        [Category("NumericBox")]
        [Browsable(true)]
        public T? MaxValue
        {
            get { return (T?)this.GetValue(MaxValueProperty); }
            set { this.SetValue(MaxValueProperty, value); }
        }

        /// <summary>
        /// Gets or sets the number styles used for validation
        /// </summary>
        public NumberStyles NumberStyles
        {
            get { return (NumberStyles)this.GetValue(NumberStylesProperty); }
            set { this.SetValue(NumberStylesProperty, value); }
        }

        [Category("NumericBox")]
        [Browsable(true)]
        public T? Increment
        {
            get { return (T?)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = (NumericBox<T>)d;
            if (!Equals(e.NewValue, e.OldValue))
            {
                numericBox.OnValueChanged(e.NewValue, e.OldValue);
                numericBox.CheckSpinners();
            }
        }

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.CheckSpinners();
            var newMax = e.NewValue as T?;
            box.MaxLimit = newMax ?? TypeMax;
        }

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.CheckSpinners();
            var newMin = e.NewValue as T?;
            box.MinLimit = newMin ?? TypeMin;
        }

        private static void OnIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.CheckSpinners();
        }
    }
}
