namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class ShortBox : NumericBox<short>
    {
        static ShortBox()
        {
            UpdateMetadata(typeof(ShortBox), 1, short.MinValue, short.MaxValue, 0);
        }

        public ShortBox()
            : base(
            (x, y) => (short)(x + y), 
            (x, y) => (short)(x - y), 
            short.MinValue, 
            short.MaxValue)
        {
        }
        protected override void ValidateText(string txt)
        {
            short result;
            if (short.TryParse(txt, out result))
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
            short value;
            bool ok = short.TryParse(text, out value);
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
            this.TextBox.Text = String.Format("{0} {1}", this.Value.ToString(CultureInfo.InvariantCulture), this.Suffix);
        }
    }
}