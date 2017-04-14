namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>A <see cref="System.Windows.Controls.TextBox"/> for input of <see cref="decimal"/>.</summary>
    [ToolboxItem(true)]
    public class DecimalBox : DecimalDigitsBox<decimal>
    {
        static DecimalBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DecimalBox), new FrameworkPropertyMetadata(typeof(DecimalBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<decimal>), typeof(DecimalBox), NumberStyles.Currency);
            IncrementProperty.OverrideMetadataWithDefaultValue<decimal>(typeof(DecimalBox), 1);
        }

        public override bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out decimal result)
        {
            return decimal.TryParse(text, numberStyles, culture, out result);
        }

        protected override decimal Add(decimal x, decimal y)
        {
            return x + y;
        }

        protected override decimal Subtract(decimal x, decimal y)
        {
            return x - y;
        }

        protected override decimal TypeMin()
        {
            return decimal.MinValue;
        }

        protected override decimal TypeMax()
        {
            return decimal.MaxValue;
        }
    }
}