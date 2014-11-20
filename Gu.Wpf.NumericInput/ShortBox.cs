namespace Gu.Wpf.NumericInput
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

        protected override bool CanParse(string s)
        {
            short d;
            return short.TryParse(s, NumberStyles.Integer, Culture, out d);
        }

        protected override short Parse(string s)
        {
            return short.Parse(s, NumberStyles.Integer, Culture);
        }
    }
}