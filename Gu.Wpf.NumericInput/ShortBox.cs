namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    [ToolboxItem(true)]
    public class ShortBox : NumericBox<short>
    {
        static ShortBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ShortBox), new FrameworkPropertyMetadata(typeof(ShortBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<short>), typeof(ShortBox), NumberStyles.Integer);
            IncrementProperty.OverrideMetadataWithDefaultValue<short>(typeof(ShortBox), 1);
        }

        public ShortBox()
            : base(
            (x, y) => (short)(x + y),
            (x, y) => (short)(x - y))
        {
        }

        public override bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out short result)
        {
            return short.TryParse(text, numberStyles, culture, out result);
        }
    }
}