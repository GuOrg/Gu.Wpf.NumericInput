namespace Gu.Wpf.NumericControls.Validation
{
    using System;
    using System.Globalization;
    using System.Windows.Controls;
    using System.Windows.Data;

    public class ClearErrors<T> : ValidationRule
        where T : struct, IComparable<T>, IFormattable
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
        {
            Validation.ClearInvalid(owner);
            var dependencyObject = owner.Target;
            var bindingExpression = BindingOperations.GetBindingExpression(owner.Target, NumericBox<T>.ValueProperty);
            if (bindingExpression != null)
            {
                Validation.ClearInvalid(bindingExpression);
            }
            bindingExpression = BindingOperations.GetBindingExpression(owner.Target, TextBox.TextProperty);
            if (bindingExpression != null)
            {
                Validation.ClearInvalid(bindingExpression);
            }
            return base.Validate(value, cultureInfo, owner);
        }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return ValidationResult.ValidResult;
        }
    }
}