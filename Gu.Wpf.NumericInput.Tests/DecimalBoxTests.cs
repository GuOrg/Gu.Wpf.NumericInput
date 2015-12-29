namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class DecimalBoxTests : FloatBaseTests<DecimalBox, decimal>
    {
        protected override decimal Max => 10;

        protected override decimal Min => -10;

        protected override decimal Increment => 1;

        protected override Func<DecimalBox> Creator
        {
            get { return () => new DecimalBox(); }
        }
    }
}