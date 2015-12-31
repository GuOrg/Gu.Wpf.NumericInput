namespace Gu.Wpf.NumericInput.Validation
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class Validator<T> : DependencyObject
        where T : struct, IComparable<T>, IFormattable, IConvertible, IEquatable<T>
    {
        private readonly ExplicitBinding<T> proxyBinding;
        private readonly NumericBox<T> numericBox;

        public Validator(NumericBox<T> numericBox, params ValidationRule[] rules)
        {
            this.numericBox = numericBox;
            this.proxyBinding = new ExplicitBinding<T>(numericBox, rules);

            this.numericBox.TextChanged += this.NumericBoxOnTextChanged;
            this.numericBox.ValueChanged += this.NumericBoxOnValueChanged;
            numericBox.ValidationDirty += this.OnValidationDirty;
            this.proxyBinding.ValidationFailed += this.OnValidationError;
            this.numericBox.LostFocus += this.OnLostFocus;
        }

        private BindingExpression ValueBinding => BindingOperations.GetBindingExpression(this.numericBox, NumericBox<T>.ValueProperty);

        private void NumericBoxOnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!this.IsTextChanged())
            {
                this.proxyBinding.ExplicitValidate();
                return;
            }

            this.numericBox.SetCurrentValue(ExplicitBinding.TextProxyProperty, this.numericBox.Text);
            if (!this.proxyBinding.IsUpdatingText)
            {
                if (!this.proxyBinding.HasValidationError)
                {
                    this.proxyBinding.UpdateValue();
                }
            }
            else
            {
                this.proxyBinding.ExplicitValidate();
            }
        }

        private void NumericBoxOnValueChanged(object sender, ValueChangedEventArgs<T?> e)
        {
            if (this.proxyBinding.IsUpdatingValue)
            {
                return;
            }

            this.proxyBinding.UpdateTextProxy();
        }

        private void OnLostFocus(object sender, RoutedEventArgs routedEventArgs)
        {
            if (!this.proxyBinding.HasValidationError)
            {
                this.proxyBinding.IsUpdatingText = true;
                this.numericBox.Text = this.numericBox.Value?.ToString(this.numericBox.StringFormat, this.numericBox.Culture) ?? string.Empty;
                this.proxyBinding.IsUpdatingText = false;
            }
        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            var expression = this.ValueBinding;
            if (expression != null)
            {
                this.proxyBinding.IsUpdatingValue = true;
                this.ValueBinding.UpdateTarget(); // Reset Value to value from from vm binding.
                this.proxyBinding.IsUpdatingValue = false;
            }
        }

        private void OnValidationDirty(object sender, RoutedEventArgs routedEventArgs)
        {
            this.proxyBinding.ExplicitValidate();
        }

        private bool IsTextChanged()
        {
            var viewText = this.numericBox.Text;
            var proxyText = (string)this.numericBox.GetValue(ExplicitBinding.TextProxyProperty);
            if (viewText == proxyText)
            {
                return false;
            }

            if (viewText.HasMoreDecimalDigitsThan(proxyText, this.numericBox))
            {
                return true;
            }

            return proxyText != viewText;
        }
    }
}
