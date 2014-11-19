namespace Gu.Wpf.NumericControls.Tests
{
    using System.Windows;
    using System.Windows.Controls;
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