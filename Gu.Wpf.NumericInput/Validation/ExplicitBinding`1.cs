namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.ComponentModel;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// This is a ~manual~ bidning with explicit two way updates.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class ExplicitBinding<T> : ExplicitBinding
    {
        private readonly NumericBox<T> _numericBox;

        private static readonly DependencyPropertyDescriptor CultureDescriptor = DependencyPropertyDescriptor.FromProperty(
            BaseBox.CultureProperty,
            typeof(NumericBox<T>));

        private static readonly DependencyPropertyDescriptor StringFormatDescriptor = DependencyPropertyDescriptor.FromProperty(
            BaseBox.StringFormatProperty,
            typeof(NumericBox<T>));

        private readonly BindingExpressionBase _bindingExpression;

        private StringFormatConverter _stringFormatConverter;

        public ExplicitBinding(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
            _numericBox = numericBox;
            this._stringFormatConverter = new StringFormatConverter(numericBox);
            var binding = new Binding(NumericBox<T>.ValueProperty.Name)
            {
                Source = numericBox,
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                NotifyOnValidationError = true,
                Converter = this._stringFormatConverter
            };
            foreach (var rule in rules)
            {
                binding.ValidationRules.Add(rule);
            }
            _bindingExpression = BindingOperations.SetBinding(numericBox, TextProxyProperty, binding);
            NumericInput.Validation.AddErrorHandler(numericBox, OnValidationError);
            UpdateTextProxy();

            CultureDescriptor.AddValueChanged(numericBox, (s, e) => this.UpdateFormat());
            StringFormatDescriptor.AddValueChanged(numericBox, (s, e) => this.UpdateFormat());
        }

        public event EventHandler<ValidationErrorEventArgs> ValidationFailed;

        public bool IsUpdatingText { get; set; }

        public bool IsUpdatingValue { get; set; }

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

        public void UpdateValue()
        {
            IsUpdatingValue = true;
            _bindingExpression.UpdateSource();
            IsUpdatingValue = false;
        }

        public void UpdateTextProxy()
        {
            IsUpdatingText = true;
            _bindingExpression.UpdateTarget();
            _numericBox.Text = _stringFormatConverter.FormattedText;
            IsUpdatingText = false;
        }

        public void UpdateFormat()
        {
            if (!_bindingExpression.HasValidationError)
            {
                IsUpdatingText = true;
                _numericBox.Text = _stringFormatConverter.FormattedText;
                IsUpdatingText = false;
            }
        }

        public void ExplicitValidate()
        {
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
