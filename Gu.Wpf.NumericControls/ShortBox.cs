namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class ShortBox : NumericBox<short>
    {
        static ShortBox()
        {
            UpdateMetadata(typeof(ShortBox), 1);
        }

        public ShortBox()
            : base(
            (x, y) => (short)(x + y), 
            (x, y) => (short)(x - y))
        {
        }

        protected override bool CanParse(string s, IFormatProvider provider)
        {
            short d;
            return short.TryParse(s, NumberStyles.Integer, provider, out d);
        }

        protected override short Parse(string s, IFormatProvider provider)
        {
            return short.Parse(s, NumberStyles.Integer, provider);
        }
    }
}