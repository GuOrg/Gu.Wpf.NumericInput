﻿namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    using Gu.Wpf.NumericInput;
    using Gu.Wpf.NumericInput.Annotations;

    public class Validator<T> : DependencyObject, INotifyPropertyChanged
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        internal static readonly DependencyProperty TextProxyProperty = DependencyProperty.RegisterAttached(
            "TextProxy",
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
            this._numericBox = numericBox;

            this._rules = rules;
            if (this._numericBox.IsLoaded)
            {
                this.Bind();
            }
            else
            {
                this._numericBox.Loaded += (_, __) => this.Bind();
            }
            this._numericBox.TextChanged += this.NumericBoxOnTextChanged;
            this._numericBox.ValueChanged += this.NumericBoxOnValueChanged;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public T ValueProxy
        {
            get
            {
                return (T)this._numericBox.GetValue(NumericBox<T>.ValueProperty);
            }
            set
            {
                this._value = value;
                if (value.Equals(this.ValueProxy))
                {
                    return;
                }
                this._numericBox.SetCurrentValue(NumericBox<T>.ValueProperty, value);
                this.OnPropertyChanged();
            }
        }

        private BindingExpression ValueBinding
        {
            get
            {
                return BindingOperations.GetBindingExpression(this._numericBox, NumericBox<T>.ValueProperty);
            }
        }

        internal static void SetTextProxy(NumericBox<T> element, string value)
        {
            element.SetValue(TextProxyProperty, value);
        }

        internal static string GetTextProxy(NumericBox<T> element)
        {
            return (string)element.GetValue(TextProxyProperty);
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private static void OnFakeTextChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            o.SetCurrentValue(TextBox.TextProperty, e.NewValue);
        }

        private void NumericBoxOnTextChanged(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            this._numericBox.SetCurrentValue(TextProxyProperty, this._numericBox.Text);
        }

        private void Bind()
        {
            var binding = new Binding("ValueProxy")
            {
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
                Source = this,
                NotifyOnValidationError = true
            };

            foreach (var rule in this._rules)
            {
                binding.ValidationRules.Add(rule);
            }

            Validation.AddErrorHandler(this._numericBox, this.OnValidationError);
            this._binding = this._numericBox.SetBinding(TextProxyProperty, binding);
            this._value = this._numericBox.Value;
            this._binding.UpdateTarget();
            MinDescriptor.AddValueChanged(this._numericBox, (s, e) => this._binding.UpdateSource());
            MaxDescriptor.AddValueChanged(this._numericBox, (s, e) => this._binding.UpdateSource());
        }

        private void NumericBoxOnValueChanged(object sender, ValueChangedEventArgs<T> valueChangedEventArgs)
        {
            if (this._value.CompareTo(valueChangedEventArgs.NewValue) != 0)
            {
                if (this._binding.HasValidationError)
                {
                    return;
                }
                this._value = valueChangedEventArgs.NewValue;
                this._binding.UpdateTarget();
            }
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs validationErrorEventArgs)
        {
            var expression = this.ValueBinding;
            if (expression != null)
            {
                this.ValueBinding.UpdateTarget(); // Reset local data on failure
            }
        }
    }
}