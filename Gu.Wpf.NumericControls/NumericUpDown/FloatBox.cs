namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class FloatBox : NumericBox<float>
    {
        static FloatBox()
        {
            UpdateMetadata(typeof(FloatBox), 1f, float.MinValue, float.MaxValue, 2);
        }

        public FloatBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y,
            float.MinValue,
            float.MaxValue)
        {
        }

        protected override void ValidateText(string txt)
        {
            float result;
            if (float.TryParse(txt, out result))
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
            return @"^[-]?[\d]*[,.]?[\d]{0," + this.Decimals.ToString(CultureInfo.InvariantCulture) + "}?$";
        }

        protected override bool ValidValue(string text)
        {
            float value;
            bool ok = float.TryParse(text, out value);
            if (!ok)
            {
                return false;
            }
            else
            {
                return this.ValidateValue(value)
                    .ToString() == text;
            }
        }

        protected override void ConvertValueToText()
        {
            if (this.TextBox != null)
            {
                this.TextBox.Text = string.Format("{0} {1}", this.Value.ToString("F" + this.Decimals), this.Suffix);
            }
        }
    }
}