namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class DoubleBoxTests : FloatBaseTests<double>
    {
        protected override Func<NumericBox<double>> Creator
        {
            get { return () => new DoubleBox(); }
        }
        protected override double Max
        {
            get { return 10; }
        }
        protected override double Min
        {
            get { return -10; }
        }
        protected override double Increment
        {
            get { return 1; }
        }

        [Test]
        public void AddedDigitsNotTruncated()
        {
            Sut.Decimals = 2;
            Sut.Text = "1.23";
            Sut.Text = "1.234";
            Assert.AreEqual(1.234, Sut.Value);
        }

        [Test]
        public void FewerDecimalsUpdatesValue()
        {
            Sut.Decimals = 4;
            Sut.Text = "1.2334";
            Sut.Text = "1.23";
            Assert.AreEqual(1.23, Sut.Value);
        }
    }
}