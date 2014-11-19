namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;

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
    }
}