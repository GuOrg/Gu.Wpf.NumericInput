namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="long"/>
    /// </summary>
    [ToolboxItem(true)]
    public class LongBox : NumericBox<long>
    {
        static LongBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LongBox), new FrameworkPropertyMetadata(typeof(LongBox)));
            NumberStylesProperty.OverrideMetadata(
                typeof(LongBox),
                new PropertyMetadata(NumberStyles.Integer));
            IncrementProperty.OverrideMetadataWithDefaultValue<long>(typeof(LongBox), 1);
        }

        public LongBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool TryParse(string text, out long result)
        {
            return long.TryParse(text, this.NumberStyles, this.Culture, out result);
        }
    }
}