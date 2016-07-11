namespace Gu.Wpf.NumericInput.UITests
{
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.WPFUIItems;

    public static class TextBoxExt
    {
        public static Label FormattedView(this TextBox baseBox)
        {
            return baseBox.Get<Label>("PART_FormattedText");
        }
    }
}