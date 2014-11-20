namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Validation;

    public abstract class NumericBox<T>
        : BaseUpDown, INumericBox
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Direct,
            typeof(ValueChangedEventHandler<T>),
            typeof(NumericBox<T>));

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
            typeof(int?),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                OnDecimalsValueChanged,
                OnCoerceDecimalsValueChanged));

        private readonly Func<T, T, T> _add;
        private readonly Func<T, T, T> _subtract;
        private static readonly T TypeMin = (T)typeof(T).GetField("MinValue").GetValue(null);
        private static readonly T TypeMax = (T)typeof(T).GetField("MaxValue").GetValue(null);
        private readonly Validator<T> _validator;

        protected static void UpdateMetadata(Type type, T increment)
        {
            TextProperty.OverrideMetadata(type, new FrameworkPropertyMetadata("0", FrameworkPropertyMetadataOptions.NotDataBindable));
            IsReadOnlyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(OnIsReadOnlyChanged));

            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            IncrementProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
            MaxValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(TypeMax));
            MinValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(TypeMin));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="add">How to add two values (x, y) => x + y</param>
        /// <param name="subtract">How to subtract two values (x, y) => x - y</param>
        protected NumericBox(Func<T, T, T> add, Func<T, T, T> subtract)
        {
            _add = add;
            _subtract = subtract;
            _validator = new Validator<T>(
                this,
                new DataErrorValidationRule(),
                new ExceptionValidationRule(),
                new CanParse<T>(CanParse),
                new IsGreaterThan<T>(Parse, () => MinValue),
                new IsLessThan<T>(Parse, () => MaxValue));
        }

        [Category("NumericBox"), Browsable(true)]
        public event ValueChangedEventHandler<T> ValueChanged
        {
            add
            {
                AddHandler(ValueChangedEvent, value);
            }
            remove
            {
                RemoveHandler(ValueChangedEvent, value);
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public T Value
        {
            get
            {
                return (T)GetValue(ValueProperty);
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }


        IFormattable INumericBox.Value
        {
            get
            {
                return Value;
            }
        }


        [Description(""), Category("NumericBox"), Browsable(true)]
        public T MaxValue
        {
            get
            {
                return (T)GetValue(MaxValueProperty);
            }
            set
            {
                SetValue(MaxValueProperty, value);
            }
        }

        IFormattable INumericBox.MaxValue
        {
            get
            {
                return MaxValue;
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public T MinValue
        {
            get
            {
                return (T)GetValue(MinValueProperty);
            }
            set
            {
                SetValue(MinValueProperty, value);
            }
        }

        IFormattable INumericBox.MinValue
        {
            get
            {
                return MinValue;
            }
        }

        [Description(""), Category("NumericBox"), Browsable(true)]
        public T Increment
        {
            get
            {
                return (T)GetValue(IncrementProperty);
            }
            set
            {
                SetValue(IncrementProperty, value);
            }
        }

        IFormattable INumericBox.Increment
        {
            get
            {
                return Increment;
            }
        }

        [Browsable(true)]
        public int? Decimals
        {
            get
            {
                return (int)GetValue(DecimalsProperty);
            }
            set
            {
                SetValue(DecimalsProperty, value);
            }
        }

        public CultureInfo Culture
        {
            get { return Language.GetEquivalentCulture(); }
        }

        protected abstract bool CanParse(string s, IFormatProvider provider);

        protected abstract T Parse(string s, IFormatProvider provider);

        protected virtual void OnIncrementChanged()
        {
            CheckSpinners();
        }

        protected virtual void OnValueChanged(object newValue, object oldValue)
        {
            if (newValue != oldValue)
            {
                var args = new ValueChangedEventArgs<T>((T)oldValue, (T)newValue, ValueChangedEvent, this);
                RaiseEvent(args);
                CheckSpinners();
            }
        }

        protected virtual void OnMinValueChanged(object newValue, object oldValue)
        {
            CheckSpinners();
        }

        protected virtual void OnMaxValueChanged(object newValue, object oldValue)
        {
            CheckSpinners();
        }

        [Obsolete("Remove")]
        protected virtual T OnCoerceValueChanged(T value)
        {
            return value;
        }

        protected virtual object OnCoerceDecimalsValueChanged(object value)
        {
            var decimals = (int?)value;

            if (decimals == null || decimals < 0)
            {
                return null;
            }
            else
            {
                return value;
            }
        }

        protected override bool CanIncrease()
        {
            if (Comparer<T>.Default.Compare(Value, MaxValue) >= 0)
            {
                return false;
            }
            if (!CanParse(Text, Culture))
            {
                return false;
            }
            return base.CanIncrease();
        }

        protected override void Increase()
        {
            if (CanIncrease())
            {
                var value = AddIncrement();
                Value = value;
            }
        }

        protected override bool CanDecrease()
        {
            if (Comparer<T>.Default.Compare(Value, MinValue) <= 0)
            {
                return false;
            }
            if (!CanParse(Text, Culture))
            {
                return false;
            }
            return base.CanDecrease();
        }

        protected override void Decrease()
        {
            if (CanDecrease())
            {
                var value = SubtractIncrement();
                Value = value;
            }
        }

        protected virtual T ValidateValue(T value)
        {
            if (Comparer<T>.Default.Compare(value, MaxValue) == 1)
            {
                return MaxValue;
            }

            if (Comparer<T>.Default.Compare(value, MinValue) == -1)
            {
                return MinValue;
            }

            return value;
        }

        private static void OnDecimalsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var numericBox = (NumericBox<T>)d;
            if (e.NewValue == null)
            {
                numericBox.StringFormat = "R";
            }
            else
            {
                numericBox.StringFormat = "F" + e.NewValue;
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

        private static void OnIsReadOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var baseUpDown = (NumericBox<T>)d;
            baseUpDown.CheckSpinners();
        }

        private T AddIncrement()
        {
            var min = Comparer<T>.Default.Compare(MaxValue, TypeMax) < 0
                ? MaxValue
                : TypeMax;
            var subtract = _subtract(min, Increment);
            return Value.CompareTo(subtract) < 0
                            ? _add(Value, Increment)
                            : min;
        }

        private T SubtractIncrement()
        {
            var max = MinValue.CompareTo(TypeMin) > 0
                                ? MinValue
                                : TypeMin;
            var add = _add(max, Increment);
            return Value.CompareTo(add) > 0
                            ? _subtract(Value, Increment)
                            : max;
        }
    }
}