namespace Gu.Wpf.NumericInput
{
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

        public override bool CanParse(string s)
        {
            int d;
            return int.TryParse(s, NumberStyles.Integer, Culture, out d);
        }

        public override int Parse(string s)
        {
            return int.Parse(s, NumberStyles.Integer, Culture);
        }
    }
}