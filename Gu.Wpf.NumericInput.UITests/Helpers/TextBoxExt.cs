namespace Gu.Wpf.NumericInput.UITests
{
    using FlaUI.Core.AutomationElements;

    public static class TextBoxExt
    {
        public static Label FormattedView(this TextBox baseBox)
        {
            return baseBox.FindLabel("PART_FormattedText");
        }
    }
}