namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class IntBoxTests : NumericBoxTests<IntBox, int>
    {
        protected override Func<IntBox> Creator
        {
            get { return () => new IntBox(); }
        }
        protected override int Max
        {
            get { return 10; }
        }
        protected override int Min
        {
            get { return -10; }
        }
        protected override int Increment
        {
            get { return 1; }
        }
    }
}