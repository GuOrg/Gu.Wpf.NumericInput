namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="decimal"/>
    /// </summary>
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
                OnDecimalsValueChanged));

        static DecimalBox()
        {
            UpdateMetadata(typeof(DecimalBox), 1m);
            NumberStylesProperty.OverrideMetadata(
                typeof(DecimalBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.AllowDecimalPoint |
                    NumberStyles.AllowExponent |
                    NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite));
        }

        public DecimalBox()
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
            decimal d;
            return decimal.TryParse(text, this.NumberStyles, this.Culture, out d);
        }

        public override decimal Parse(string text)
        {
            return decimal.Parse(text, this.NumberStyles, this.Culture);
        }
    }
}