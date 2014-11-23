namespace Gu.Wpf.NumericInput.Tests
{
    using Gu.Wpf.NumericInput.Validation;

    using NUnit.Framework;
    [RequiresSTA]
    public class StringHelperTests
    {
        private DoubleBox _box;

        [SetUp]
        public void SetUp()
        {
            _box = new DoubleBox();
        }

        [TestCase("1.2", "1.23", false)]
        [TestCase("1.2", "1.2", false)]
        [TestCase("1.23", "1.2", true)]
        [TestCase("1.26", "1.2", false)]
        [TestCase("2.23", "1.2", false)]
        public void HasMoreDecimalDigitsThan(string first, string other, bool expected)
        {
            var actual = first.HasMoreDecimalDigitsThan(other, _box);
            Assert.AreEqual(expected, actual);
        }
    }
}
