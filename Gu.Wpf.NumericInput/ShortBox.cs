namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    [ToolboxItem(true)]
    public class ShortBox : NumericBox<short>
    {
        static ShortBox()
        {
            UpdateMetadata(typeof(ShortBox), 1);
            NumberStylesProperty.OverrideMetadata(typeof(ShortBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.AllowExponent |
                    NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite));
        }

        public ShortBox()
            : base(
            (x, y) => (short)(x + y), 
            (x, y) => (short)(x - y))
        {
        }

        public override bool CanParse(string s)
        {
            short d;
            return short.TryParse(s, NumberStyles.Integer, Culture, out d);
        }

        public override short Parse(string s)
        {
            return short.Parse(s, NumberStyles.Integer, Culture);
        }
    }
}