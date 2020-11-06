namespace Gu.Wpf.NumericInput
{
    /// <summary>
    /// The source of the text in a numeric box.
    /// </summary>
    public enum TextSource
    {
        /// <summary>
        /// Could not determine source of the text.
        /// </summary>
        None,

        /// <summary>
        /// User input is the source of the text in the <see cref="System.Windows.Controls.TextBox"/>.
        /// </summary>
        UserInput,

        /// <summary>
        /// The text in the <see cref="System.Windows.Controls.TextBox"/> was last updated from binding.
        /// </summary>
        ValueBinding,
    }
}
