namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(true)]
    public class DecimalBox : NumericBox<decimal>
    {
        static DecimalBox()
        {
            UpdateMetadata(typeof(DecimalBox), 1m, decimal.MinValue, decimal.MaxValue, 2);
        }

        public DecimalBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        protected override void ValidateText(string txt)
        {
            decimal result;
            if (decimal.TryParse(txt, out result))
            {
                this.Value = result;
            }
            else
            {
                this.ConvertValueToText();
            }
        }

        protected override string ValidInput()
        {
            return @"^[-]?[\d]*[,.]?[\d]{0," + this.Decimals.ToString() + "}?$";
        }

        protected override bool ValidValue(string text)
        {
            return this.ValidateValue(Convert.ToDecimal(text))
                .ToString() == text;
        }

        protected override void ConvertValueToText()
        {
            this.TextBox.Text = string.Format("{0:0." + new string('0', this.Decimals) + "} {1}", Math.Round(this.Value, this.Decimals), this.Suffix);
        }
    }
}