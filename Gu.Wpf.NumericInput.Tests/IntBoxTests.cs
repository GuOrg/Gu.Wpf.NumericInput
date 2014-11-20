namespace Gu.Wpf.NumericInput.Tests
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