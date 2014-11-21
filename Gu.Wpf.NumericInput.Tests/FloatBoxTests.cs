namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class FloatBoxTests : FloatBaseTests<float>
    {
        protected override Func<NumericBox<float>> Creator
        {
            get { return () => new FloatBox(); }
        }
        protected override float Max
        {
            get { return 10; }
        }
        protected override float Min
        {
            get { return -10; }
        }
        protected override float Increment
        {
            get { return 1; }
        }
    }
}