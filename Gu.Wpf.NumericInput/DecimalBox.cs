namespace Gu.Wpf.NumericInput
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

        public override bool CanParse(string s)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Float, Culture, out d);
        }

        public override decimal Parse(string s)
        {
            return decimal.Parse(s, NumberStyles.Float, Culture);
        }
    }
}