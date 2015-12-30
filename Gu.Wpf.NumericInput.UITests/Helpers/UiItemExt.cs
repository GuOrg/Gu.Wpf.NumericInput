namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using TestStack.White.UIItems;

    public static class UiItemExt
    {
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
    }
}
