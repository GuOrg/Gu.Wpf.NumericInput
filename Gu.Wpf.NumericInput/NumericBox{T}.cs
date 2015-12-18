namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.NumericInput.Validation;

    /// <summary>
    /// Baseclass with common functionality for numeric textboxes
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> property</typeparam>
    public abstract class NumericBox<T>
        : BaseBox, INumericBox
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        /// <summary>
        /// Identifies the ValueChanged event
        /// </summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Direct,
            typeof(ValueChangedEventHandler<T>),
            typeof(NumericBox<T>));

        /// <summary>
        /// Identifies the Value property
        /// </summary>
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            {
                DefaultUpdateSourceTrigger = UpdateSourceTrigger.LostFocus,
                BindsTwoWayByDefault = true,
                PropertyChangedCallback = OnValueChanged,
            });

        /// <summary>
        /// Identifies the Increment property
        /// </summary>
        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment",
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None));

        /// <summary>
        /// Identifies the MaxValue property
        /// </summary>
        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(T?),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                OnMaxValueChanged));

        /// <summary>
        /// Identifies the MinValue property
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(T?),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                OnMinValueChanged));

        public static readonly DependencyProperty NumberStylesProperty = DependencyProperty.Register(
            "NumberStyles",
            typeof(NumberStyles),
            typeof(NumericBox<T>),
            new PropertyMetadata(NumberStyles.Any));

        private static readonly T TypeMin = (T)typeof(T).GetField("MinValue").GetValue(null);
        private static readonly T TypeMax = (T)typeof(T).GetField("MaxValue").GetValue(null);
        private readonly Func<T, T, T> add;
        private readonly Func<T, T, T> subtract;
        private readonly Validator<T> validator; // Keep this alive

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericBox{T}"/> class.
        /// </summary>
        /// <param name="add">How to add two values (x, y) => x + y</param>
        /// <param name="subtract">How to subtract two values (x, y) => x - y</param>
        protected NumericBox(Func<T, T, T> add, Func<T, T, T> subtract)
        {
            this.add = add;
            this.subtract = subtract;
            this.validator = new Validator<T>(
                this,
                new DataErrorValidationRule(),
                new ExceptionValidationRule(),
                new CanParse<T>(this.CanParse),
                new IsMatch(() => this.RegexPattern),
                new IsGreaterThan<T>(this.Parse, () => this.MinValue),
                new IsLessThan<T>(this.Parse, () => this.MaxValue));
            this.MaxLimit = TypeMax;
            this.MinLimit = TypeMin;
        }

        [Category("NumericBox")]
        [Browsable(true)]
        public event ValueChangedEventHandler<T> ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }

        [Description("")]
        [Category("NumericBox")]
        [Browsable(true)]
        public T Value
        {
            get { return (T)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }

        IFormattable INumericBox.Value => this.Value;

        [Description("")]
        [Category("NumericBox")]
        [Browsable(true)]
        public T? MaxValue
        {
            get { return (T?)this.GetValue(MaxValueProperty); }
            set { this.SetValue(MaxValueProperty, value); }
        }

        IFormattable INumericBox.MaxValue => this.MaxValue;

        [Description("")]
        [Category("NumericBox")]
        [Browsable(true)]
        public T? MinValue
        {
            get { return (T?)this.GetValue(MinValueProperty); }
            set { this.SetValue(MinValueProperty, value); }
        }

        IFormattable INumericBox.MinValue => this.MinValue;

        [Description("")]
        [Category("NumericBox")]
        [Browsable(true)]
        public T Increment
        {
            get { return (T)this.GetValue(IncrementProperty); }
            set { this.SetValue(IncrementProperty, value); }
        }

        IFormattable INumericBox.Increment => this.Increment;

        /// <summary>
        /// Gets or sets the number styles used for validation
        /// </summary>
        public NumberStyles NumberStyles
        {
            get { return (NumberStyles)this.GetValue(NumberStylesProperty); }
            set { this.SetValue(NumberStylesProperty, value); }
        }

        /// <summary>
        /// Gets the current value. Will throw if bad format
        /// </summary>
        internal T CurrentValue => this.Parse(this.Text);

        internal T MaxLimit { get; private set; }

        internal T MinLimit { get; private set; }

        public abstract bool CanParse(string text);

        public abstract T Parse(string text);

        IFormattable INumericBox.Parse(string text)
        {
            return this.Parse(text);
        }

        protected static void UpdateMetadata(Type type, T increment)
        {
            TextProperty.OverrideMetadata(
                type, new FrameworkPropertyMetadata(
                    "0",
                    FrameworkPropertyMetadataOptions.NotDataBindable,
                    (o, e) => ((NumericBox<T>)o).CheckSpinners()));
            IsReadOnlyProperty.OverrideMetadata(
                type,
                new FrameworkPropertyMetadata(
                    (o, e) => ((NumericBox<T>)o).CheckSpinners()));

            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            IncrementProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
        }

        protected static void OnDecimalsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = (NumericBox<T>)d;
            numericBox.StringFormat = (string)DecimalDigitsToStringFormatConverter.Default.Convert(e.NewValue, null, null, null);

            // not sure if binding StringFormat to DecimalDigits is nicer
        }

        protected virtual void OnValueChanged(object newValue, object oldValue)
        {
            if (newValue != oldValue)
            {
                var args = new ValueChangedEventArgs<T>((T)oldValue, (T)newValue, ValueChangedEvent, this);
                this.RaiseEvent(args);
                this.CheckSpinners();
            }
        }

        protected override bool CanIncrease(object parameter)
        {
            if (!this.CanParse(this.Text))
            {
                return false;
            }

            if (Comparer<T>.Default.Compare(this.CurrentValue, this.MaxLimit) >= 0)
            {
                return false;
            }

            return base.CanIncrease(parameter);
        }

        protected override void Increase(object parameter)
        {
            if (this.CanIncrease(parameter))
            {
                var value = this.AddIncrement();
                var text = value.ToString(this.StringFormat, this.Culture);

                var textBox = parameter as TextBox;
                SetTextUndoable(textBox ?? this, text);
            }
        }

        protected override bool CanDecrease(object parameter)
        {
            if (!this.CanParse(this.Text))
            {
                return false;
            }

            if (Comparer<T>.Default.Compare(this.CurrentValue, this.MinLimit) <= 0)
            {
                return false;
            }

            return base.CanDecrease(parameter);
        }

        protected override void Decrease(object parameter)
        {
            if (this.CanDecrease(parameter))
            {
                var value = this.SubtractIncrement();
                var text = value.ToString(this.StringFormat, this.Culture);

                var textBox = parameter as TextBox;
                SetTextUndoable(textBox ?? this, text);
            }
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

        private static void SetTextUndoable(TextBox textBox, string text)
        {
            // http://stackoverflow.com/questions/27083236/change-the-text-in-a-textbox-with-text-binding-sometext-so-it-is-undoable/27083548?noredirect=1#comment42677255_27083548
            // Dunno if nice, testing it for now
            textBox.SelectAll();
            textBox.SelectedText = text;
            textBox.Select(0, 0);
        }

        private T AddIncrement()
        {
            var min = this.MaxLimit.CompareTo(TypeMax) < 0
                ? this.MaxLimit
                : TypeMax;
            var incremented = this.subtract(min, this.Increment);
            var currentValue = this.CurrentValue;
            return currentValue.CompareTo(incremented) < 0
                            ? this.add(currentValue, this.Increment)
                            : min;
        }

        private T SubtractIncrement()
        {
            var max = this.MinLimit.CompareTo(TypeMin) > 0
                                ? this.MinLimit
                                : TypeMin;
            var incremented = this.add(max, this.Increment);
            var currentValue = this.CurrentValue;
            return currentValue.CompareTo(incremented) > 0
                            ? this.subtract(currentValue, this.Increment)
                            : max;
        }
    }
}