namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows;

    /// <summary>
    /// A <see cref="System.Windows.Controls.TextBox"/> for inut of <see cref="float"/>
    /// </summary>
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
                OnDecimalsValueChanged));

        static FloatBox()
        {
            UpdateMetadata(typeof(FloatBox), 1f);
            NumberStylesProperty.OverrideMetadata(
                typeof(DecimalBox),
                new FrameworkPropertyMetadata(
                    NumberStyles.AllowDecimalPoint |
                    NumberStyles.AllowExponent |
                    NumberStyles.AllowLeadingSign |
                    NumberStyles.AllowLeadingWhite |
                    NumberStyles.AllowTrailingWhite));
        }

        public FloatBox()
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
            float d;
            return float.TryParse(text, this.NumberStyles, this.Culture, out d);
        }

        public override float Parse(string text)
        {
            return float.Parse(text, this.NumberStyles, this.Culture);
        }
    }
}