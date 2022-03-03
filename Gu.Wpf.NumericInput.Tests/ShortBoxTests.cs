namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class ShortBoxTests : NumericBoxTests<ShortBox, short>
    {
        protected override Func<ShortBox> Creator => () => new ShortBox();

        protected override short Max => 10;

        protected override short Min => -10;

        protected override short Increment => 1;

        [Test]
        public void IncreaseOverflow()
        {
            var box = new ShortBox
            {
                MaxValue = short.MaxValue,
                Value = short.MaxValue - 1,
                Increment = 10,
            };
            box.IncreaseCommand!.Execute(null);
            Assert.AreEqual(short.MaxValue, box.Value);
        }

        [Test]
        public void DecreaseOverflow()
        {
            var box = new ShortBox
            {
                MinValue = short.MinValue,
                Value = short.MinValue + 1,
                Increment = 10,
            };
            box.DecreaseCommand!.Execute(null);
            Assert.AreEqual(short.MinValue, box.Value);
        }
    }
}
