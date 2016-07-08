namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Threading;

    using NUnit.Framework;

    [TestFixture, Apartment(ApartmentState.STA)]
    public class DoubleBoxTests : FloatBaseTests<DoubleBox, double>
    {
        protected override double Max => 10;

        protected override double Min => -10;

        protected override double Increment => 1;

        protected override Func<DoubleBox> Creator
        {
            get { return () => new DoubleBox(); }
        }
    }
}