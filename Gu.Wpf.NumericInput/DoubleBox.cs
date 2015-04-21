namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    [ToolboxItem(true)]
    public class DoubleBox : NumericBox<double>, IDecimals
    {
        /// <summary>
        /// Identifies the Decimals property
        /// </summary>
        public static readonly DependencyProperty DecimalDigitsProperty = DependencyProperty.Register(
            "DecimalDigits",
            typeof(int?),
            typeof(DoubleBox),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                OnDecimalsValueChanged,
                OnCoerceDecimalsValue));

        static DoubleBox()
        {
            UpdateMetadata(typeof(DoubleBox), 1d);
            NumberStylesProperty.OverrideMetadata(typeof(DoubleBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.AllowDecimalPoint |
                    NumberStyles.AllowExponent |
                    NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite));
        }

        public DoubleBox()
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
            double d;
            return double.TryParse(s, NumberStyles.Float, Culture, out d);
        }

        public override double Parse(string s)
        {
            return double.Parse(s, NumberStyles.Float, Culture);
        }
    }
}