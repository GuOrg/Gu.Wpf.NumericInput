namespace Gu.Wpf.NumericInput
{
    /// <summary>
    /// Controls how the increment and decrement commands work.
    /// </summary>
    public enum SpinUpdateMode
    {
        /// <summary>
        /// Use the same as the binding i.e. LostFocus or PropertyChanged
        /// </summary>
        AsBinding,

        /// <summary>
        /// Update the binding source on click/execute.
        /// </summary>
        PropertyChanged
    }
}