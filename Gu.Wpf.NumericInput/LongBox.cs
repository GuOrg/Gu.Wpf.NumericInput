namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="long"/>
    /// </summary>
    [ToolboxItem(true)]
    public class LongBox : NumericBox<long>
    {
        static LongBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(LongBox), new FrameworkPropertyMetadata(typeof(LongBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<long>), typeof(LongBox), NumberStyles.Integer);
            IncrementProperty.OverrideMetadataWithDefaultValue<long>(typeof(LongBox), 1);
        }

        public override bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out long result)
        {
            return long.TryParse(text, numberStyles, culture, out result);
        }

        protected override long Add(long x, long y)
        {
            return x + y;
        }

        protected override long Subtract(long x, long y)
        {
            return x - y;
        }

        protected override long TypeMin()
        {
            return long.MinValue;
        }

        protected override long TypeMax()
        {
            return long.MaxValue;
        }
    }
}