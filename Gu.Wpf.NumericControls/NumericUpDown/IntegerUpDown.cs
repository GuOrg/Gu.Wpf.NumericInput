namespace Gu.Wpf.NumericControls
{
    using System;
    using System.ComponentModel;
    using System.Windows;

    [ToolboxItem(true)]
    public class IntegerUpDown : CommonUpDown<int>
    {
        static IntegerUpDown()
        {
            UpdateMetadata(typeof(IntegerUpDown), 1, int.MinValue, int.MaxValue, 0);
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

        protected override int IncrementValue(int value, int increment)
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(ValueChangedEvent);
            newEventArgs.RoutedEvent = ValueChangedEvent;
            this.RaiseEvent(newEventArgs);

            return value + increment;
        }

        protected override int DecrementValue(int value, int increment)
        {
            return value - increment;
        }

        protected override void ConvertValueToText()
        {
            if (this.TextBox != null)
            {
                this.TextBox.Text = String.Format("{0} {1}",this.Value.ToString(), this.Suffix);
            }
        }
    }
}