namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Baseclass with common functionality for numeric textboxes
    /// </summary>
    /// <typeparam name="T">The type of the <see cref="Value"/> property</typeparam>
    public abstract partial class NumericBox<T> : BaseBox
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        private static readonly T TypeMin = (T)typeof(T).GetField("MinValue").GetValue(null);
        private static readonly T TypeMax = (T)typeof(T).GetField("MaxValue").GetValue(null);
        private readonly Func<T, T, T> add;
        private readonly Func<T, T, T> subtract;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericBox{T}"/> class.
        /// </summary>
        /// <param name="add">How to add two values (x, y) => x + y</param>
        /// <param name="subtract">How to subtract two values (x, y) => x - y</param>
        protected NumericBox(Func<T, T, T> add, Func<T, T, T> subtract)
        {
            this.add = add;
            this.subtract = subtract;
        }

        /// <summary>
        /// Gets the current value. Will throw if bad format
        /// </summary>
        internal T? CurrentTextValue
        {
            get
            {
                var text = (string)this.GetValue(TextBindableProperty);
                T result;
                if (this.TryParse(text, out result))
                {
                    return result;
                }

                return null;
            }
        }

        internal T MaxLimit => this.MaxValue ?? TypeMax;

        internal T MinLimit => this.MinValue ?? TypeMin;

        public bool TryParse(string text, out T result)
        {
            return this.TryParse(text, this.NumberStyles, this.Culture, out result);
        }

        public abstract bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out T result);

        public bool CanParse(string text)
        {
            if (this.CanValueBeNull && string.IsNullOrEmpty(text))
            {
                return true;
            }

            T temp;
            return this.TryParse(text, out temp);
        }

        public T? Parse(string text)
        {
            if (this.CanValueBeNull && string.IsNullOrEmpty(text))
            {
                return null;
            }

            T result;
            if (this.TryParse(text, out result))
            {
                return result;
            }

            throw new FormatException($"Could not parse {text} to an instance of {typeof(T)}");
        }

        internal string Format(T? value)
        {
            return value?.ToString(this.StringFormat, this.Culture) ?? string.Empty;
        }

        public void UpdateFormat()
        {
            var text = (string)this.GetValue(TextBindableProperty);
            T result;
            if (this.TryParse(text, out result))
            {
                var newText = this.Format(result);
                this.FormattedText = newText;
            }
            else
            {
                this.FormattedText = text;
            }

            this.IsFormattingDirty = false;
        }

        public void UpdateValidation()
        {
            Debug.WriteLine(string.Empty);
            var status = this.Status;
            this.Status = Status.Validating;
            var text = this.GetValue(TextBindableProperty);
            this.SetCurrentValue(TextBindableProperty, text);
            this.IsValidationDirty = false;
            this.Status = status;
        }

        protected virtual void OnValueChanged(object newValue, object oldValue)
        {
            if (newValue != oldValue)
            {
                var args = new ValueChangedEventArgs<T?>((T?)oldValue, (T?)newValue, ValueChangedEvent, this);
                this.RaiseEvent(args);
            }
        }

        protected override bool CanIncrease(object parameter)
        {
            if (this.IsReadOnly || !this.IsEnabled || !this.AllowSpinners)
            {
                return false;
            }

            var currentValue = this.CurrentTextValue;
            if (currentValue == null)
            {
                return false;
            }

            return Comparer<T>.Default.Compare(currentValue.Value, this.MaxLimit) < 0;
        }

        protected override void Increase(object parameter)
        {
            var currentValue = this.CurrentTextValue;
            if (currentValue == null)
            {
                return;
            }

            var value = this.AddIncrement(currentValue.Value, this.Increment);
            this.SetIncremented(value);
        }

        protected override bool CanDecrease(object parameter)
        {
            if (this.IsReadOnly || !this.IsEnabled || !this.AllowSpinners)
            {
                return false;
            }

            var currentValue = this.CurrentTextValue;
            if (currentValue == null)
            {
                return false;
            }

            return Comparer<T>.Default.Compare(currentValue.Value, this.MinLimit) > 0;
        }

        protected override void Decrease(object parameter)
        {
            var currentValue = this.CurrentTextValue;
            if (currentValue == null)
            {
                return;
            }

            var value = this.SubtractIncrement(currentValue.Value, this.Increment);
            this.SetIncremented(value);
        }

        protected virtual void SetIncremented(T value)
        {
            Keyboard.Focus(this);
            this.SetTextAndCreateUndoAction(value.ToString(this.Culture));
            this.FormattedText = this.Format(value);
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == IsReadOnlyProperty)
            {
                this.CheckSpinners();
            }

            if (e.Property == IsEnabledProperty)
            {
                this.CheckSpinners();
            }

            base.OnPropertyChanged(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            Debug.WriteLine(string.Empty);
            this.UpdateFormat();
            base.OnLostFocus(e);
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.UpdateValidation();
        }

        private static void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            var box = (NumericBox<T>)sender;
            if (box.TextBindingExpression.HasValidationError && box.Status != Status.ResettingValue)
            {
                var valueBindingExpression = BindingOperations.GetBindingExpression(box, ValueProperty);
                if (valueBindingExpression != null)
                {
                    Debug.WriteLine(string.Empty);
                    var status = box.Status;
                    box.Status = Status.ResettingValue;
                    valueBindingExpression.UpdateTarget(); // Reset Value to value from DataContext binding.
                    box.Status = status;
                }
            }
        }

        private static void OnFormatDirty(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Empty);
            var box = (NumericBox<T>)sender;
            if (box.IsFocused || box.IsKeyboardFocusWithin)
            {
                return;
            }

            box.UpdateFormat();
        }

        private static void OnValidationDirty(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Empty);
            var box = (NumericBox<T>)sender;
            box.UpdateValidation();
        }

        private T AddIncrement(T currentValue, T increment)
        {
            var incremented = this.subtract(this.MaxLimit, increment);
            return currentValue.CompareTo(incremented) < 0
                            ? this.add(currentValue, increment)
                            : this.MaxLimit;
        }

        private T SubtractIncrement(T currentValue, T increment)
        {
            var incremented = this.add(this.MinLimit, increment);
            return currentValue.CompareTo(incremented) > 0
                            ? this.subtract(currentValue, increment)
                            : this.MinLimit;
        }
    }
}