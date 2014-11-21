namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class LongBoxTests : NumericBoxTests<long>
    {
        protected override Func<NumericBox<long>> Creator
        {
            get { return () => new LongBox(); }
        }
        protected override long Max
        {
            get { return 10; }
        }
        protected override long Min
        {
            get { return -10; }
        }
        protected override long Increment
        {
            get { return 1; }
        }
    }
}