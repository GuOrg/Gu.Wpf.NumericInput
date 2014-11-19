namespace Gu.Wpf.NumericControls
{
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
    }
}