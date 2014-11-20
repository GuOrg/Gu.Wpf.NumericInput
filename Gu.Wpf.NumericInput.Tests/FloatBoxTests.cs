namespace Gu.Wpf.NumericInput.Tests
{
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class FloatBoxTests : NumericBoxTests<float>
    {
        public FloatBoxTests()
            : base(() => new FloatBox(), -10, 10, 1)
        {
        }
    }
}