namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class FloatBox : NumericBox<float>
    {
        static FloatBox()
        {
            UpdateMetadata(typeof(FloatBox), 1f);
        }

        public FloatBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        protected override bool CanParse(string s, IFormatProvider provider)
        {
            float d;
            return float.TryParse(s, NumberStyles.Float, provider, out d);
        }

        protected override float Parse(string s, IFormatProvider provider)
        {
            return float.Parse(s, NumberStyles.Float, provider);
        }
    }
}