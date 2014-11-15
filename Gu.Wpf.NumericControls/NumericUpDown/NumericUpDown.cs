namespace Gu.Wpf.NumericControls
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    [TemplatePart(Name = "PART_Text", Type = typeof(TextBox)), TemplatePart(Name = "PART_Up", Type = typeof(Button)),
     TemplatePart(Name = "PART_Down", Type = typeof(Button)), ToolboxItem(false)]
    public abstract class NumericUpDown<T> : BaseUpDown
        where T : struct, IComparable<T>
    {
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Direct,
            typeof(RoutedEventArgs),
            typeof(NumericUpDown<T>));

        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value",
            typeof(T),
            typeof(NumericUpDown<T>),
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
            typeof(NumericUpDown<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None,
                OnIncrementChanged));

        public static readonly DependencyProperty MaxValueProperty = DependencyProperty.Register(
            "MaxValue",
            typeof(T),
            typeof(NumericUpDown<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None,
                OnMaxValueChanged));

        public static readonly DependencyProperty MinValueProperty = DependencyProperty.Register(
            "MinValue",
            typeof(T),
            typeof(NumericUpDown<T>),
            new FrameworkPropertyMetadata(
                default(T),
                FrameworkPropertyMetadataOptions.None,
                OnMinValueChanged));

        public static readonly DependencyProperty DecimalsProperty = DependencyProperty.Register(
            "Decimals",
            typeof(int),
            typeof(NumericUpDown<T>),
            new FrameworkPropertyMetadata(
                2,
                FrameworkPropertyMetadataOptions.None,
                OnDecimalsValueChanged,
                OnCoerceDecimalsValueChanged));

        [Category("NumericUpDown"), Browsable(true)]
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

        [Description(""), Category("NumericUpDown"), Browsable(true)]
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

        [Description(""), Category("NumericUpDown"), Browsable(true)]
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

        [Description(""), Category("NumericUpDown"), Browsable(true)]
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

        [Description(""), Category("NumericUpDown"), Browsable(true)]
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

        protected virtual void OnAllowSpinnersChanged()
        {
            if (this.HasBeenInitialized)
            {
                this.CheckSpinners();
            }
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

        protected override bool CanDecrease()
        {
            if (Comparer<T>.Default.Compare(this.Value, this.MinValue) <= 0)
            {
                return false;
            }
            return base.CanDecrease();
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
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnDecimalsValueChanged(e.NewValue, e.OldValue);
            }
        }

        private static object OnCoerceDecimalsValueChanged(DependencyObject d, object value)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                return numericUpDown.OnCoerceDecimalsValueChanged(value);
            }
            else
            {
                return value;
            }
        }

        private static object OnCoerceValueChanged(DependencyObject d, object value)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                return numericUpDown.OnCoerceValueChanged((T)value);
            }
            else
            {
                return value;
            }
        }

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnValueChanged(e.NewValue, e.OldValue);
                numericUpDown.CheckSpinners();
            }
        }

        private static void OnMaxValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnMaxValueChanged(e.NewValue, e.OldValue);
                numericUpDown.CheckSpinners();
            }
        }

        private static void OnIncrementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnIncrementChanged();
                numericUpDown.CheckSpinners();
            }
        }

        private static void OnMinValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            NumericUpDown<T> numericUpDown = d as NumericUpDown<T>;
            if (numericUpDown != null)
            {
                numericUpDown.OnMinValueChanged(e.NewValue, e.OldValue);
                numericUpDown.CheckSpinners();
            }
        }
    }
}