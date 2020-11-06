namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>Base class with common functionality for numeric text boxes.</summary>
    /// <typeparam name="T">The type of the numeric value.</typeparam>
    public abstract partial class NumericBox<T> : BaseBox, ISpinnerBox
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        /// <summary>Initializes a new instance of the <see cref="NumericBox{T}"/> class.</summary>
        protected NumericBox()
        {
            this.IncreaseCommand = new ManualRelayCommand(this.Increase, this.CanIncrease);
            this.DecreaseCommand = new ManualRelayCommand(this.Decrease, this.CanDecrease);
        }

        /// <summary>Gets the current value. Will throw if bad format.</summary>
        internal T? CurrentTextValue
        {
            get
            {
                var text = this.Text;
                if (this.TryParse(text, out T result))
                {
                    return result;
                }

                return null;
            }
        }

        internal T MaxLimit => this.MaxValue ?? this.TypeMax();

        internal T MinLimit => this.MinValue ?? this.TypeMin();

        /// <summary>
        /// Try parse a <typeparamref name="T"/> from <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if success.</returns>
        public bool TryParse(string text, out T result)
        {
            return this.TryParse(text, this.NumberStyles, this.Culture, out result);
        }

        /// <summary>
        /// Try parse a <typeparamref name="T"/> from <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="numberStyles">The <see cref="NumberStyles"/>.</param>
        /// <param name="culture">The <see cref="IFormatProvider"/>.</param>
        /// <param name="result">The result.</param>
        /// <returns>True if success.</returns>
        public abstract bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out T result);

        /// <summary>
        /// Check if <paramref name="text"/> can be parsed.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>True if success.</returns>
        public bool CanParse(string text)
        {
            if (this.CanValueBeNull && string.IsNullOrEmpty(text))
            {
                return true;
            }

            return this.TryParse(text, out T _);
        }

        /// <summary>
        /// Parse a <typeparamref name="T"/> from <paramref name="text"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The value.</returns>
        public T? Parse(string text)
        {
            if (this.CanValueBeNull && string.IsNullOrEmpty(text))
            {
                return null;
            }

            if (this.TryParse(text, out T result))
            {
                return result;
            }

            throw new FormatException($"Could not parse {text} to an instance of {typeof(T)}");
        }

        /// <summary>
        /// Update the formatted text.
        /// </summary>
        public void UpdateFormattedText()
        {
            this.UpdateFormattedText(skipIfNotDirty: false);
        }

        /// <inheritdoc/>
        public override void UpdateValidation()
        {
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

        /// <summary>
        /// Update the formatted text.
        /// </summary>
        /// <param name="skipIfNotDirty">Skip if true.</param>
        public void UpdateFormattedText(bool skipIfNotDirty)
        {
            if (skipIfNotDirty && !this.IsFormattingDirty)
            {
                return;
            }

            var text = this.Text;
            if (this.TryParse(text, out T result))
            {
                this.UpdateFormattedText(result);
            }
            else
            {
                this.FormattedText = text;
            }

            this.IsFormattingDirty = false;
        }

        internal string Format(T? value)
        {
            return value?.ToString(this.StringFormat, this.Culture) ?? string.Empty;
        }

        internal string ToRawText(T? value)
        {
            return value?.ToString(this.Culture) ?? string.Empty;
        }

        /// <summary>
        /// Update the formatted text.
        /// </summary>
        /// <param name="value">The value.</param>
        protected virtual void UpdateFormattedText(T? value)
        {
            var formattedText = this.Format(value);
            this.FormattedText = formattedText;
            this.IsFormattingDirty = false;
        }

        /// <summary>
        /// Reset <see cref="Value"/> from binding.
        /// </summary>
        protected virtual void ResetValueFromSource()
        {
            Debug.WriteLine(string.Empty);
            var valueBindingExpression = BindingOperations.GetBindingExpression(this, ValueProperty);
            if (valueBindingExpression != null)
            {
                Debug.WriteLine(string.Empty);
                var status = this.Status;
                this.Status = Status.ResettingValue;
                valueBindingExpression.UpdateTarget(); // Reset Value to value from DataContext binding.
                this.Status = status;
            }
        }

        /// <summary>
        /// Called when <see cref="Value"/> changes.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        protected virtual void OnValueChanged(T? oldValue, T? newValue)
        {
            if (this.Status == Status.Idle)
            {
                this.Status = Status.UpdatingFromValueBinding;
                this.TextSource = TextSource.ValueBinding;
                var newRaw = (string?)this.TextValueConverter?.ConvertBack(newValue, typeof(string), this, null) ?? string.Empty;
                this.SetTextClearUndo(newRaw);
                this.UpdateFormattedText(newValue);
                this.IsValidationDirty = true;
                this.CheckSpinners();
                this.Status = Status.Idle;
            }

            var args = new ValueChangedEventArgs<T?>(oldValue, newValue, ValueChangedEvent, this);
            this.RaiseEvent(args);
        }

        /// <summary>
        /// Called when Text changes.
        /// </summary>
        /// <param name="oldText">The old text.</param>
        /// <param name="newText">The new value.</param>
        protected virtual void OnTextChanged(string? oldText, string? newText)
        {
            if (this.Status == Status.Idle)
            {
                this.Status = Status.UpdatingFromUserInput;
                this.TextSource = TextSource.UserInput;
                if (this.IsLoaded && this.ValidationTrigger == ValidationTrigger.PropertyChanged)
                {
                    var result = Validator.ValidateAndGetValue(this);
                    this.IsValidationDirty = false;
                    if (result != Binding.DoNothing)
                    {
                        this.SetCurrentValue(ValueProperty, result);
                    }
                    else
                    {
                        this.ResetValueFromSource();
                    }
                }
                else
                {
                    var result = this.TextValueConverter?.Convert(newText, typeof(T), this, null);
                    if (result != Binding.DoNothing)
                    {
                        this.SetCurrentValue(ValueProperty, result);
                    }
                    else
                    {
                        this.ResetValueFromSource();
                    }

                    this.IsValidationDirty = true;
                }

                this.Status = Status.Idle;
                this.IsFormattingDirty = true;
                this.CheckSpinners();
            }
        }

        /// <summary>
        /// Raises CanExecuteChanged if <see cref="AllowSpinners"/>.
        /// </summary>
        protected virtual void CheckSpinners()
        {
            if (this.AllowSpinners)
            {
                (this.IncreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
                (this.DecreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        /// <summary>
        /// Invoked when IncreaseCommand.CanExecute() is executed.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        /// <returns>True if the value can be increased.</returns>
        protected virtual bool CanIncrease(object parameter)
        {
            if (this.IsReadOnly || !this.IsEnabled || !this.AllowSpinners)
            {
                return false;
            }

            var currentValue = this.CurrentTextValue;
            if (currentValue is null)
            {
                return false;
            }

            return Comparer<T>.Default.Compare(currentValue.Value, this.MaxLimit) < 0;
        }

        /// <summary>
        /// Invoked when IncreaseCommand.Execute() is executed.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        protected virtual void Increase(object parameter)
        {
            var currentValue = this.CurrentTextValue;
            if (currentValue is null)
            {
                return;
            }

            var value = this.AddIncrement(currentValue.Value, this.Increment);
            this.SetIncremented(value);
        }

        /// <summary>
        /// Invoked when DecreaseCommand.CanExecute() is executed.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        /// <returns>True if the value can be decreased.</returns>
        protected virtual bool CanDecrease(object parameter)
        {
            if (this.IsReadOnly || !this.IsEnabled || !this.AllowSpinners)
            {
                return false;
            }

            var currentValue = this.CurrentTextValue;
            if (currentValue is null)
            {
                return false;
            }

            return Comparer<T>.Default.Compare(currentValue.Value, this.MinLimit) > 0;
        }

        /// <summary>
        /// Invoked when DecreaseCommand.Execute() is executed.
        /// </summary>
        /// <param name="parameter">Not used.</param>
        protected virtual void Decrease(object parameter)
        {
            var currentValue = this.CurrentTextValue;
            if (currentValue is null)
            {
                return;
            }

            var value = this.SubtractIncrement(currentValue.Value, this.Increment);
            this.SetIncremented(value);
        }

        /// <summary>
        /// Adds <paramref name="x"/> and <paramref name="y"/>.
        /// </summary>
        /// <param name="x">The left value.</param>
        /// <param name="y">The right value.</param>
        /// <returns>The sum.</returns>
        protected abstract T Add(T x, T y);

        /// <summary>
        /// Adds <paramref name="y"/> from <paramref name="x"/>.
        /// </summary>
        /// <param name="x">The left value.</param>
        /// <param name="y">The right value.</param>
        /// <returns>The difference.</returns>
        protected abstract T Subtract(T x, T y);

        /// <summary>
        /// The minimum value for <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Minimum value for <typeparamref name="T"/>.</returns>
        protected abstract T TypeMin();

        /// <summary>
        /// The maximum value for <typeparamref name="T"/>.
        /// </summary>
        /// <returns>Maximum value for <typeparamref name="T"/>.</returns>
        protected abstract T TypeMax();

        /// <summary>
        /// Called when user clicks a spinner button.
        /// </summary>
        /// <param name="value">The new value.</param>
        protected virtual void SetIncremented(T value)
        {
            _ = Keyboard.Focus(this);
            this.SetTextAndCreateUndoAction(value.ToString(this.Culture));
            this.UpdateFormattedText(value);
            if (this.SpinUpdateMode == SpinUpdateMode.PropertyChanged)
            {
                if (BindingOperations.GetBindingExpression(this, ValueProperty) is { ParentBinding: { } binding } expression &&
                    binding.Mode.IsEither(BindingMode.TwoWay, BindingMode.Default) &&
                    binding.UpdateSourceTrigger.IsEither(UpdateSourceTrigger.Default, UpdateSourceTrigger.LostFocus))
                {
                    this.UpdateValidation();
                    expression.UpdateSource();
                }
            }
        }

        /// <inheritdoc/>
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
                this.OnTextChanged((string?)e.OldValue, (string?)e.NewValue);
            }

            base.OnPropertyChanged(e);
        }

        /// <inheritdoc/>
        protected override void OnLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            Debug.WriteLine(string.Empty);
            this.UpdateFormattedText(skipIfNotDirty: true);
            base.OnLostKeyboardFocus(e);
        }

        /// <inheritdoc/>
        protected override void OnLostFocus(RoutedEventArgs e)
        {
            Debug.WriteLine(string.Empty);
            this.UpdateFormattedText(skipIfNotDirty: true);
            base.OnLostFocus(e);
        }

        /// <inheritdoc/>
        protected override void OnLoaded()
        {
            base.OnLoaded();
            this.UpdateValidation();
        }

        private static void OnValidationError(object? sender, ValidationErrorEventArgs e)
        {
            if (sender is NumericBox<T> box)
            {
                if (box.Status == Status.ResettingValue)
                {
                    return;
                }

                if (box.TextBindingExpression.HasValidationError)
                {
                    box.ResetValueFromSource();
                }
            }
        }

        private static void OnFormatDirty(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Empty);
            var box = (NumericBox<T>)sender;
            if (box.IsKeyboardFocused || box.IsKeyboardFocusWithin)
            {
                return;
            }

            box.UpdateFormattedText(skipIfNotDirty: true);
        }

        private static void OnValidationDirty(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(string.Empty);
            var box = (NumericBox<T>)sender;
            if (!box.IsKeyboardFocused || box.ValidationTrigger == ValidationTrigger.PropertyChanged)
            {
                box.UpdateValidation();
            }
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
