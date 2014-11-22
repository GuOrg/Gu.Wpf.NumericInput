namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class FloatBoxTests : FloatBaseTests<FloatBox, float>
    {
        protected override Func<FloatBox> Creator
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