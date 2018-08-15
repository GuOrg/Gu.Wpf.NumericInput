namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>A <see cref="System.Windows.Controls.TextBox"/> for input of <see cref="double"/> or nullable double.</summary>
    [ToolboxItem(true)]
    public class DoubleBox : DecimalDigitsBox<double>
    {
        static DoubleBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DoubleBox), new FrameworkPropertyMetadata(typeof(DoubleBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<double>), typeof(DoubleBox), NumberStyles.Float);
            IncrementProperty.OverrideMetadataWithDefaultValue<double>(typeof(DoubleBox), 1);
        }

        /// <inheritdoc />
        public override bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out double result)
        {
            return double.TryParse(text, numberStyles, culture, out result);
        }

        /// <inheritdoc />
        protected override double Add(double x, double y)
        {
            return x + y;
        }

        /// <inheritdoc />
        protected override double Subtract(double x, double y)
        {
            return x - y;
        }

        /// <inheritdoc />
        protected override double TypeMin()
        {
            return double.MinValue;
        }

        /// <inheritdoc />
        protected override double TypeMax()
        {
            return double.MaxValue;
        }
    }
}
