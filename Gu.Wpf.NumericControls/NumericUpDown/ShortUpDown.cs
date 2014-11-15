namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Globalization;

    [ToolboxItem(true)]
    public class ShortUpDown : CommonUpDown<short>
    {
        static ShortUpDown()
        {
            UpdateMetadata(typeof(ShortUpDown), (short)1, short.MinValue, short.MaxValue, 0);
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
        protected override short IncrementValue(short value, short increment)
        {
            return (short)(value + increment);
        }
        protected override short DecrementValue(short value, short increment)
        {
            return (short)(value - increment);
        }

        protected override void ConvertValueToText()
        {
            this.TextBox.Text = String.Format("{0} {1}",this.Value.ToString(CultureInfo.InvariantCulture), this.Suffix);
        }
    }
}