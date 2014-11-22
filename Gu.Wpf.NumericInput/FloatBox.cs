namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    [ToolboxItem(true)]
    public class FloatBox : NumericBox<float>, IDecimals
    {
        /// <summary>
        /// Identifies the Decimals property
        /// </summary>
        public static readonly DependencyProperty DecimalDigitsProperty = DoubleBox.DecimalDigitsProperty.AddOwner(
            typeof(FloatBox),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                OnDecimalsValueChanged,
                OnCoerceDecimalsValue));

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

        /// <summary>
        /// The number of decimals to display in the UI, null uses default.
        /// </summary>
        [Description(""), Category("NumericBox"), Browsable(true)]
        public int? DecimalDigits
        {
            get
            {
                return (int?)GetValue(DecimalDigitsProperty);
            }
            set
            {
                SetValue(DecimalDigitsProperty, value);
            }
        }

        public override bool CanParse(string s)
        {
            float d;
            return float.TryParse(s, NumberStyles.Float, Culture, out d);
        }

        public override float Parse(string s)
        {
            return float.Parse(s, NumberStyles.Float, Culture);
        }
    }
}