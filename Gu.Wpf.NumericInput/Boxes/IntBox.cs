namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>A <see cref="System.Windows.Controls.TextBox"/> for input of <see cref="int"/>.</summary>
    [ToolboxItem(true)]
    public class IntBox : NumericBox<int>
    {
        static IntBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(IntBox), new FrameworkPropertyMetadata(typeof(IntBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<int>), typeof(IntBox), NumberStyles.Integer);
            IncrementProperty.OverrideMetadataWithDefaultValue(typeof(IntBox), 1);
        }

        /// <inheritdoc />
        public override bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out int result)
        {
            return int.TryParse(text, numberStyles, culture, out result);
        }

        /// <inheritdoc />
        protected override int Add(int x, int y)
        {
            return x + y;
        }

        /// <inheritdoc />
        protected override int Subtract(int x, int y)
        {
            return x - y;
        }

        /// <inheritdoc />
        protected override int TypeMin()
        {
            return int.MinValue;
        }

        /// <inheritdoc />
        protected override int TypeMax()
        {
            return int.MaxValue;
        }
    }
}
