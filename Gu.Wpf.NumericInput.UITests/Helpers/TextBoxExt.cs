namespace Gu.Wpf.NumericInput.UITests
{
    using Gu.Wpf.UiAutomation;

    public static class TextBoxExt
    {
        public static TextBlock FormattedView(this TextBox baseBox)
        {
            return baseBox.FindTextBlock("PART_FormattedText");
        }
    }
}