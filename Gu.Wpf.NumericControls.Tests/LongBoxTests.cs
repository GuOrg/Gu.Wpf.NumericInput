namespace Gu.Wpf.NumericControls.Tests
{
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class LongBoxTests : NumericBoxTests<long>
    {
        public LongBoxTests()
            : base(() => new LongBox(), -10, 10, 1)
        {
        }
    }
}