namespace Gu.Wpf.NumericControls
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
        
        protected override bool CanParse(string s, IFormatProvider provider)
        {
            double d;
            return double.TryParse(s, NumberStyles.Float, provider, out d);
        }

        protected override double Parse(string s, IFormatProvider provider)
        {
            return double.Parse(s, NumberStyles.Float, provider);
        }
    }
}