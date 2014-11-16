namespace Gu.Wpf.NumericControls.Tests
{
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class DecimalBoxTests : NumericBoxTests<decimal>
    {
        public DecimalBoxTests()
            : base(() => new DecimalBox(), -10, 10, 1)
        {
        }
    }
}