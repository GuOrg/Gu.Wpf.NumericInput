namespace Gu.Wpf.NumericInput.Tests
{
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class ShortBoxTests : NumericBoxTests<short>
    {
        public ShortBoxTests()
            : base(() => new ShortBox(), -10, 10, 1)
        {
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
