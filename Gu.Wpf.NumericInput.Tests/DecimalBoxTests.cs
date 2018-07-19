namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture]
    [Apartment(ApartmentState.STA)]
    public class DecimalBoxTests : FloatBaseTests<DecimalBox, decimal>
    {
        protected override decimal Max => 10;

        protected override decimal Min => -10;

        protected override decimal Increment => 1;

        protected override Func<DecimalBox> Creator => () => new DecimalBox();
    }
}
