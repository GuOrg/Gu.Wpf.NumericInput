namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="double"/>
    /// </summary>
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
                OnDecimalsValueChanged));

        static DoubleBox()
        {
            UpdateMetadata(typeof(DoubleBox), 1d);
            NumberStylesProperty.OverrideMetadata(
                typeof(DoubleBox),
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

        /// <inheritdoc/>
        [Description("")]
        [Category("NumericBox")]
        [Browsable(true)]
        public int? DecimalDigits
        {
            get { return (int?)this.GetValue(DecimalDigitsProperty); }
            set { this.SetValue(DecimalDigitsProperty, value); }
        }

        public override bool CanParse(string text)
        {
            double d;
            return double.TryParse(text, this.NumberStyles, this.Culture, out d);
        }

        public override double Parse(string text)
        {
            return double.Parse(text, this.NumberStyles, this.Culture);
        }
    }
}