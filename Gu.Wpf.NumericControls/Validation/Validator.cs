namespace Gu.Wpf.NumericControls.Validation
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.NumericControls.Annotations;

    public class Validator<T> : DependencyObject, INotifyPropertyChanged
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly DependencyProperty FakeTextProperty = DependencyProperty.RegisterAttached(
            "FakeText",
            typeof(string),
            typeof(Validator<T>),
            new PropertyMetadata(default(string), OnFakeTextChanged));

        private static readonly DependencyPropertyDescriptor MinDescriptor = DependencyPropertyDescriptor.FromProperty(
            NumericBox<T>.MinValueProperty, 
            typeof (NumericBox<T>));

        private static readonly DependencyPropertyDescriptor MaxDescriptor = DependencyPropertyDescriptor.FromProperty(
            NumericBox<T>.MaxValueProperty,
            typeof (NumericBox<T>));

        private readonly NumericBox<T> _numericBox;
        private readonly ValidationRule[] _rules;
        private T _value;
        private BindingExpressionBase _binding;

        public Validator(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
            _numericBox = numericBox;

            _rules = rules;
            if (_numericBox.IsLoaded)
            {
                Bind();
            }
            else
            {
                _numericBox.Loaded += (_, __) => Bind();
            }
            _numericBox.TextChanged += NumericBoxOnTextChanged;
            _numericBox.ValueChanged += NumericBoxOnValueChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public T FakeValue
        {
            get
            {
                return (T)_numericBox.GetValue(NumericBox<T>.ValueProperty);
            }
            set
            {
                _value = value;
                if (value.Equals(FakeValue))
                {
                    return;
                }
                _numericBox.SetCurrentValue(NumericBox<T>.ValueProperty, value);
                this.OnPropertyChanged();
            }
        }

        private BindingExpression ValueBinding
        {
            get
            {
                return BindingOperations.GetBindingExpression(_numericBox, NumericBox<T>.ValueProperty);
            }
        }

        internal static void SetFakeText(NumericBox<T> element, string value)
        {
            element.SetValue(FakeTextProperty, value);
        }

        internal static string GetFakeText(NumericBox<T> element)
        {
            return (string)element.GetValue(FakeTextProperty);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static void OnFakeTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            o.SetCurrentValue(TextBox.TextProperty, e.NewValue);
        }

        private void Bind()
        {
            var binding = new Binding("FakeValue")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Source = this,
                NotifyOnValidationError = true
            };

            foreach (var rule in _rules)
            {
                binding.ValidationRules.Add(rule);
            }

            Validation.AddErrorHandler(_numericBox, this.OnValidationError);
            _binding = _numericBox.SetBinding(FakeTextProperty, binding);
            _value = _numericBox.Value;
            _binding.UpdateTarget();
            MinDescriptor.AddValueChanged(_numericBox, (s, e) => _binding.UpdateSource());
            MaxDescriptor.AddValueChanged(_numericBox, (s, e) => _binding.UpdateSource());
        }

        private void NumericBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            _numericBox.SetCurrentValue(FakeTextProperty, _numericBox.Text);
        }

        private void NumericBoxOnValueChanged(object sender, ValueChangedEventArgs<T> valueChangedEventArgs)
        {
            if (_value.CompareTo(valueChangedEventArgs.NewValue) != 0)
            {
                if (_binding.HasValidationError)
                {
                    return;
                }
                _value = valueChangedEventArgs.NewValue;
                _binding.UpdateTarget();
            }
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs validationErrorEventArgs)
        {
            var expression = ValueBinding;
            if (expression != null)
            {
                ValueBinding.UpdateTarget(); // Reset local data on failure
            }
        }
    }
}
