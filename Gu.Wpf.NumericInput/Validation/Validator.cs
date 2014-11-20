namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using NumericInput;

    public class Validator<T> : DependencyObject
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        private static readonly DependencyPropertyDescriptor MinDescriptor = DependencyPropertyDescriptor.FromProperty(
            NumericBox<T>.MinValueProperty,
            typeof(NumericBox<T>));

        private static readonly DependencyPropertyDescriptor MaxDescriptor = DependencyPropertyDescriptor.FromProperty(
            NumericBox<T>.MaxValueProperty,
            typeof(NumericBox<T>));

        private readonly ExplicitBinding<T> _proxyBinding;
        private readonly NumericBox<T> _numericBox;

        private bool _isUpdatingValue;
        private bool _isUpdatingText;
        public Validator(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
            _numericBox = numericBox;
            _proxyBinding = new ExplicitBinding<T>(numericBox, rules);

            _numericBox.TextChanged += NumericBoxOnTextChanged;
            _numericBox.ValueChanged += NumericBoxOnValueChanged;
            MinDescriptor.AddValueChanged(_numericBox, (s, e) => _proxyBinding.ExplicitValidate());
            MaxDescriptor.AddValueChanged(_numericBox, (s, e) => _proxyBinding.ExplicitValidate());
            _proxyBinding.ValidationFailed += OnValidationError;
            _numericBox.LostFocus += OnLostFocus;
        }

        private BindingExpression ValueBinding
        {
            get
            {
                return BindingOperations.GetBindingExpression(_numericBox, NumericBox<T>.ValueProperty);
            }
        }

        private void NumericBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            _numericBox.SetCurrentValue(ExplicitBinding<T>.TextProxyProperty, _numericBox.Text);
            if (!_isUpdatingText)
            {
                _isUpdatingValue = true;
                _proxyBinding.UpdateValue();
                _isUpdatingValue = false;
            }
            else
            {
                _proxyBinding.ExplicitValidate();
            }
        }

        private void NumericBoxOnValueChanged(object sender, ValueChangedEventArgs<T> e)
        {
            if (_isUpdatingValue)
            {
                return;
            }
            _isUpdatingText = true;
            _numericBox.Text = _numericBox.Value.ToString(_numericBox.StringFormat, _numericBox.Culture);
            _isUpdatingText = false;
        }

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!_proxyBinding.HasValidationError)
            {
                _isUpdatingText = true;
                _numericBox.Text = _numericBox.Value.ToString(_numericBox.StringFormat, _numericBox.Culture);
                _isUpdatingText = false;
            }
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            var expression = ValueBinding;
            if (expression != null)
            {
                ValueBinding.UpdateTarget(); // Reset Value from vm binding on failure
                //_isUpdatingText = true;
                //_proxyBinding.UpdateTextProxy();
                //_isUpdatingText = false;
            }
        }
    }
}
