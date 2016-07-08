namespace Gu.Wpf.NumericInput
{
    public enum TextSource
    {
        /// <summary>
        /// Could not determine source of the text
        /// </summary>
        None,

        /// <summary>
        /// User input is the source of the text in the textbox
        /// </summary>
        UserInput,

        /// <summary>
        /// The text in the textbox was last updated from binding
        /// </summary>
        ValueBinding
    }
}