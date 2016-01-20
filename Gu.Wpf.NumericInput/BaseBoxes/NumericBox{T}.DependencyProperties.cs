namespace Gu.Wpf.NumericInput
{
    using System;
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

        public static readonly DependencyProperty CanValueBeNullProperty = NumericBox.CanValueBeNullProperty.AddOwner(
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(BooleanBoxes.False, FrameworkPropertyMetadataOptions.Inherits, OnCanBeNullChanged));

        public static readonly DependencyProperty NumberStylesProperty = NumericBox.NumberStylesProperty.AddOwner(
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(NumberStyles.None, FrameworkPropertyMetadataOptions.Inherits, OnNumberStylesChanged));

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

        private static readonly EventHandler<ValidationErrorEventArgs> ValidationErrorHandler = OnValidationError;
        private static readonly RoutedEventHandler FormatDirtyHandler = OnFormatDirty;
        private static readonly RoutedEventHandler ValidationDirtyHandler = OnValidationDirty;

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
            EventManager.RegisterClassHandler(typeof(NumericBox<T>), Validation.ErrorEvent, ValidationErrorHandler);
            EventManager.RegisterClassHandler(typeof(NumericBox<T>), ValidationDirtyEvent, ValidationDirtyHandler);
            EventManager.RegisterClassHandler(typeof(NumericBox<T>), FormatDirtyEvent, FormatDirtyHandler);
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public T? Value
        {
            get { return (T?)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public bool CanValueBeNull
        {
            get { return (bool)this.GetValue(CanValueBeNullProperty); }
            set { this.SetValue(CanValueBeNullProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public NumberStyles NumberStyles
        {
            get { return (NumberStyles)this.GetValue(NumberStylesProperty); }
            set { this.SetValue(NumberStylesProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public T? MinValue
        {
            get { return (T?)this.GetValue(MinValueProperty); }
            set { this.SetValue(MinValueProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public T? MaxValue
        {
            get { return (T?)this.GetValue(MaxValueProperty); }
            set { this.SetValue(MaxValueProperty, value); }
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public T Increment
        {
            get { return (T)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
            var numericBox = (NumericBox<T>)d;
            if (numericBox.Status == NumericInput.Status.Idle)
            {
                numericBox.Status = NumericInput.Status.UpdatingFromValueBinding;
                numericBox.TextSource = TextSource.ValueBinding;
                var newValue = (T?)e.NewValue;
                var newRaw = newValue?.ToString(numericBox.Culture) ?? string.Empty;
                numericBox.SetTextClearUndo(newRaw);
                numericBox.SetCurrentValue(TextBindableProperty, newRaw);
                numericBox.FormattedText = numericBox.Format(newValue);
                numericBox.Status = Status.Idle;
            }

            numericBox.OnValueChanged(e.NewValue, e.OldValue);
        }

        private static void OnCanBeNullChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsValidationDirty = true;
            }
        }

        private static void OnNumberStylesChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            if (box.TextSource != TextSource.None)
            {
                box.IsValidationDirty = true;
            }
        }

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            if (box.TextSource != TextSource.None)
            {
                box.CheckSpinners();
                box.IsValidationDirty = true;
            }
        }

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            if (box.TextSource != TextSource.None)
            {
                box.CheckSpinners();
                box.IsValidationDirty = true;
            }
        }

        private static void OnIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.CheckSpinners();
        }
    }
}
