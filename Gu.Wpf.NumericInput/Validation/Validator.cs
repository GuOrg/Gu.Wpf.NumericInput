namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using Gu.Wpf.NumericInput;
    using Gu.Wpf.NumericInput.Annotations;

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

        public Validator(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
            _numericBox = numericBox;
            _proxyBinding = new ExplicitBinding<T>(numericBox, rules);

            _numericBox.TextChanged += this.NumericBoxOnTextChanged;
            _numericBox.ValueChanged += this.NumericBoxOnValueChanged;
            MinDescriptor.AddValueChanged(_numericBox, (s, e) => _proxyBinding.ExplicitValidate());
            MaxDescriptor.AddValueChanged(_numericBox, (s, e) => _proxyBinding.ExplicitValidate());
        }

        private BindingExpression ValueBinding
        {
            get
            {
                return BindingOperations.GetBindingExpression(_numericBox, NumericBox<T>.ValueProperty);
            }
        }

        private void NumericBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            _numericBox.SetCurrentValue(ExplicitBinding<T>.TextProxyProperty, _numericBox.Text);
            _proxyBinding.UpdateValue();
        }

        private void NumericBoxOnValueChanged(object sender, ValueChangedEventArgs<T> e)
        {

            //if (_value.CompareTo(e.NewValue) != 0)
            //{
            //    //if (_binding.HasValidationError)
            //    //{
            //    //    return;
            //    //}
            //    _numericBox.Text = e.NewValue.ToString();
            //}
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs validationErrorEventArgs)
        {
            var expression = this.ValueBinding;
            if (expression != null)
            {
                this.ValueBinding.UpdateTarget(); // Reset local data on failure
                _proxyBinding.UpdateTextProxy();
            }
        }
    }
}
