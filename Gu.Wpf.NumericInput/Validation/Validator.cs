namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Diagnostics;
    using System.Globalization;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Markup;

    internal static class Validator
    {
        private static readonly object Default = new object();

        static Validator()
        {
            EventManager.RegisterClassHandler(typeof(BaseBox), BaseBox.ValidationDirtyEvent, new RoutedEventHandler(OnValidationDirty));
        }

        internal static void Start()
        {
            // nop to trigger static ctor
        }

        private static void OnValidationDirty(object sender, RoutedEventArgs e)
        {
            var box = (BaseBox)sender;
            var converter = box.TextValueConverter;
            var expression = box.TextBindingExpression;

            if (converter == null)
            {
                Validation.MarkInvalid(expression, new ValidationError(IsMatch.FromText, expression.ParentBinding, $"{BaseBox.TextValueConverterProperty.Name} == null", null));
                return;
            }
            bool validated = false;
            var value = Default;
            foreach (var rule in box.ValidationRules)
            {
                if (!ShouldValidate(rule, box.TextSource))
                {
                    continue;
                }
                switch (rule.ValidationStep)
                {
                    case ValidationStep.RawProposedValue:
                        {
                            var result = rule.Validate(box.Text, GetCulture(expression), expression);
                            if (!result.IsValid)
                            {
                                Validation.MarkInvalid(expression, new ValidationError(rule, expression.ParentBinding, result, null));
                                return;
                            }

                            validated = true;
                            break;
                        }

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
                                return;
                            }

                            validated = true;
                            break;
                        }

                    case ValidationStep.UpdatedValue:
                    case ValidationStep.CommittedValue:
                        throw new NotSupportedException("Only rules with ValidationStep RawProposedValue or ConvertedProposedValue are allowed");
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (validated)
            {
                Validation.ClearInvalid(expression);
            }
        }

        private static bool ShouldValidate(ValidationRule rule, TextSource source)
        {
            if (source == TextSource.None)
            {
                return false;
            }

            return rule.ValidatesOnTargetUpdated && source == TextSource.ValueBinding ||
                   !rule.ValidatesOnTargetUpdated && source == TextSource.UserInput;
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
