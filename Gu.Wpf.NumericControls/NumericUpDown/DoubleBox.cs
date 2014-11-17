namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(true)]
    public class DoubleBox : NumericBox<double>
    {
        static DoubleBox()
        {
            UpdateMetadata(typeof(DoubleBox), 1d, double.MinValue, double.MaxValue, 2);
        }

        public DoubleBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
        {
        }

        protected override void ValidateText(string txt)
        {
            double result;
            if (double.TryParse(txt, out result))
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
            return this.ValidateValue(Convert.ToDouble(text))
                .ToString() == text;
        }

        protected override void ConvertValueToText()
        {
            if (this.TextBox != null)
            {
                this.TextBox.Text = this.TextBox.Text = String.Format("{0} {1}",this.Value.ToString("F" + this.Decimals), this.Suffix);
            }
        }
    }
}