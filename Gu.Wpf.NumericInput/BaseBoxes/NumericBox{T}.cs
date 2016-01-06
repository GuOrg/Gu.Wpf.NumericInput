namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

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
        private static readonly EventHandler<ValidationErrorEventArgs> ValidationErrorHandler = OnValidationError;
        private static readonly RoutedEventHandler FormatDirtyHandler = OnFormatDirty;
        private static readonly RoutedEventHandler ValidationDirtyHandler = OnValidationDirty;
        private readonly BindingExpressionBase textValueBindingExpression;

        /// <summary>
        /// Initializes a new instance of the <see cref="NumericBox{T}"/> class.
        /// </summary>
        /// <param name="add">How to add two values (x, y) => x + y</param>
        /// <param name="subtract">How to subtract two values (x, y) => x - y</param>
        protected NumericBox(Func<T, T, T> add, Func<T, T, T> subtract)
        {
            this.add = add;
            this.subtract = subtract;
            var binding = new Binding
            {
                Path = BindingHelper.GetPath(ValueProperty),
                Source = this,
                Mode = BindingMode.OneWayToSource,
                NotifyOnValidationError = true,
                Converter = StringConverter<T>.Default,
                ConverterParameter = this,
            };

            binding.ValidationRules.Add(CanParse<T>.Default);
            binding.ValidationRules.Add(IsMatch.FromText);
            binding.ValidationRules.Add(IsMatch.FromValue);
            binding.ValidationRules.Add(IsGreaterThanOrEqualToMinRule<T>.FromText);
            binding.ValidationRules.Add(IsGreaterThanOrEqualToMinRule<T>.FromValue);
            binding.ValidationRules.Add(IsLessThanOrEqualToMaxRule<T>.FromText);
            binding.ValidationRules.Add(IsLessThanOrEqualToMaxRule<T>.FromValue);
            this.textValueBindingExpression = BindingOperations.SetBinding(this, TextBindableProperty, binding);
            this.AddHandler(System.Windows.Controls.Validation.ErrorEvent, ValidationErrorHandler);
            this.AddHandler(FormatDirtyEvent, FormatDirtyHandler);
            this.AddHandler(ValidationDirtyEvent, ValidationDirtyHandler);
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
            return TryParse(text, this.NumberStyles, this.Culture, out result);
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
                var status = this.Status;
                this.Status = NumericInput.Status.Formatting;
                var newText = this.Format(result);
                Debug.WriteLine((object)this.Text, newText);
                this.Text = newText;
                this.Status = status;
            }
            else
            {
                Debug.WriteLine("NOP");
            }

            this.IsFormattingDirty = false;
        }

        public void UpdateValidation()
        {
            Debug.WriteLine(string.Empty);
            var status = this.Status;
            this.Status = NumericInput.Status.Validating;
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
            if (this.IsReadOnly || !this.IsEnabled)
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

            var value = this.AddIncrement(currentValue.Value);
            var text = value.ToString(this.StringFormat, this.Culture);
            this.SetTextUndoable(text);
        }

        protected override bool CanDecrease(object parameter)
        {
            if (this.IsReadOnly || !this.IsEnabled)
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

            var value = this.SubtractIncrement(currentValue.Value);
            var text = value.ToString(this.StringFormat, this.Culture);
            this.SetTextUndoable(text);
        }

        protected virtual void SetTextUndoable(string text)
        {
            this.ValueBox.SetTextUndoable(text);
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
            if (this.IsFormattingDirty || this.TextSource == TextSource.UserInput)
            {
                this.UpdateFormat();
            }

            base.OnLostFocus(e);
        }

        private static void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            var box = (NumericBox<T>)sender;
            if (box.textValueBindingExpression.HasValidationError && box.Status != Status.ResettingValue)
            {
                var valueBindingExpression = BindingOperations.GetBindingExpression(box, ValueProperty);
                if (valueBindingExpression != null)
                {
                    Debug.WriteLine(string.Empty);
                    var status = box.Status;
                    box.Status = NumericInput.Status.ResettingValue;
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

        private T AddIncrement(T currentValue)
        {
            var incremented = this.subtract(this.MaxLimit, this.Increment);
            return currentValue.CompareTo(incremented) < 0
                            ? this.add(currentValue, this.Increment)
                            : this.MaxLimit;
        }

        private T SubtractIncrement(T currentValue)
        {
            var incremented = this.add(this.MinLimit, this.Increment);
            return currentValue.CompareTo(incremented) > 0
                            ? this.subtract(currentValue, this.Increment)
                            : this.MinLimit;
        }
    }
}