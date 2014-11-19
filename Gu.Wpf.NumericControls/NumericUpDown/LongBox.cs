namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(true)]
    public class LongBox : NumericBox<long>
    {
        static LongBox()
        {
            UpdateMetadata(typeof(LongBox), 1);
        }

        public LongBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }
    }
}