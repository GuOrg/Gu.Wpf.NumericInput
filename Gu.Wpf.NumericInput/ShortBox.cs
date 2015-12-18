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
            NumberStylesProperty.OverrideMetadata(
                typeof(ShortBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.Integer |
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

        public override bool CanParse(string text)
        {
            short d;
            return short.TryParse(text, NumberStyles.Integer, this.Culture, out d);
        }

        public override short Parse(string text)
        {
            return short.Parse(text, NumberStyles.Integer, this.Culture);
        }
    }
}