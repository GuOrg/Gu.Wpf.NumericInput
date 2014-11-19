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
        where T : struct, IComparable<T>, IFormattable
    {
        internal static readonly DependencyProperty FakeTextProperty = DependencyProperty.RegisterAttached(
            "FakeText",
            typeof(string),
            typeof(Validator<T>),
            new PropertyMetadata(default(string), OnFakeTextChanged));

        private readonly NumericBox<T> _numericBox;
        private T _value;
        private BindingExpressionBase _binding;

        public Validator(NumericBox<T> numericBox)
        {
            _numericBox = numericBox;
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
            binding.ValidationRules.Add(new IsDoubleValidationRule());
            Validation.AddErrorHandler(_numericBox, this.OnValidationError);
            _binding = _numericBox.SetBinding(FakeTextProperty, binding);
            _value = _numericBox.Value;
            _binding.UpdateTarget();
            _numericBox.Text = GetFakeText(_numericBox);
        }

        private void NumericBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var numericBox = (NumericBox<T>)sender;
            numericBox.SetCurrentValue(FakeTextProperty, numericBox.Text);
        }

        private void NumericBoxOnValueChanged(object sender, ValueChangedEventArgs<T> valueChangedEventArgs)
        {
            if (_value.CompareTo(valueChangedEventArgs.NewValue) != 0)
            {
                //var fakeText = GetFakeText(_numericBox);
                //_binding.UpdateTarget();
                //var text = GetFakeText(_numericBox);
                //_numericBox.Text = text;
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
