namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using System.Runtime.CompilerServices;
    using Gu.Wpf.NumericInput.Demo;
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
            if (itemStatus == "HasValidationError: True")
            {
                return true;
            }

            if (itemStatus == "HasValidationError: False")
            {
                return false;
            }

            throw new InvalidOperationException();
        }

        internal static TextSource TextSource(this GroupBox groupBox)
        {
            TextSource result;
            var text = groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text;
            if (!Enum.TryParse(text, out result))
            {
                throw new ArgumentException();
            }

            return result;
        }

        internal static Status Status(this GroupBox groupBox)
        {
            Status result;
            var text = groupBox.Get<Label>(AutomationIds.StatusBlock).Text;
            if (!Enum.TryParse(text, out result))
            {
                throw new ArgumentException();
            }

            return result;
        }

        internal static string EditText(this TextBox textBox)
        {
            return textBox.Text;
        }

        internal static Button IncreaseButton(this TextBox textBox)
        {
            var parent = textBox.GetParent<CustomUIItem>();
            return parent.Get<Button>(BaseBox.IncreaseButtonName);
        }

        internal static Button DecreaseButton(this TextBox textBox)
        {
            var parent = textBox.GetParent<CustomUIItem>();
            return parent.Get<Button>(BaseBox.DecreaseButtonName);
        }

        internal static string FormattedText(this TextBox textBox)
        {
            var label = FormattedTextCache.GetValue(textBox, x => x.Get<Label>(BaseBox.FormattedName));
            return label.Text;
        }
    }
}
