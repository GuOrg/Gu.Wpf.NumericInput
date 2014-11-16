namespace Gu.Wpf.NumericControls.Tests
{
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class IntBoxTests : NumericBoxTests<int>
    {
        public IntBoxTests()
            : base(() => new IntBox(), -10, 10, 1)
        {
        }
    }
}