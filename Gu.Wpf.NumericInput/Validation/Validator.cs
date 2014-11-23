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

        private static readonly DependencyPropertyDescriptor PatternDescriptor = DependencyPropertyDescriptor.FromProperty(
            BaseBox.RegexPatternProperty,
            typeof(NumericBox<T>));

        private readonly ExplicitBinding<T> _proxyBinding;
        private readonly NumericBox<T> _numericBox;

        public Validator(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
            _numericBox = numericBox;
            _proxyBinding = new ExplicitBinding<T>(numericBox, rules);

            _numericBox.TextChanged += NumericBoxOnTextChanged;
            _numericBox.ValueChanged += NumericBoxOnValueChanged;
            MinDescriptor.AddValueChanged(_numericBox, (s, e) => _proxyBinding.ExplicitValidate());
            MaxDescriptor.AddValueChanged(_numericBox, (s, e) => _proxyBinding.ExplicitValidate());
            PatternDescriptor.AddValueChanged(_numericBox, (s, e) => _proxyBinding.ExplicitValidate());
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
            if (!this.IsTextChanged())
            {
                _proxyBinding.ExplicitValidate();
                return;
            }
            _numericBox.SetCurrentValue(ExplicitBinding<T>.TextProxyProperty, _numericBox.Text);
            if (!_proxyBinding.IsUpdatingText)
            {
                if (!_proxyBinding.HasValidationError)
                {
                    _proxyBinding.UpdateValue();
                }
            }
            else
            {
                _proxyBinding.ExplicitValidate();
            }
        }

        private void NumericBoxOnValueChanged(object sender, ValueChangedEventArgs<T> e)
        {
            if (_proxyBinding.IsUpdatingValue)
            {
                return;
            }
            _proxyBinding.UpdateTextProxy();
        }

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!_proxyBinding.HasValidationError)
            {
                _proxyBinding.IsUpdatingText = true;
                _numericBox.Text = _numericBox.Value.ToString(_numericBox.StringFormat, _numericBox.Culture);
                _proxyBinding.IsUpdatingText = false;
            }
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            var expression = ValueBinding;
            if (expression != null)
            {
                _proxyBinding.IsUpdatingValue = true;
                ValueBinding.UpdateTarget(); // Reset Value to value from from vm binding.
                _proxyBinding.IsUpdatingValue = false;
            }
        }

        private bool IsTextChanged()
        {
            var viewText = _numericBox.Text;
            var proxyText = (string)_numericBox.GetValue(ExplicitBinding.TextProxyProperty);
            if (viewText == proxyText)
            {
                return false;
            }
            if (viewText.HasMoreDecimalDigitsThan(proxyText, _numericBox))
            {
                return true;
            }
            return proxyText != viewText;
        }
    }
}
