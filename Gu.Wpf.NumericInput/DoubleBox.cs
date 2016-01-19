namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="double"/>
    /// </summary>
    [ToolboxItem(true)]
    public class DoubleBox : DecimalDigitsBox<double>
    {
        static DoubleBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleBox), new FrameworkPropertyMetadata(typeof(DoubleBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<double>), typeof(DoubleBox), NumberStyles.Float);
            IncrementProperty.OverrideMetadataWithDefaultValue<double>(typeof(DoubleBox), 1);
        }

        public DoubleBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out double result)
        {
            return double.TryParse(text, numberStyles, culture, out result);
        }
    }
}