namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    [ToolboxItem(true)]
    public class NullableDoubleBox : NumericBox<double?>, IDecimals
    {
        /// <summary>
        /// Identifies the Decimals property
        /// </summary>
        public static readonly DependencyProperty DecimalDigitsProperty = DependencyProperty.Register(
            "DecimalDigits",
            typeof(int?),
            typeof(NullableDoubleBox),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                OnDecimalsValueChanged,
                OnCoerceDecimalsValue));

        static NullableDoubleBox()
        {
            UpdateMetadata(typeof(NullableDoubleBox), 1d);
            NumberStylesProperty.OverrideMetadata(typeof(NullableDoubleBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.AllowDecimalPoint |
                    NumberStyles.AllowExponent |
                    NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite));
        }

        public NullableDoubleBox()
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

        public override double? Parse(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return null;
            }
            return double.Parse(s, NumberStyles.Float, Culture);
        }
    }
}