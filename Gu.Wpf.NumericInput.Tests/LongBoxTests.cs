namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class LongBoxTests : NumericBoxTests<LongBox,long>
    {
        protected override Func<LongBox> Creator
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