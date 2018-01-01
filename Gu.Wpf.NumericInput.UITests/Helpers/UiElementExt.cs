namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using System.Linq;
    using System.Windows;
    using Gu.Wpf.UiAutomation;

    public static class UiElementExt
    {
        public static bool HasValidationError(this Gu.Wpf.UiAutomation.UiElement item)
        {
            var itemStatus = item.ItemStatus;
            if (itemStatus.Contains("HasError: True"))
            {
                return true;
            }

            if (itemStatus.Contains("HasError: False"))
            {
                return false;
            }

            throw new InvalidOperationException();
        }

        public static string ValidationError(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus;
            var text = itemStatus.Get("FirstError");
            return text;
        }

        internal static TextSource TextSource(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus;
            var text = itemStatus.Get(BaseBox.TextSourceProperty);
            if (!Enum.TryParse(text, out TextSource result))
            {
                throw new ArgumentException();
            }

            return result;
        }

        internal static string Status(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus;
            return itemStatus.Get("Status");
        }

        internal static string Value(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus;
            var text = itemStatus.Get(NumericBox<double>.ValueProperty);
            return text;
        }

        internal static string EditText(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus;
            var text = itemStatus.Get(System.Windows.Controls.TextBox.TextProperty);
            return text;
        }

        internal static string FormattedText(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus;
            var text = itemStatus.Get(BaseBox.FormattedTextProperty);
            return text;
        }

        internal static Button IncreaseButton(this TextBox textBox)
        {
            return textBox.Parent
                          .FindButton(SpinnerDecorator.IncreaseButtonName);
        }

        internal static Button DecreaseButton(this TextBox textBox)
        {
            return textBox.Parent
                          .FindButton(SpinnerDecorator.DecreaseButtonName);
        }

        private static string Get(this string text, DependencyProperty property)
        {
            return text.Get(property.Name);
        }

        private static string Get(this string text, string property)
        {
            return text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                       .Single(x => x.StartsWith(property + ":"))
                       .Split(':')[1].Trim();
        }
    }
}
