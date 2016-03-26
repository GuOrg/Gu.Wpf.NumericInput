namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Windows;

    using TestStack.White.UIItems;
    using TestStack.White.UIItems.Custom;
    using TestStack.White.UIItems.WPFUIItems;

    public static class UiItemExt
    {
        private static readonly ConditionalWeakTable<TextBox, Label> FormattedTextCache = new ConditionalWeakTable<TextBox, Label>();

        public static string ItemStatus(this IUIItem item)
        {
            return (string)item.AutomationElement.Current.ItemStatus;
        }

        public static bool HasValidationError(this UIItem item)
        {
            var itemStatus = item.ItemStatus();
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

        internal static TextSource TextSource(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus();
            var text = itemStatus.Get(BaseBox.TextSourceProperty);
            TextSource result;
            if (!Enum.TryParse(text, out result))
            {
                throw new ArgumentException();
            }

            return result;
        }

        internal static Status Status(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus();
            var text = itemStatus.Get(BaseBox.StatusProperty);
            Status result;

            if (!Enum.TryParse(text, out result))
            {
                throw new ArgumentException();
            }

            return result;
        }

        internal static string Value(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus();
            var text = itemStatus.Get(DoubleBox.ValueProperty);
            return text;
        }

        internal static string EditText(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus();
            var text = itemStatus.Get(System.Windows.Controls.TextBox.TextProperty);
            return text;
        }

        internal static string FormattedText(this TextBox textBox)
        {
            var itemStatus = textBox.ItemStatus();
            var text = itemStatus.Get(BaseBox.FormattedTextProperty);
            return text;
        }

        internal static Button IncreaseButton(this TextBox textBox)
        {
            var parent = textBox.GetParent<CustomUIItem>();
            return parent.Get<Button>(SpinnerDecorator.IncreaseButtonName);
        }

        internal static Button DecreaseButton(this TextBox textBox)
        {
            var parent = textBox.GetParent<CustomUIItem>();
            return parent.Get<Button>(SpinnerDecorator.DecreaseButtonName);
        }

        private static string Get(this string text, DependencyProperty property)
        {
            return text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                .Single(x => x.StartsWith(property.Name + ":"))
                .Split(':')[1]
                .Trim();

        }
    }
}
