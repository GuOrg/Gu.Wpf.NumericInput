namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class DoubleBoxTests : FloatBaseTests<DoubleBox, double>
    {
        protected override Func<DoubleBox> Creator
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