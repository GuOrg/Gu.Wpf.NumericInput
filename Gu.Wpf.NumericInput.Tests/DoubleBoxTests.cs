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
    }
}