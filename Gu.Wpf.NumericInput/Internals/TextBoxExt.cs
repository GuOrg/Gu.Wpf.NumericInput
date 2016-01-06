namespace Gu.Wpf.NumericInput
{
    using System.Windows.Controls;

    internal static class TextBoxExt
    {
        internal static void SetTextUndoable(this TextBox textBox, string text)
        {
            // http://stackoverflow.com/questions/27083236/change-the-text-in-a-textbox-with-text-binding-sometext-so-it-is-undoable/27083548?noredirect=1#comment42677255_27083548
            // Dunno if nice, testing it for now
            textBox.SelectAll();
            textBox.SelectedText = text;
            textBox.Select(0, 0);
        }
    }
}
