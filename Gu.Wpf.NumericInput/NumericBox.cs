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

    public abstract class NumericBox<T>
        : BaseUpDown
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
            this._add = add;
            this._subtract = subtract;
            this._validator = new Validator<T>(
                this,
                new DataErrorValidationRule(),
                new ExceptionValidationRule(),
                new CanParse<T>(this.CanParse),
                new IsGreaterThan<T>(this.Parse, () => this.MinValue),
                new IsLessThan<T>(this.Parse, () => this.MaxValue));
        }

        [Category("NumericBox"), Browsable(true)]
        public event ValueChangedEventHandler<T> ValueChanged
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
        public int? Decimals
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

        private CultureInfo Culture
        {
            get { return this.Language.GetEquivalentCulture(); }
        }

        protected abstract bool CanParse(string s, IFormatProvider provider);

        protected abstract T Parse(string s, IFormatProvider provider);

        protected virtual void OnIncrementChanged()
        {
            this.CheckSpinners();
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

        [Obsolete("Fix this")]
        protected virtual void OnDecimalsValueChanged(object newValue, object oldvalue)
        {
        }

        protected virtual void OnMinValueChanged(object newValue, object oldValue)
        {
            this.CheckSpinners();
        }

        protected virtual void OnMaxValueChanged(object newValue, object oldValue)
        {
            this.CheckSpinners();
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
            if (Comparer<T>.Default.Compare(this.Value, this.MaxValue) >= 0)
            {
                return false;
            }
            if (!this.CanParse(this.Text, this.Culture))
            {
                return false;
            }
            return base.CanIncrease();
        }

        protected override void Increase()
        {
            if (this.CanIncrease())
            {
                var value = this.AddIncrement();
                this.Value = value;
            }
        }

        protected override bool CanDecrease()
        {
            if (Comparer<T>.Default.Compare(this.Value, this.MinValue) <= 0)
            {
                return false;
            }
            if (!this.CanParse(this.Text, this.Culture))
            {
                return false;
            }
            return base.CanDecrease();
        }

        protected override void Decrease()
        {
            if (this.CanDecrease())
            {
                var value = this.SubtractIncrement();
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
            var min = Comparer<T>.Default.Compare(this.MaxValue, TypeMax) < 0
                ? this.MaxValue
                : TypeMax;
            var subtract = this._subtract(min, this.Increment);
            return this.Value.CompareTo(subtract) < 0
                            ? this._add(this.Value, this.Increment)
                            : min;
        }

        private T SubtractIncrement()
        {
            var max = this.MinValue.CompareTo(TypeMin) > 0
                                ? this.MinValue
                                : TypeMin;
            var add = this._add(max, this.Increment);
            return this.Value.CompareTo(add) > 0
                            ? this._subtract(this.Value, this.Increment)
                            : max;
        }
    }
}