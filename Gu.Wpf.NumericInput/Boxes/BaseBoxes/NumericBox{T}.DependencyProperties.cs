// ReSharper disable StaticMemberInGenericType
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
        /// <summary>Identifies the <see cref="Value"/> dependency property.</summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            nameof(Value),
            typeof(T?),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                defaultValue: null,
                flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                propertyChangedCallback: OnValueChanged,
                coerceValueCallback: null,
                isAnimationProhibited: false,
                defaultUpdateSourceTrigger: UpdateSourceTrigger.LostFocus));

        /// <summary>Identifies the <see cref="CanValueBeNull"/> dependency property.</summary>
        public static readonly DependencyProperty CanValueBeNullProperty = NumericBox.CanValueBeNullProperty.AddOwner(
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(BooleanBoxes.False, FrameworkPropertyMetadataOptions.Inherits, OnCanValueBeNullChanged));

        /// <summary>Identifies the <see cref="NumberStyles"/> dependency property.</summary>
        public static readonly DependencyProperty NumberStylesProperty = NumericBox.NumberStylesProperty.AddOwner(
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(NumberStyles.None, FrameworkPropertyMetadataOptions.Inherits, OnNumberStylesChanged));

        /// <summary>Identifies the <see cref="MinValue"/> dependency property.</summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            nameof(MinValue),
            typeof(T?),
            typeof(NumericBox<T>),
            new PropertyMetadata(
                null,
                OnMinValueChanged));

        /// <summary>Identifies the <see cref="MaxValue"/> dependency property.</summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            nameof(MaxValue),
            typeof(T?),
            typeof(NumericBox<T>),
            new PropertyMetadata(
                null,
                OnMaxValueChanged));

        /// <summary>Identifies the <see cref="AllowSpinners"/> dependency property.</summary>
        public static readonly DependencyProperty AllowSpinnersProperty = NumericBox.AllowSpinnersProperty.AddOwner(
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                BooleanBoxes.False,
                FrameworkPropertyMetadataOptions.Inherits,
                OnAllowSpinnersChanged));

        /// <summary>Identifies the <see cref="SpinUpdateMode"/> dependency property.</summary>
        public static readonly DependencyProperty SpinUpdateModeProperty = NumericBox.SpinUpdateModeProperty.AddOwner(
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                SpinUpdateMode.AsBinding,
                FrameworkPropertyMetadataOptions.Inherits));

        private static readonly DependencyPropertyKey IncreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IncreaseCommand),
            typeof(ICommand),
            typeof(NumericBox<T>),
            new PropertyMetadata(null));

        /// <summary>Identifies the <see cref="IncreaseCommand"/> dependency property.</summary>
        public static readonly DependencyProperty IncreaseCommandProperty = IncreaseCommandPropertyKey.DependencyProperty;

        private static readonly DependencyPropertyKey DecreaseCommandPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(DecreaseCommand),
            typeof(ICommand),
            typeof(NumericBox<T>),
            new PropertyMetadata(null));

        /// <summary>Identifies the <see cref="DecreaseCommand"/> dependency property.</summary>
        public static readonly DependencyProperty DecreaseCommandProperty = DecreaseCommandPropertyKey.DependencyProperty;

        /// <summary>Identifies the <see cref="Increment"/> dependency property.</summary>
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            nameof(Increment),
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
                RegexValidationRule.FromText,
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
            get => (T?)this.GetValue(ValueProperty);
            set => this.SetValue(ValueProperty, value);
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public bool CanValueBeNull
        {
            get => (bool)this.GetValue(CanValueBeNullProperty);
            set => this.SetValue(CanValueBeNullProperty, value);
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public NumberStyles NumberStyles
        {
            get => (NumberStyles)this.GetValue(NumberStylesProperty);
            set => this.SetValue(NumberStylesProperty, value);
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public T? MinValue
        {
            get => (T?)this.GetValue(MinValueProperty);
            set => this.SetValue(MinValueProperty, value);
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public T? MaxValue
        {
            get => (T?)this.GetValue(MaxValueProperty);
            set => this.SetValue(MaxValueProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating whether spinners should be visible.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public bool AllowSpinners
        {
            get => (bool)this.GetValue(AllowSpinnersProperty);
            set => this.SetValue(AllowSpinnersProperty, value);
        }

        /// <summary>
        /// Gets or sets a value indicating how the IncreaseCommand and DecreaseCommand behaves.
        /// The default is AsBinding meaning the value updates using the UpdateSourceTrigger specified in the binding. Default is LostFocus.
        /// If set to PropertyChanged the binding source will be updated at each click even if the binding has UpdateSourceTrigger = LostFocus.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public SpinUpdateMode SpinUpdateMode
        {
            get => (SpinUpdateMode)this.GetValue(SpinUpdateModeProperty);
            set => this.SetValue(SpinUpdateModeProperty, value);
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public ICommand IncreaseCommand
        {
            get => (ICommand)this.GetValue(IncreaseCommandProperty);
            private set => this.SetValue(IncreaseCommandPropertyKey, value);
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public ICommand DecreaseCommand
        {
            get => (ICommand)this.GetValue(DecreaseCommandProperty);
            private set => this.SetValue(DecreaseCommandPropertyKey, value);
        }

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public T Increment
        {
            get => (T)this.GetValue(IncrementProperty);
            set => this.SetValue(IncrementProperty, value);
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Debug.WriteLine(e);
            var numericBox = (NumericBox<T>)d;
            numericBox.OnValueChanged((T?)e.OldValue, (T?)e.NewValue);
        }

        private static void OnCanValueBeNullChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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
