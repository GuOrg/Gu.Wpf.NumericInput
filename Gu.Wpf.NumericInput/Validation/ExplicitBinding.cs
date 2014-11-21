namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Net.Mime;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    internal class ExplicitBinding<T> : DependencyObject
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        /// <summary>
        /// A proxy since Text is not bindable (we want it like this)
        /// </summary>
        internal static readonly DependencyProperty TextProxyProperty = DependencyProperty.RegisterAttached(
            "TextProxy",
            typeof(string),
            typeof(Validator<T>),
            new PropertyMetadata(default(string)));

        private readonly BindingExpressionBase _bindingExpression;

        public ExplicitBinding(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
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

            //var boxBinding = new Binding { Source = numericBox };
            //var formatBinding = new Binding(BaseUpDown.StringFormatProperty.Name) { Source = numericBox };
            //var cultureBinding = new Binding(BaseUpDown.CultureProperty.Name) { Source = numericBox };
            //var minBinding = new Binding(NumericBox<T>.MinValueProperty.Name) { Source = numericBox };
            //var maxBinding = new Binding(NumericBox<T>.MaxValueProperty.Name) { Source = numericBox };

            //var multiBinding = new MultiBinding
            //{
            //    NotifyOnValidationError = true,
            //    Mode = BindingMode.OneWayToSource,
            //    UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
            //};
            //multiBinding.Bindings.Add(boxBinding);
            //multiBinding.Bindings.Add(valueBinding);
            //multiBinding.Bindings.Add(formatBinding);
            //multiBinding.Bindings.Add(cultureBinding);
            //multiBinding.Bindings.Add(minBinding);
            //multiBinding.Bindings.Add(maxBinding);
            //_bindingExpression = BindingOperations.SetBinding(numericBox, TextProxyProperty, multiBinding);
            //Validation.AddErrorHandler(numericBox, OnValidationError);
            //UpdateTextProxy();
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

        public void ExplicitValidate()
        {
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
