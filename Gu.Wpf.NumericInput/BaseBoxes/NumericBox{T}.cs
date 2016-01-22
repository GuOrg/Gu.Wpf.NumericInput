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
    public abstract partial class NumericBox<T> : BaseBox, IIncrementBox
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumericBox{T}"/> class.
        /// </summary>
        /// <param name="add">How to add two values (x, y) => x + y</param>
        /// <param name="subtract">How to subtract two values (x, y) => x - y</param>
        protected NumericBox()
        {
            this.IncreaseCommand = new ManualRelayCommand(this.Increase, this.CanIncrease);
            this.DecreaseCommand = new ManualRelayCommand(this.Decrease, this.CanDecrease);
        }

        /// <summary>
        /// Gets the current value. Will throw if bad format
        /// </summary>
        internal T? CurrentTextValue
        {
            get
            {
                var text = this.Text;
                T result;
                if (this.TryParse(text, out result))
                {
                    return result;
                }

                return null;
            }
        }

        internal T MaxLimit => this.MaxValue ?? this.TypeMax();

        internal T MinLimit => this.MinValue ?? this.TypeMin();

        public bool TryParse(string text, out T result)
        {
            return this.TryParse(text, this.NumberStyles, this.Culture, out result);
        }

        public abstract bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out T result);

        public bool CanParse(string text)
        {
            if (this.CanValueBeNull && String.IsNullOrEmpty(text))
            {
                return true;
            }

            T temp;
            return this.TryParse(text, out temp);
        }

        public T? Parse(string text)
        {
            if (this.CanValueBeNull && String.IsNullOrEmpty(text))
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
            return value?.ToString(this.StringFormat, this.Culture) ?? String.Empty;
        }

        internal string ToRawText(T? value)
        {
            return value?.ToString(this.Culture) ?? String.Empty;
        }

        public void UpdateFormat()
        {
            var text = this.Text;
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
            Debug.WriteLine(String.Empty);
            var status = this.Status;
            this.Status = Status.Validating;
            var result = Validator.ValidateAndGetValue(this);
            this.IsValidationDirty = false;
            if (result != Binding.DoNothing)
            {
                this.SetCurrentValue(ValueProperty, result);
            }
            this.IsValidationDirty = false;
            this.Status = status;
        }

        protected virtual void OnValidationError()
        {
            Debug.WriteLine(String.Empty);
            var valueBindingExpression = BindingOperations.GetBindingExpression(this, ValueProperty);
            if (valueBindingExpression != null)
            {
                Debug.WriteLine(String.Empty);
                var status = this.Status;
                this.Status = Status.ResettingValue;
                valueBindingExpression.UpdateTarget(); // Reset Value to value from DataContext binding.
                this.Status = status;
            }
        }

        protected virtual void OnValueChanged(T? oldValue, T? newValue)
        {
            if (this.Status == Status.Idle)
            {
                this.Status = Status.UpdatingFromValueBinding;
                this.TextSource = TextSource.ValueBinding;
                var newRaw = (string)this?.TextValueConverter.ConvertBack(newValue, typeof(string), this, null) ?? String.Empty;
                this.SetTextClearUndo(newRaw);
                this.FormattedText = this.Format(newValue);
                this.IsValidationDirty = true;
                this.CheckSpinners();
                this.Status = Status.Idle;
            }

            var args = new ValueChangedEventArgs<T?>(oldValue, newValue, ValueChangedEvent, this);
            this.RaiseEvent(args);
        }

        protected virtual void OnTextChanged(string oldText, string newText)
        {
            Debug.WriteLine(newText);
            if (this.Status == Status.Idle)
            {
                this.Status = Status.UpdatingFromUserInput;
                this.TextSource = TextSource.UserInput;
                var result = Validator.ValidateAndGetValue(this);
                this.IsValidationDirty = false;
                if (result != Binding.DoNothing)
                {
                    this.SetCurrentValue(ValueProperty, result);
                }

                this.Status = Status.Idle;
                this.IsFormattingDirty = true;
                this.CheckSpinners();
            }
        }

        protected virtual void CheckSpinners()
        {
            if (this.AllowSpinners)
            {
                (this.IncreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
                (this.DecreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Invoked when IncreaseCommand.CanExecute() is executed
        /// </summary>
        /// <param name="parameter">Not used</param>
        /// <returns>True if the value can be increased</returns>
        protected virtual bool CanIncrease(object parameter)
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

        /// <summary>
        /// Invoked when IncreaseCommand.Execute() is executed
        /// </summary>
        /// <param name="parameter">Not used</param>
        protected virtual void Increase(object parameter)
        {
            var currentValue = this.CurrentTextValue;
            if (currentValue == null)
            {
                return;
            }

            var value = this.AddIncrement(currentValue.Value, this.Increment);
            this.SetIncremented(value);
        }

        /// <summary>
        /// Invoked when DecreaseCommand.CanExecute() is executed
        /// </summary>
        /// <param name="parameter">Not used</param>
        /// <returns>True if the value can be decreased</returns>
        protected virtual bool CanDecrease(object parameter)
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

        /// <summary>
        /// Invoked when DecreaseCommand.Execute() is executed
        /// </summary>
        /// <param name="parameter">Not used</param>
        protected virtual void Decrease(object parameter)
        {
            var currentValue = this.CurrentTextValue;
            if (currentValue == null)
            {
                return;
            }

            var value = this.SubtractIncrement(currentValue.Value, this.Increment);
            this.SetIncremented(value);
        }

        protected abstract T Add(T x, T y);

        protected abstract T Subtract(T x, T y);

        protected abstract T TypeMin();

        protected abstract T TypeMax();

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

            if (e.Property == TextProperty && !Equals(e.OldValue, e.NewValue))
            {
                this.OnTextChanged((string)e.OldValue, (string)e.NewValue);
            }

            base.OnPropertyChanged(e);
        }

        protected override void OnLostFocus(RoutedEventArgs e)
        {
            Debug.WriteLine(String.Empty);
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
            if (box.Status == Status.ResettingValue)
            {
                return;
            }

            if (box.TextBindingExpression.HasValidationError)
            {
                box.OnValidationError();
            }
        }

        private static void OnFormatDirty(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(String.Empty);
            var box = (NumericBox<T>)sender;
            if (box.IsFocused || box.IsKeyboardFocusWithin)
            {
                return;
            }

            box.UpdateFormat();
        }

        private static void OnValidationDirty(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(String.Empty);
            var box = (NumericBox<T>)sender;
            box.UpdateValidation();
        }

        private T AddIncrement(T currentValue, T increment)
        {
            var incremented = this.Subtract(this.MaxLimit, increment);
            return currentValue.CompareTo(incremented) < 0
                            ? this.Add(currentValue, increment)
                            : this.MaxLimit;
        }

        private T SubtractIncrement(T currentValue, T increment)
        {
            var incremented = this.Add(this.MinLimit, increment);
            return currentValue.CompareTo(incremented) > 0
                            ? this.Subtract(currentValue, increment)
                            : this.MinLimit;
        }
    }
}