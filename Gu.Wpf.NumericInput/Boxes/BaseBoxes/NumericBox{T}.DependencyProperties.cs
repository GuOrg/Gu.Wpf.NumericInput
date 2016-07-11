namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>DependencyProperties for <see cref="NumericBox{T}"/>.</summary>
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

        public static readonly DependencyProperty AllowSpinnersProperty = NumericBox.AllowSpinnersProperty.AddOwner(
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits,
                OnAllowSpinnersChanged));

        private static readonly DependencyPropertyKey IncreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "IncreaseCommand",
            typeof(ICommand),
            typeof(NumericBox<T>),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IncreaseCommandProperty = IncreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey DecreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            "DecreaseCommand",
            typeof(ICommand),
            typeof(NumericBox<T>),
            new PropertyMetadata(null));

        public static readonly DependencyProperty DecreaseCommandProperty = DecreaseCommandPropertyKey.DependencyProperty;

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
            TextValueConverterProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<T>), TextValueConverter<T>.Default);
            var validationRules = new ValidationRule[]
            {
                CanParse<T>.FromText,
                RegexRule.FromText,
                IsGreaterThanOrEqualToMinRule<T>.FromText,
                IsLessThanOrEqualToMaxRule<T>.FromText,
            };

            ValidationRulesProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<T>), validationRules);
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
            numericBox.OnValueChanged((T?)e.OldValue, (T?)e.NewValue);
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

        private static void OnAllowSpinnersChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            (box.IncreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
            (box.DecreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
        }

        private static void OnIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var box = (NumericBox<T>)d;
            box.CheckSpinners();
        }
    }
}
