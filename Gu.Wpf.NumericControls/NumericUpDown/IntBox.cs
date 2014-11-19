namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

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

        protected override bool CanParse(string s, IFormatProvider provider)
        {
            int d;
            return int.TryParse(s, NumberStyles.Integer, provider, out d);
        }

        protected override int Parse(string s, IFormatProvider provider)
        {
            return int.Parse(s, NumberStyles.Integer, provider);
        }
    }
}