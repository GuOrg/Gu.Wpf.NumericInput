namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.ComponentModel;
    using System.Net.Mime;
    using System.Reflection;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class ExplicitBinding<T> : DependencyObject
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        private readonly NumericBox<T> _numericBox;

        /// <summary>
        /// A proxy since Text is not bindable (we want it like this)
        /// </summary>
        internal static readonly DependencyProperty TextProxyProperty = DependencyProperty.RegisterAttached(
            "TextProxy",
            typeof(string),
            typeof(Validator<T>),
            new PropertyMetadata(default(string)));
        private static readonly DependencyPropertyDescriptor CultureDescriptor = DependencyPropertyDescriptor.FromProperty(
            BaseUpDown.CultureProperty,
            typeof(NumericBox<T>));

        private static readonly DependencyPropertyDescriptor StringFormatDescriptor = DependencyPropertyDescriptor.FromProperty(
            BaseUpDown.StringFormatProperty,
            typeof(NumericBox<T>));

        private readonly BindingExpressionBase _bindingExpression;

        public ExplicitBinding(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
            _numericBox = numericBox;
            var binding = new Binding(NumericBox<T>.ValueProperty.Name)
            {
                Source = numericBox,
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                NotifyOnValidationError = true,
                Converter = new StringFormatConverter(numericBox)
            };
            foreach (var rule in rules)
            {
                binding.ValidationRules.Add(rule);
            }
            _bindingExpression = BindingOperations.SetBinding(numericBox, TextProxyProperty, binding);
            Validation.AddErrorHandler(numericBox, OnValidationError);
            UpdateTextProxy();

            CultureDescriptor.AddValueChanged(numericBox, (s, e) => this.UpdateFormat());
            StringFormatDescriptor.AddValueChanged(numericBox, (s, e) => this.UpdateFormat());
        }

        public event EventHandler<ValidationErrorEventArgs> ValidationFailed;

        public bool HasValidationError
        {
            get
            {
                ExplicitValidate();
                return _bindingExpression.HasValidationError;
            }
        }

        private BindingExpression ValueBinding
        {
            get
            {
                return BindingOperations.GetBindingExpression(_numericBox, NumericBox<T>.ValueProperty);
            }
        }

        public string ProxyText
        {
            get
            {
                return (string)_numericBox.GetValue(TextProxyProperty);
            }
            private set
            {
                _numericBox.SetCurrentValue(TextProxyProperty, value);
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

        public void UpdateValue()
        {
            _bindingExpression.UpdateSource();
        }

        public void UpdateTextProxy()
        {
            _bindingExpression.UpdateTarget();
        }

        public void UpdateFormat()
        {
            if (!_bindingExpression.HasValidationError)
            {
                _numericBox.Text = _numericBox.Value.ToString(_numericBox.StringFormat, _numericBox.Culture);
            }
        }

        public void ExplicitValidate()
        {
#if DEBUG
            var propertyInfo = _bindingExpression.GetType()
                                                 .GetProperty("NeedsValidation", BindingFlags.NonPublic|BindingFlags.Instance);
            var needsValidation =(bool) propertyInfo.GetValue(_bindingExpression);
            ProxyText = ProxyText; //// Trying this to set NeedsValidation to true.  
            needsValidation = (bool)propertyInfo.GetValue(_bindingExpression);
#endif
            ProxyText = ProxyText; //// Trying this to set NeedsValidation to true. 
            _bindingExpression.ValidateWithoutUpdate();
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                return;
            }
            RaiseValidationFailed(e);
        }

        protected virtual void RaiseValidationFailed(ValidationErrorEventArgs e)
        {
            var handler = ValidationFailed;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
