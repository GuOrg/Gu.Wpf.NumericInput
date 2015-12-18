namespace Gu.Wpf.NumericInput
{
    public interface IDecimals
    {
        /// <summary>
        /// Gets or sets the number of decimals to display in the UI, null uses default.
        /// Changing this changes stringformat.
        /// </summary>
        int? DecimalDigits { get; set; }
    }
}