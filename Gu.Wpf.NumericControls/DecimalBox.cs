namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class DecimalBox : NumericBox<decimal>
    {
        static DecimalBox()
        {
            UpdateMetadata(typeof(DecimalBox), 1m);
        }

        public DecimalBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        protected override bool CanParse(string s, IFormatProvider provider)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Float, provider, out d);
        }

        protected override decimal Parse(string s, IFormatProvider provider)
        {
            return decimal.Parse(s, NumberStyles.Float, provider);
        }
    }
}