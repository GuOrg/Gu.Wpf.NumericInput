namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class IntBox : NumericBox<int>
    {
        static IntBox()
        {
            UpdateMetadata(typeof(IntBox), 1);
        }

        public IntBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        protected override bool CanParse(string s)
        {
            int d;
            return int.TryParse(s, NumberStyles.Integer, Culture, out d);
        }

        protected override int Parse(string s)
        {
            return int.Parse(s, NumberStyles.Integer, Culture);
        }
    }
}