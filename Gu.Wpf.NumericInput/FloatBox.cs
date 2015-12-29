namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="float"/>
    /// </summary>
    [ToolboxItem(true)]
    public class FloatBox : DecimalDigitsBox<float>
    {
        static FloatBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatBox), new FrameworkPropertyMetadata(typeof(FloatBox)));
            NumberStylesProperty.OverrideMetadata(
                typeof(FloatBox),
                new PropertyMetadata(NumberStyles.Float));
            IncrementProperty.OverrideMetadataWithDefaultValue<float>(typeof(FloatBox), 1);
        }

        public FloatBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool CanParse(string text)
        {
            float d;
            return float.TryParse(text, this.NumberStyles, this.Culture, out d);
        }

        public override float Parse(string text)
        {
            return float.Parse(text, this.NumberStyles, this.Culture);
        }
    }
}