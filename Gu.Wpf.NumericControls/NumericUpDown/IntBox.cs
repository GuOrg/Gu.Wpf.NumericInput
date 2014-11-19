namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Windows;

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
    }
}