namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;

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
    }
}