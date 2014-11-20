namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class LongBox : NumericBox<long>
    {
        static LongBox()
        {
            UpdateMetadata(typeof(LongBox), 1);
        }

        public LongBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        protected override bool CanParse(string s, IFormatProvider provider)
        {
            long d;
            return long.TryParse(s, NumberStyles.Integer, provider, out d);
        }

        protected override long Parse(string s, IFormatProvider provider)
        {
            return long.Parse(s, NumberStyles.Integer, provider);
        }
    }
}