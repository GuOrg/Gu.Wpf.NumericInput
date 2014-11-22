namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class DecimalBoxTests : FloatBaseTests<DecimalBox, decimal>
    {
        protected override Func<DecimalBox> Creator
        {
            get { return () => new DecimalBox(); }
        }
        protected override decimal Max
        {
            get { return 10; }
        }
        protected override decimal Min
        {
            get { return -10; }
        }
        protected override decimal Increment
        {
            get { return 1; }
        }
    }
}