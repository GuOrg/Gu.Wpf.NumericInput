namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;

    internal static class Validator
    {
        private static readonly object Default = new object();

        internal static void UpdateValidation(BaseBox box)
        {
            ValidateAndGetValue(box);
        }

        internal static object ValidateAndGetValue(BaseBox box)
        {
            var converter = box.TextValueConverter;
            var expression = box.TextBindingExpression;

            if (converter == null)
            {
                Validation.MarkInvalid(expression, new ValidationError(IsMatch.FromText, expression.ParentBinding, $"{BaseBox.TextValueConverterProperty.Name} == null", null));
                return Binding.DoNothing;
            }

            bool validated = false;
            var value = Default;
            foreach (var rule in box.ValidationRules)
            {
                switch (rule.ValidationStep)
                {
                    case ValidationStep.RawProposedValue:
                        {
                            var result = rule.Validate(box.Text, GetCulture(expression), expression);
                            if (!result.IsValid)
                            {
                                Validation.MarkInvalid(expression, new ValidationError(rule, expression.ParentBinding, result, null));
                                return Binding.DoNothing;
                            }

                            validated = true;
                            break;
                        }

                    case ValidationStep.UpdatedValue:
                    case ValidationStep.CommittedValue:
                    case ValidationStep.ConvertedProposedValue:
                        {
                            if (value == Default)
                            {
                                value = converter.Convert(box.Text, null, box, GetCulture(expression));
                            }

                            if (value == Binding.DoNothing)
                            {
                                continue;
                            }

                            var result = rule.Validate(value, GetCulture(expression), expression);
                            if (!result.IsValid)
                            {
                                Validation.MarkInvalid(expression, new ValidationError(rule, expression.ParentBinding, result, null));
                                return Binding.DoNothing;
                            }

                            validated = true;
                            break;
                        }

                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (validated)
            {
                Validation.ClearInvalid(expression);
            }

            if (value == Default)
            {
                value = converter.Convert(box.Text, null, box, GetCulture(expression));
            }

            return value;
        }

        private static CultureInfo GetCulture(BindingExpression expression)
        {
            if (expression.ParentBinding.ConverterCulture != null)
            {
                return expression.ParentBinding.ConverterCulture;
            }

            return ((XmlLanguage)(expression.Target as BaseBox)?.GetValue(FrameworkElement.LanguageProperty))
                    ?.GetSpecificCulture();
        }
    }
}
