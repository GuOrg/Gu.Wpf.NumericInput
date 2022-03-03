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

        public static string? SelectedText(this TextBox textBox)
        {
            var selection = textBox.TextPattern.GetSelection();
            return selection.Length switch
            {
                0 => null,
                1 => selection[0].GetText(int.MaxValue),
                _ => throw new InvalidOperationException(),
            };
        }
    }
}
