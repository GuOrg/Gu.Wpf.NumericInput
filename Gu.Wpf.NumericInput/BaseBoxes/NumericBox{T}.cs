namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using Gu.Wpf.NumericInput.Validation;

    /// <summary>
    /// Baseclass with common functionality for numeric textboxes
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> property</typeparam>
    public abstract partial class NumericBox<T>
        : BaseBox, INumericBox
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
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

        IFormattable INumericBox.Value => this.Value;

        IFormattable INumericBox.MaxValue => this.MaxValue;

        IFormattable INumericBox.MinValue => this.MinValue;

        IFormattable INumericBox.Increment => this.Increment;

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
            //TextProperty.OverrideMetadata(
            //    type, new FrameworkPropertyMetadata(
            //        "0",
            //        FrameworkPropertyMetadataOptions.NotDataBindable,
            //        (o, e) => ((NumericBox<T>)o).CheckSpinners()));

            //IsReadOnlyProperty.OverrideMetadata(
            //    type,
            //    new FrameworkPropertyMetadata(
            //        (o, e) => ((NumericBox<T>)o).CheckSpinners()));

            DefaultStyleKeyProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(type));
            IncrementProperty.OverrideMetadata(type, new FrameworkPropertyMetadata(increment));
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
            if (this.Increment == null)
            {
                return false;
            }

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
            if (this.Increment == null)
            {
                return false;
            }

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

        private static void SetTextUndoable(TextBox textBox, string text)
        {
            // http://stackoverflow.com/questions/27083236/change-the-text-in-a-textbox-with-text-binding-sometext-so-it-is-undoable/27083548?noredirect=1#comment42677255_27083548
            // Dunno if nice, testing it for now
            textBox.SelectAll();
            textBox.SelectedText = text;
            textBox.Select(0, 0);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == IsReadOnlyProperty)
            {
                this.CheckSpinners();
            }

            base.OnPropertyChanged(e);
        }

        protected override void OnTextChanged(TextChangedEventArgs e)
        {
            this.CheckSpinners();
            base.OnTextChanged(e);
        }

        private T AddIncrement()
        {
            var min = this.MaxLimit.CompareTo(TypeMax) < 0
                ? this.MaxLimit
                : TypeMax;
            var incremented = this.subtract(min, this.Increment.Value);
            var currentValue = this.CurrentValue;
            return currentValue.CompareTo(incremented) < 0
                            ? this.add(currentValue, this.Increment.Value)
                            : min;
        }

        private T SubtractIncrement()
        {
            var max = this.MinLimit.CompareTo(TypeMin) > 0
                                ? this.MinLimit
                                : TypeMin;
            var incremented = this.add(max, this.Increment.Value);
            var currentValue = this.CurrentValue;
            return currentValue.CompareTo(incremented) > 0
                            ? this.subtract(currentValue, this.Increment.Value)
                            : max;
        }
    }
}