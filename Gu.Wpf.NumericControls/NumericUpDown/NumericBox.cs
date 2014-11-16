namespace Gu.Wpf.NumericControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [TemplatePart(Name = "PART_Text", Type = typeof(TextBox)),
     ToolboxItem(false)]
    public abstract class NumericBox<T>
        : BaseUpDown
        where T : struct, IComparable<T>
    {
        private readonly Func<T, T, T> _add;
        private readonly Func<T, T, T> _subtract;
        private readonly T _typeMin;
        private readonly T _typeMax;

        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventArgs),
            typeof(NumericBox<T>));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                OnValueChanged,
                OnCoerceValueChanged,
                false,
                UpdateSourceTrigger.LostFocus));

        public static readonly DependencyProperty IncrementProperty = DependencyProperty.Register(
            "Increment",
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None,
                OnIncrementChanged));

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None,
                OnMaxValueChanged));

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None,
                OnMinValueChanged));

        public static readonly DependencyProperty DecimalsProperty = DependencyProperty.Register(
            "Decimals",
            typeof(int),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                2,
                FrameworkPropertyMetadataOptions.None,
                OnDecimalsValueChanged,
                OnCoerceDecimalsValueChanged));

        [Category("NumericBox"), Browsable(true)]
        public event RoutedEventHandler ValueChanged
        {
            add
            {
                this.AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                this.RemoveHandler(ValueChangedEvent, value);
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public T Value
        {
            get
            {
                return (T)this.GetValue(ValueProperty);
            }
            set
            {
                this.SetValue(ValueProperty, value);
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public T MaxValue
        {
            get
            {
                return (T)this.GetValue(MaxValueProperty);
            }
            set
            {
                this.SetValue(MaxValueProperty, value);
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public T MinValue
        {
            get
            {
                return (T)this.GetValue(MinValueProperty);
            }
            set
            {
                this.SetValue(MinValueProperty, value);
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public T Increment
        {
            get
            {
                return (T)this.GetValue(IncrementProperty);
            }
            set
            {
                this.SetValue(IncrementProperty, value);
            }
        }

        [Browsable(true)]
        public int Decimals
        {
            get
            {
                return (int)this.GetValue(DecimalsProperty);
            }
            set
            {
                this.SetValue(DecimalsProperty, value);
            }
        }

        protected static void UpdateMetadata(Type type, T increment, T minValue, T maxValue, int decimals)
        {
            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            IncrementProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
            MaxValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(maxValue));
            MinValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(minValue));
            DecimalsProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(decimals));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="add">How to add two values (x, y) => x + y</param>
        /// <param name="subtract">How to subtract two values (x, y) => x - y</param>
        /// <param name="typeMin">Ex: Int.MinValue</param>
        /// <param name="typeMax">Ex: Int.MaxValue</param>
        protected NumericBox(Func<T, T, T> add, Func<T, T, T> subtract, T typeMin, T typeMax)
        {
            _add = add;
            _subtract = subtract;
            _typeMin = typeMin;
            _typeMax = typeMax;
        }

        protected virtual void OnIncrementChanged()
        {
            this.CheckSpinners();
        }

        protected virtual void OnValueChanged(object newValue, object oldValue)
        {
            if (newValue != oldValue)
            {
                RoutedEventArgs newEventArgs = new RoutedEventArgs(ValueChangedEvent);
                newEventArgs.RoutedEvent = ValueChangedEvent;
                this.RaiseEvent(newEventArgs);

                if (this.HasBeenInitialized)
                {
                    this.ConvertValueToText();
                }

                this.CheckSpinners();
            }
        }

        protected virtual void OnDecimalsValueChanged(object newValue, object oldvalue)
        {
            if (this.HasBeenInitialized)
            {
                this.ConvertValueToText();
            }
        }

        protected virtual void OnMinValueChanged(object newValue, object oldValue)
        {
            this.CheckSpinners();
        }

        protected virtual void OnMaxValueChanged(object newValue, object oldValue)
        {
            this.CheckSpinners();
        }

        protected virtual T OnCoerceValueChanged(T value)
        {
            value = this.ValidateValue(value);

            if (this.HasBeenInitialized)
            {
                this.ConvertValueToText();
            }

            return value;
        }

        protected virtual object OnCoerceDecimalsValueChanged(object value)
        {
            int decimals = (int)value;

            if (decimals < 0)
            {
                return "0";
            }
            else
            {
                return value;
            }
        }

        protected override void OnGotFocus(RoutedEventArgs e)
        {
            if (this.TextBox != null)
            {
                this.TextBox.Focus();
            }
            base.OnGotFocus(e);
        }

        protected override bool CanIncrease()
        {
            if (Comparer<T>.Default.Compare(this.Value, this.MaxValue) >= 0)
            {
                return false;
            }
            return base.CanIncrease();
        }

        protected override void Increase()
        {
            if (this.CanIncrease())
            {
                var value = AddIncrement();
                this.Value = value;
            }
        }

        protected override bool CanDecrease()
        {
            if (Comparer<T>.Default.Compare(this.Value, this.MinValue) <= 0)
            {
                return false;
            }
            return base.CanDecrease();
        }

        protected override void Decrease()
        {
            if (this.CanDecrease())
            {
                var value = SubtractIncrement();
                this.Value = value;
            }
        }

        protected virtual T ValidateValue(T value)
        {
            if (Comparer<T>.Default.Compare(value, this.MaxValue) == 1)
            {
                return this.MaxValue;
            }

            if (Comparer<T>.Default.Compare(value, this.MinValue) == -1)
            {
                return this.MinValue;
            }

            return value;
        }

        private static void OnDecimalsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = d as NumericBox<T>;
            if (numericBox != null)
            {
                numericBox.OnDecimalsValueChanged(e.NewValue, e.OldValue);
            }
        }

        private static object OnCoerceDecimalsValueChanged(DependencyObject d, object value)
        {
            var numericBox = d as NumericBox<T>;
            if (numericBox != null)
            {
                return numericBox.OnCoerceDecimalsValueChanged(value);
            }
            else
            {
                return value;
            }
        }

        private static object OnCoerceValueChanged(DependencyObject d, object value)
        {
            var numericBox = d as NumericBox<T>;
            if (numericBox != null)
            {
                return numericBox.OnCoerceValueChanged((T)value);
            }
            else
            {
                return value;
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = d as NumericBox<T>;
            if (numericBox != null)
            {
                numericBox.OnValueChanged(e.NewValue, e.OldValue);
                numericBox.CheckSpinners();
            }
        }

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = d as NumericBox<T>;
            if (numericBox != null)
            {
                numericBox.OnMaxValueChanged(e.NewValue, e.OldValue);
                numericBox.CheckSpinners();
            }
        }

        private static void OnIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = d as NumericBox<T>;
            if (numericBox != null)
            {
                numericBox.OnIncrementChanged();
                numericBox.CheckSpinners();
            }
        }

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = d as NumericBox<T>;
            if (numericBox != null)
            {
                numericBox.OnMinValueChanged(e.NewValue, e.OldValue);
                numericBox.CheckSpinners();
            }
        }

        private T AddIncrement()
        {
            var min = Comparer<T>.Default.Compare(MaxValue, _typeMax) < 0
                ? MaxValue
                : _typeMax;
            var subtract = _subtract(min, Increment);
            return Value.CompareTo(subtract) < 0
                            ? _add(Value, Increment)
                            : min;
        }

        private T SubtractIncrement()
        {
            var max = MinValue.CompareTo(_typeMin) > 0
                                ? MinValue
                                : _typeMin;
            var add = _add(max, Increment);
            return Value.CompareTo(add) > 0
                            ? _subtract(Value, Increment)
                            : max;
        }
    }
}