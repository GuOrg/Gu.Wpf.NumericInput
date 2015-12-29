namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="double"/>
    /// </summary>
    [ToolboxItem(true)]
    public class DoubleBox : DecimalDigitsBox<double>
    {
        static DoubleBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleBox), new FrameworkPropertyMetadata(typeof(DoubleBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(DoubleBox), NumberStyles.Float);
            IncrementProperty.OverrideMetadataWithDefaultValue<double>(typeof(DoubleBox), 1);
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