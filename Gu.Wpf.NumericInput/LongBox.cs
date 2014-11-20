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

        protected override bool CanParse(string s)
        {
            long d;
            return long.TryParse(s, NumberStyles.Integer, Culture, out d);
        }

        protected override long Parse(string s)
        {
            return long.Parse(s, NumberStyles.Integer, Culture);
        }
    }
}