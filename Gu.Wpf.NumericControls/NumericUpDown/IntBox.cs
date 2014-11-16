namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    [ToolboxItem(true)]
    public class IntBox : NumericBox<int>
    {
        static IntBox()
        {
            UpdateMetadata(typeof(IntBox), 1, int.MinValue, int.MaxValue, 0);
        }

        public IntBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y,
            int.MinValue,
            int.MaxValue)
        {
        }

        protected override void ValidateText(string txt)
        {
            int result;
            if (int.TryParse(txt, out result))
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
            return @"^[-]?[\d]*$";
        }

        protected override bool ValidValue(string text)
        {
            return this.ValidateValue(Convert.ToInt32(text))
                .ToString() == text;
        }

        protected override void ConvertValueToText()
        {
            if (this.TextBox != null)
            {
                this.TextBox.Text = String.Format("{0} {1}", this.Value.ToString(), this.Suffix);
            }
        }
    }
}