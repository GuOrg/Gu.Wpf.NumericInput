namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using Gu.Wpf.UiAutomation;

    public static class TextBoxExt
    {
        public static TextBlock FormattedView(this TextBox baseBox)
        {
            return baseBox.FindTextBlock("PART_FormattedText");
        }

        public static string SelectedText(this TextBox baseBox)
        {
            var selection = baseBox.TextPattern.GetSelection();
            switch (selection.Length)
            {
                case 0:
                    return null;
                case 1:
                    return selection[0].GetText(int.MaxValue);
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
