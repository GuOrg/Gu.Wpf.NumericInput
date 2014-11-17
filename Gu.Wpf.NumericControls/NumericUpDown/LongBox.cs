namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;

    [ToolboxItem(true)]
    public class LongBox : NumericBox<long>
    {
        static LongBox()
        {
            UpdateMetadata(typeof(LongBox), 1, long.MinValue, long.MaxValue, 0);
        }

        public LongBox()
            : base(
            (x, y) => x + y,
            (x, y) => x - y)
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
            long value;
            bool ok = long.TryParse(text, out value);
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
            this.TextBox.Text = String.Format("{0} {1}", this.Value.ToString(), this.Suffix);
        }
    }
}