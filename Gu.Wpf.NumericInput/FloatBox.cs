namespace Gu.Wpf.NumericInput
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

        public override bool CanParse(string s)
        {
            float d;
            return float.TryParse(s, NumberStyles.Float, Culture, out d);
        }

        public override float Parse(string s)
        {
            return float.Parse(s, NumberStyles.Float, Culture);
        }
    }
}