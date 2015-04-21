namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    [ToolboxItem(true)]
    public class LongBox : NumericBox<long>
    {
        static LongBox()
        {
            UpdateMetadata(typeof(LongBox), 1);
            NumberStylesProperty.OverrideMetadata(typeof(LongBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.AllowExponent |
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

        public override bool CanParse(string s)
        {
            long d;
            return long.TryParse(s, NumberStyles.Integer, Culture, out d);
        }

        public override long Parse(string s)
        {
            return long.Parse(s, NumberStyles.Integer, Culture);
        }
    }
}