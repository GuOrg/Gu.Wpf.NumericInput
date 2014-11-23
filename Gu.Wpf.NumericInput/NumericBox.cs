namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    using Validation;

    /// <summary>
    /// Baseclass with common functionality for numeric textboxes
    /// </summary>
    /// <typeparam name="T"></typeparam>
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
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None,
                (o, e) => ((NumericBox<T>)o).CheckSpinners()));

        /// <summary>
        /// Identifies the MinValue property
        /// </summary>
        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(T),
            typeof(NumericBox<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None,
                (o, e) => ((NumericBox<T>)o).CheckSpinners()));

        private readonly Func<T, T, T> _add;
        private readonly Func<T, T, T> _subtract;
        private static readonly T TypeMin = (T)typeof(T).GetField("MinValue").GetValue(null);
        private static readonly T TypeMax = (T)typeof(T).GetField("MaxValue").GetValue(null);
        private readonly Validator<T> _validator;

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
                new CanParse<T>(this.CanParse),
                new IsMatch(() => RegexPattern),
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

        /// <summary>
        /// The current value Parse(Text) will throw if bad format
        /// </summary>
        internal T CurrentValue
        {
            get { return Parse(Text); }
        }

        /// <summary>
        /// Value.ToString(StringFormat, Culture)
        /// </summary>
        public string FormattedText
        {
            get { return Value.ToString(StringFormat, Culture); }
        }

        public abstract bool CanParse(string s);

        public abstract T Parse(string s);

        IFormattable INumericBox.Parse(string s)
        {
            return Parse(s);
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
            MaxValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(TypeMax));
            MinValueProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(TypeMin));
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

        protected virtual object OnCoerceDecimalsValue(object value)
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

        protected override bool CanIncrease(object parameter)
        {
            if (!CanParse(Text))
            {
                return false;
            }
            if (Comparer<T>.Default.Compare(CurrentValue, MaxValue) >= 0)
            {
                return false;
            }
            return base.CanIncrease(parameter);
        }

        protected override void Increase(object parameter)
        {
            if (CanIncrease(parameter))
            {
                var value = AddIncrement();
                var text = value.ToString(StringFormat, Culture);

                var textBox = parameter as TextBox;
                if (textBox != null)
                {
                    // http://stackoverflow.com/questions/27083236/change-the-text-in-a-textbox-with-text-binding-sometext-so-it-is-undoable/27083548?noredirect=1#comment42677255_27083548
                    // Dunno if nice, testing it for now
                    textBox.SelectAll();
                    textBox.SelectedText = text;
                    textBox.Select(0, 0);
                }
                else
                {
                    Text = text;
                }
            }
        }

        protected override bool CanDecrease(object parameter)
        {
            if (!CanParse(Text))
            {
                return false;
            }
            if (Comparer<T>.Default.Compare(CurrentValue, MinValue) <= 0)
            {
                return false;
            }
            return base.CanDecrease(parameter);
        }

        protected override void Decrease(object parameter)
        {
            if (CanDecrease(parameter))
            {
                var value = SubtractIncrement();
                var text = value.ToString(StringFormat, Culture);

                var textBox = parameter as TextBox;
                if (textBox != null)
                {
                    // http://stackoverflow.com/questions/27083236/change-the-text-in-a-textbox-with-text-binding-sometext-so-it-is-undoable/27083548?noredirect=1#comment42677255_27083548
                    // Dunno if nice, testing it for now
                    textBox.SelectAll();
                    textBox.SelectedText = text;
                    textBox.Select(0, 0);
                }
                else
                {
                    Text = text;
                }
            }
        }

        protected static void OnDecimalsValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
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

        protected static object OnCoerceDecimalsValue(DependencyObject d, object value)
        {
            var numericBox = d as NumericBox<T>;
            if (numericBox != null)
            {
                return numericBox.OnCoerceDecimalsValue(value);
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

        private T AddIncrement()
        {
            var min = Comparer<T>.Default.Compare(MaxValue, TypeMax) < 0
                ? MaxValue
                : TypeMax;
            var subtract = _subtract(min, Increment);
            var currentValue = CurrentValue;
            return currentValue.CompareTo(subtract) < 0
                            ? _add(currentValue, Increment)
                            : min;
        }

        private T SubtractIncrement()
        {
            var max = MinValue.CompareTo(TypeMin) > 0
                                ? MinValue
                                : TypeMin;
            var add = _add(max, Increment);
            var currentValue = CurrentValue;
            return currentValue.CompareTo(add) > 0
                            ? _subtract(currentValue, Increment)
                            : max;
        }
    }
}