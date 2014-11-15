namespace Gu.Wpf.NumericControls.Tests
{
    using System;

    public class BaseUpDownImpl : BaseUpDown
    {
        protected override void Increase()
        {
        }

        protected override void Decrease()
        {
        }

        protected override void ValidateText(string text)
        {
            throw new NotImplementedException();
        }

        protected override string ValidInput()
        {
            throw new NotImplementedException();
        }

        protected override bool ValidValue(string text)
        {
            throw new NotImplementedException();
        }

        protected override void ConvertValueToText()
        {
            throw new NotImplementedException();
        }

        protected override void CheckSpinners()
        {
            base.CheckSpinners();
        }
    }
}