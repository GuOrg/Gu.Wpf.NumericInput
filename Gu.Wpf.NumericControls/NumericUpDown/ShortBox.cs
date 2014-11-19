namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class ShortBox : NumericBox<short>
    {
        static ShortBox()
        {
            UpdateMetadata(typeof(ShortBox), 1);
        }

        public ShortBox()
            : base(
            (x, y) => (short)(x + y), 
            (x, y) => (short)(x - y))
        {
        }
    }
}