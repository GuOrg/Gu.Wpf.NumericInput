namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using Gu.Wpf.UiAutomation;

    public static class TextBoxExt
    {
        public static TextBlock FormattedView(this TextBox textBox)
        {
            return textBox.FindTextBlock("PART_FormattedText");
        }

        public static string SelectedText(this TextBox textBox)
        {
            var selection = textBox.TextPattern.GetSelection();
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

        public static void ClearSelection(this TextBox textBox)
        {
            if (textBox.SelectedText().Length != 0)
            {
                textBox.TextPattern.DocumentRange.RemoveFromSelection();
            }
        }
    }
}
