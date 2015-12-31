namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// This is a ~manual~ binding with explicit two way updates.
    /// </summary>
    /// <typeparam name="T">The type that the <see cref="numericBox"/> holds</typeparam>
    internal class ExplicitBinding<T> : ExplicitBinding
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        private readonly BindingExpressionBase bindingExpression;
        private readonly NumericBox<T> numericBox;

        internal ExplicitBinding(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
            this.numericBox = numericBox;
            var binding = new Binding
            {
                Path = BindingHelper.GetPath(NumericBox<T>.ValueProperty),
                Source = numericBox,
                Mode = BindingMode.OneWayToSource,
                UpdateSourceTrigger = UpdateSourceTrigger.Explicit,
                NotifyOnValidationError = true,
                Converter = StringFormatConverter<T>.Default,
                ConverterParameter = numericBox,
            };

            foreach (var rule in rules)
            {
                binding.ValidationRules.Add(rule);
            }

            this.bindingExpression = BindingOperations.SetBinding(numericBox, TextProxyProperty, binding);
            Validation.AddErrorHandler(numericBox, this.OnValidationError);
            this.UpdateTextProxy();
            numericBox.FormatDirty += this.OnFormatDirty;
        }

        internal event EventHandler<ValidationErrorEventArgs> ValidationFailed;

        internal bool IsUpdatingText { get; set; }

        internal bool IsUpdatingValue { get; set; }

        internal bool HasValidationError
        {
            get
            {
                this.ExplicitValidate();
                return this.bindingExpression.HasValidationError;
            }
        }

        private string ProxyText
        {
            get { return (string)this.numericBox.GetValue(TextProxyProperty); }
            set { this.numericBox.SetCurrentValue(TextProxyProperty, value); }
        }

        internal void UpdateValue()
        {
            this.IsUpdatingValue = true;
            this.bindingExpression.UpdateSource();
            this.IsUpdatingValue = false;
        }

        internal void UpdateTextProxy()
        {
            this.IsUpdatingText = true;
            this.bindingExpression.UpdateTarget();
            this.numericBox.Text = StringFormatConverter<T>.Default.GetFormattedText(this.numericBox);
            this.IsUpdatingText = false;
        }

        internal void UpdateFormat()
        {
            if (!this.bindingExpression.HasValidationError)
            {
                this.IsUpdatingText = true;
                this.numericBox.Text = StringFormatConverter<T>.Default.GetFormattedText(this.numericBox);
                this.IsUpdatingText = false;
            }
        }

        internal void ExplicitValidate()
        {
            this.ProxyText = this.ProxyText; //// Trying this to set NeedsValidation to true.
            this.bindingExpression.ValidateWithoutUpdate();
        }

        protected virtual void RaiseValidationFailed(ValidationErrorEventArgs e)
        {
            this.ValidationFailed?.Invoke(this, e);
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            if (e.Action == ValidationErrorEventAction.Removed)
            {
                return;
            }

            this.RaiseValidationFailed(e);
        }

        private void OnFormatDirty(object sender, RoutedEventArgs e)
        {
            this.UpdateFormat();
        }
    }
}
