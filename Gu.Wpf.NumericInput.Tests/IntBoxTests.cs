namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class IntBoxTests : NumericBoxTests<IntBox, int>
    {
        protected override int Max => 10;

        protected override int Min => -10;

        protected override int Increment => 1;

        protected override Func<IntBox> Creator
        {
            get { return () => new IntBox(); }
        }
    }
}