namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="decimal"/>
    /// </summary>
    [ToolboxItem(true)]
    public class DecimalBox : DecimalDigitsBox<decimal>
    {
        static DecimalBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalBox), new FrameworkPropertyMetadata(typeof(DecimalBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(DecimalBox), NumberStyles.Currency);
            IncrementProperty.OverrideMetadataWithDefaultValue<decimal>(typeof(DecimalBox), 1);
        }

        public DecimalBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        public override bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out decimal result)
        {
            return decimal.TryParse(text, numberStyles, culture, out result);
        }
    }
}