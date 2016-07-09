namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using NUnit.Framework;

    using TestStack.White.UIItems;

    public class ValidationHappyPathTests : ValidationTestsBase
    {
        public static readonly HappyPathData[] HappyPathSource = {
                                                                     new HappyPathData("1", "1"),
                                                                     new HappyPathData("1.2", "1.2"),
                                                                     new HappyPathData(".1", "0.1"),
                                                                     new HappyPathData("0.1", "0.1"),
                                                                     new HappyPathData("1e1", "10"),
                                                                     new HappyPathData("1e0", "1"),
                                                                     new HappyPathData("1e-1", "0.1"),
                                                                     new HappyPathData("1E1", "10"),
                                                                     new HappyPathData("1E0", "1"),
                                                                     new HappyPathData("1E-1", "0.1"),
                                                                 };


        [SetUp]
        public void SetUp()
        {
            this.ViewModelValueBox.Text = "0";
            this.CultureBox.Select("en-US");
            this.AllowDecimalPointBox.Checked = true;
            this.LoseFocusButton.Click();
        }

        [TestCaseSource(nameof(HappyPathSource))]
        public void HappyPathLostFocus(HappyPathData data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);

            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(HappyPathSource))]
        public void HappyPathPropertyChanged(HappyPathData data)
        {
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
        }

        public class HappyPathData
        {
            public readonly string Text;
            public readonly string Expected;

            public HappyPathData(string text, string expected)
            {
                this.Text = text;
                this.Expected = expected;
            }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Expected}";
        }
    }
}