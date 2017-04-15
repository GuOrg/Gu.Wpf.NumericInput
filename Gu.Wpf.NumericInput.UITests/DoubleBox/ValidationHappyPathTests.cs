// ReSharper disable MemberCanBePrivate.Global
namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using NUnit.Framework;

    public sealed class ValidationHappyPathTests : IDisposable
    {
        private static readonly TestCase[] TestCases =
        {
            new TestCase("1", "1"),
            new TestCase(" 1", "1"),
            new TestCase("1 ", "1"),
            new TestCase(" 1 ", "1"),
            new TestCase("1.2", "1.2"),
            new TestCase("-1.2", "-1.2"),
            new TestCase("+1.2", "1.2"),
            new TestCase(".1", "0.1"),
            new TestCase("-.1", "-0.1"),
            new TestCase("0.1", "0.1"),
            new TestCase("1e1", "10"),
            new TestCase("1e0", "1"),
            new TestCase("1e-1", "0.1"),
            new TestCase("1E1", "10"),
            new TestCase("1E0", "1"),
            new TestCase("1E-1", "0.1"),
            new TestCase("-1e1", "-10"),
            new TestCase("-1e0", "-1"),
            new TestCase("-1e-1", "-0.1"),
            new TestCase("-1E1", "-10"),
            new TestCase("-1E0", "-1"),
            new TestCase("-1E-1", "-0.1"),
        };

        private static readonly TestCase[] SwedishCases =
            {
                new TestCase("1", "1"),
                new TestCase(" 1", "1"),
                new TestCase("1 ", "1"),
                new TestCase(" 1 ", "1"),
                new TestCase("1,2", "1.2"),
                new TestCase("-1,2", "-1.2"),
                new TestCase("+1,2", "1.2"),
                new TestCase(",1", "0.1"),
                new TestCase("-,1", "-0.1"),
                new TestCase("0,1", "0.1"),
                new TestCase("1e1", "10"),
                new TestCase("1e0", "1"),
                new TestCase("1e-1", "0.1"),
                new TestCase("1E1", "10"),
                new TestCase("1E0", "1"),
                new TestCase("1E-1", "0.1"),
                new TestCase("-1e1", "-10"),
                new TestCase("-1e0", "-1"),
                new TestCase("-1e-1", "-0.1"),
                new TestCase("-1E1", "-10"),
                new TestCase("-1E0", "-1"),
                new TestCase("-1E-1", "-0.1"),
            };

        private static readonly MinMaxData[] MinMaxSource =
            {
                new MinMaxData("1", string.Empty, string.Empty, "1"),
                new MinMaxData("-1", "-1", string.Empty, "-1"),
                new MinMaxData("-1", "-10", string.Empty, "-1"),
                new MinMaxData("1", string.Empty, "1", "1"),
                new MinMaxData("1", string.Empty, "10", "1"),
                new MinMaxData("-2", "-2", "2", "-2"),
                new MinMaxData("-1", "-2", "2", "-1"),
                new MinMaxData("1", "-2", "2", "1"),
                new MinMaxData("2", "-2", "2", "2"),
            };

        private readonly DoubleBoxValidationView view;
        private bool disposed;

        public ValidationHappyPathTests()
        {
            this.view = new DoubleBoxValidationView();
        }

        [SetUp]
        public void SetUp()
        {
            this.view.Reset();
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase testCase)
        {
            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = testCase.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual(testCase.Expected, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase testCase)
        {
            var boxes = this.view.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = testCase.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual(testCase.Expected, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChanged(TestCase testCase)
        {
            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = testCase.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual(testCase.Expected, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void SwedishLostFocusValidateOnLostFocus(TestCase testCase)
        {
            this.view.CultureBox.Select("sv-SE");
            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = testCase.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual(testCase.Expected, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void SwedishLostFocusValidateOnPropertyChanged(TestCase testCase)
        {
            this.view.CultureBox.Select("sv-SE");
            var boxes = this.view.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = testCase.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual(testCase.Expected, this.view.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void SwedishPropertyChangedValidateOnPropertyChanged(TestCase testCase)
        {
            this.view.CultureBox.Select("sv-SE");
            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = testCase.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual(testCase.Expected, this.view.ViewModelValueBox.Text);
        }

        [Test]
        public void WhenNullLostFocusValidateOnLostFocus()
        {
            this.view.CanValueBeNullBox.Checked = true;
            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, this.view.ViewModelValueBox.Text);
        }

        [Test]
        public void WhenNullLostFocusValidateOnPropertyChanged()
        {
            this.view.CanValueBeNullBox.Checked = true;
            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, this.view.ViewModelValueBox.Text);
        }

        [Test]
        public void WheNullPropertyChanged()
        {
            this.view.CanValueBeNullBox.Checked = true;
            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, this.view.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxLostFocus(MinMaxData data)
        {
            this.view.MinBox.Text = data.Min;
            this.view.MaxBox.Text = data.Max;
            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxLostFocusValidateOnPropertyChanged(MinMaxData data)
        {
            this.view.MinBox.Text = data.Min;
            this.view.MaxBox.Text = data.Max;
            var boxes = this.view.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);

            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxPropertyChanged(MinMaxData data)
        {
            this.view.MinBox.Text = data.Min;
            this.view.MaxBox.Text = data.Max;
            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.view.Dispose();
        }

        public class TestCase
        {
            public TestCase(string text, string expected)
            {
                this.Text = text;
                this.Expected = expected;
            }

            public string Text { get; }

            public string Expected { get; }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Expected}";
        }

        public class MinMaxData
        {
            public MinMaxData(string text, string min, string max, string expected)
            {
                this.Text = text;
                this.Min = min;
                this.Max = max;
                this.Expected = expected;
            }

            public string Text { get; }

            public string Min { get; }

            public string Max { get; }

            public string Expected { get; }

            public override string ToString() => $"Text: {this.Text}, Min: {this.Min}, Max: {this.Max}, Expected: {this.Expected}";
        }
    }
}