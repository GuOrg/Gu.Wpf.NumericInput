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
            UpdateMetadata(typeof(LongBox), 1);
            NumberStylesProperty.OverrideMetadata(
                typeof(LongBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.Integer |
                    NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite));
        }

        public LongBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool CanParse(string text)
        {
            long d;
            return long.TryParse(text, this.NumberStyles, this.Culture, out d);
        }

        public override long Parse(string text)
        {
            return long.Parse(text, this.NumberStyles, this.Culture);
        }
    }
}