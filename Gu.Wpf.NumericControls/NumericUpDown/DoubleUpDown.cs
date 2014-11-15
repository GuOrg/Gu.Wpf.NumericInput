namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(true)]
    public class DoubleUpDown : CommonUpDown<double>
    {
        static DoubleUpDown()
        {
            UpdateMetadata(typeof(DoubleUpDown), 1d, double.NegativeInfinity, double.PositiveInfinity, 2);
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
        protected override double IncrementValue(double value, double increment)
        {
            return value + increment;
        }
        protected override double DecrementValue(double value, double increment)
        {
            return value - increment;
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