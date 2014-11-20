namespace Gu.Wpf.NumericInput.Validation
{
    using System;
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
            };
            foreach (var rule in rules)
            {
                binding.ValidationRules.Add(rule);
            }

            _bindingExpression = BindingOperations.SetBinding(numericBox, TextProxyProperty, binding);
            UpdateTextProxy();
        }

        public bool HasValidationError
        {
            get
            {
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
    }
}
