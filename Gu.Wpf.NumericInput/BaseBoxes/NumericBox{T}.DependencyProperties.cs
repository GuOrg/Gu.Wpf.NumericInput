namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public abstract partial class NumericBox<T>
    {
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(T?),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged,
                null,
                false,
                UpdateSourceTrigger.LostFocus));

        public static readonly DependencyProperty CanValueBeNullProperty = DependencyProperty.Register(
            "CanValueBeNull",
            typeof(bool),
            typeof(NumericBox<T>),
            new PropertyMetadata(default(bool), OnCanBeNullChanged));

        public static readonly DependencyProperty NumberStylesProperty = DependencyProperty.Register(
            "NumberStyles",
            typeof(NumberStyles),
            typeof(NumericBox<T>),
            new PropertyMetadata(NumberStyles.None, OnNumberStylesChanged));

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(T?),
            typeof(NumericBox<T>),
            new PropertyMetadata(
                null,
                OnMinValueChanged));

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(T?),
            typeof(NumericBox<T>),
            new PropertyMetadata(
                null,
                OnMaxValueChanged));

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment",
            typeof(T),
            typeof(NumericBox<T>),
            new PropertyMetadata(
                default(T),
                OnIncrementChanged));

        static NumericBox()
        {
            var metadata = TextProperty.GetMetadata(typeof(TextBox));
            TextProperty.OverrideMetadata(
                typeof(NumericBox<T>),
                new FrameworkPropertyMetadata(
                    metadata.DefaultValue,
                    FrameworkPropertyMetadataOptions.NotDataBindable | FrameworkPropertyMetadataOptions.Journal,
                    metadata.PropertyChangedCallback,
                    metadata.CoerceValueCallback,
                    true,
                    UpdateSourceTrigger.LostFocus));
        }

        [Category("NumericBox")]
        [Browsable(true)]
        public T? Value
        {
            get { return (T?)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        [Category("NumericBox")]
        [Browsable(true)]
        public bool CanValueBeNull
        {
            get { return (bool) this.GetValue(CanValueBeNullProperty); }
            set { this.SetValue(CanValueBeNullProperty, value); }
        }

        [Category("NumericBox")]
        [Browsable(true)]
        public NumberStyles NumberStyles
        {
            get { return (NumberStyles)this.GetValue(NumberStylesProperty); }
            set { this.SetValue(NumberStylesProperty, value); }
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

        [Category("NumericBox")]
        [Browsable(true)]
        public T Increment
        {
            get { return (T)this.GetValue(IncrementProperty); }
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

        private static void OnCanBeNullChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.RaiseEvent(ValidationDirtyEventArgs);
        }

        private static void OnNumberStylesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.RaiseEvent(ValidationDirtyEventArgs);
        }

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.CheckSpinners();
            box.RaiseEvent(ValidationDirtyEventArgs);
        }

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.CheckSpinners();
            box.RaiseEvent(ValidationDirtyEventArgs);
        }

        private static void OnIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.CheckSpinners();
        }
    }
}
