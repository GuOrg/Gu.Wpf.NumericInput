namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class DoubleBox : NumericBox<double>
    {
        static DoubleBox()
        {
            UpdateMetadata(typeof(DoubleBox), 1d);
        }

        public DoubleBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool CanParse(string s)
        {
            double d;
            return double.TryParse(s, NumberStyles.Float, Culture, out d);
        }

        public override double Parse(string s)
        {
            return double.Parse(s, NumberStyles.Float, Culture);
        }
    }
}