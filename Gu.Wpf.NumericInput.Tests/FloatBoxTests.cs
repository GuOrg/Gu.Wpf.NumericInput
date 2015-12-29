namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using NUnit.Framework;

    [TestFixture, RequiresSTA]
    public class FloatBoxTests : FloatBaseTests<FloatBox, float>
    {
        protected override float Max => 10;

        protected override float Min => -10;

        protected override float Increment => 1;

        protected override Func<FloatBox> Creator
        {
            get { return () => new FloatBox(); }
        }
    }
}