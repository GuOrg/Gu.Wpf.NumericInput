namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="int"/>
    /// </summary>
    [ToolboxItem(true)]
    public class IntBox : NumericBox<int>
    {
        static IntBox()
        {
            UpdateMetadata(typeof(IntBox), 1);
            NumberStylesProperty.OverrideMetadata(
                typeof(IntBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.Integer |
                    NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite));
        }

        public IntBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool CanParse(string text)
        {
            int d;
            return int.TryParse(text, this.NumberStyles, this.Culture, out d);
        }

        public override int Parse(string text)
        {
            return int.Parse(text, this.NumberStyles, this.Culture);
        }
    }
}