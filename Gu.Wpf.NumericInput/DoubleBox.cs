namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="double"/>
    /// </summary>
    [ToolboxItem(true)]
    public class DoubleBox : DecimalDigitsBox<double>, IDecimals
    {
        static DoubleBox()
        {
            UpdateMetadata(typeof(DoubleBox), 1d);
            NumberStylesProperty.OverrideMetadata(
                typeof(DoubleBox),
                new PropertyMetadata(NumberStyles.Float));
        }

        public DoubleBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool CanParse(string text)
        {
            double d;
            return double.TryParse(text, this.NumberStyles, this.Culture, out d);
        }

        public override double Parse(string text)
        {
            return double.Parse(text, this.NumberStyles, this.Culture);
        }
    }
}