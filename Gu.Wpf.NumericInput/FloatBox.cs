namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="float"/>
    /// </summary>
    [ToolboxItem(true)]
    public class FloatBox : DecimalDigitsBox<float>
    {
        static FloatBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FloatBox), new FrameworkPropertyMetadata(typeof(FloatBox)));
            NumberStylesProperty.OverrideMetadataWithDefaultValue(typeof(NumericBox<float>), typeof(FloatBox), NumberStyles.Float);
            IncrementProperty.OverrideMetadataWithDefaultValue<float>(typeof(FloatBox), 1);
        }

        public override bool TryParse(string text, NumberStyles numberStyles, IFormatProvider culture, out float result)
        {
            return float.TryParse(text, numberStyles, culture, out result);
        }

        protected override float Add(float x, float y)
        {
            return x + y;
        }

        protected override float Subtract(float x, float y)
        {
            return x - y;
        }

        protected override float TypeMin()
        {
            return float.MinValue;
        }

        protected override float TypeMax()
        {
            return float.MaxValue;
        }
    }
}