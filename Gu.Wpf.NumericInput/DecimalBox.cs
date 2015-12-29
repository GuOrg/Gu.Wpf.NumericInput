namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="decimal"/>
    /// </summary>
    [ToolboxItem(true)]
    public class DecimalBox : DecimalDigitsBox<decimal>
    {
        static DecimalBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalBox), new FrameworkPropertyMetadata(typeof(DecimalBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(DecimalBox), NumberStyles.Currency);
            IncrementProperty.OverrideMetadataWithDefaultValue<decimal>(typeof(DecimalBox), 1);
        }

        public DecimalBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool CanParse(string text)
        {
            decimal d;
            return decimal.TryParse(text, this.NumberStyles, this.Culture, out d);
        }

        public override decimal Parse(string text)
        {
            return decimal.Parse(text, this.NumberStyles, this.Culture);
        }
    }
}