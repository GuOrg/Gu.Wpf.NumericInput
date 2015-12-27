namespace Gu.Wpf.NumericInput.UITests.Helpers
{
    using System;
    using System.Windows.Automation;
    using TestStack.White.UIItems;

    public static class UiItemExt
    {
        public static string ItemStatus(this IUIItem item)
        {
            return (string)item.AutomationElement.GetCurrentPropertyValue(AutomationElementIdentifiers.ItemStatusProperty);
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
    }
}
