namespace Gu.Wpf.NumericControls.Tests
{
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class DoubleBoxTests : NumericBoxTests<double>
    {
        public DoubleBoxTests()
            : base(() => new DoubleBox(), -10, 10, 1)
        {
        }
    }
}