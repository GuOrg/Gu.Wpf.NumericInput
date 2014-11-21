namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class ShortBoxTests : NumericBoxTests<short>
    {
        protected override Func<NumericBox<short>> Creator
        {
            get { return () => new ShortBox(); }
        }
        protected override short Max
        {
            get { return 10; }
        }
        protected override short Min
        {
            get { return -10; }
        }
        protected override short Increment
        {
            get { return 1; }
        }

        [Test]
        public void IncreaseOverflow()
        {
            var box = new ShortBox
            {
                MaxValue = short.MaxValue,
                Value = short.MaxValue - 1,
                Increment = 10,
            };
            box.IncreaseCommand.Execute(null);
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
            box.DecreaseCommand.Execute(null);
            Assert.AreEqual(short.MinValue, box.Value);
        }
    }
}
