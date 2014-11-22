namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    [ToolboxItem(true)]
    public class DecimalBox : NumericBox<decimal>, IDecimals
    {
        /// <summary>
        /// Identifies the Decimals property
        /// </summary>
        public static readonly DependencyProperty DecimalDigitsProperty = DoubleBox.DecimalDigitsProperty.AddOwner(
            typeof(DecimalBox),
            new FrameworkPropertyMetadata(
                null,
                FrameworkPropertyMetadataOptions.None,
                OnDecimalsValueChanged,
                OnCoerceDecimalsValueChanged));

        static DecimalBox()
        {
            UpdateMetadata(typeof(DecimalBox), 1m);

        }

        public DecimalBox()
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
                return (int)GetValue(DecimalDigitsProperty);
            }
            set
            {
                SetValue(DecimalDigitsProperty, value);
            }
        }

        public override bool CanParse(string s)
        {
            decimal d;
            return decimal.TryParse(s, NumberStyles.Float, Culture, out d);
        }

        public override decimal Parse(string s)
        {
            return decimal.Parse(s, NumberStyles.Float, Culture);
        }
    }
}