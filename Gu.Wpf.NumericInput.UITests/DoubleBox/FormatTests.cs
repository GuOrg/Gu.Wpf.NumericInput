namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Globalization;
    using NUnit.Framework;

    public class FormatTests : DoubleBoxTestsBase
    {
        private static readonly CultureInfo EnUs = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo SvSe = CultureInfo.GetCultureInfo("sv-SE");

        private static readonly IReadOnlyList<TestCase> TestCases = new[]
            {
                new TestCase("1", "F1", EnUs, "1.0", "1"),
                new TestCase("1", "F1", SvSe, "1,0", "1"),
                new TestCase("1.23456", "F3", EnUs, "1.235", "1.23456"),
                new TestCase("1.23456", "F4", EnUs, "1.2346", "1.23456"),
                new TestCase("1", "0.#", EnUs, "1", "1"),
                new TestCase("1.23456", "0.###", EnUs, "1.235", "1.23456"),
            };

        [SetUp]
        public void SetUp()
        {
            this.ViewModelValueBox.Text = "0";
            this.CultureBox.Select("en-US");
            this.StringFormatBox.Text = string.Empty;
            this.CanValueBeNullBox.Checked = false;

            this.AllowLeadingWhiteBox.Checked = true;
            this.AllowTrailingWhiteBox.Checked = true;
            this.AllowLeadingSignBox.Checked = true;
            this.AllowDecimalPointBox.Checked = true;
            this.AllowThousandsBox.Checked = false;
            this.AllowExponentBox.Checked = true;

            this.MinBox.Text = string.Empty;
            this.MaxBox.Text = string.Empty;

            this.LoseFocusButton.Click();
        }

        [TestCaseSource(nameof(TestCases))]
        public void WithStringFormat(TestCase testCase)
        {
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            this.StringFormatBox.Text = testCase.StringFormat;
            this.CultureBox.Select(testCase.Culture.Name);

            doubleBox.Text = testCase.Text;
            this.LoseFocusButton.Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(testCase.Text, doubleBox.Text);
            Assert.AreEqual(testCase.Formatted, doubleBox.FormattedView().Text);
            Assert.AreEqual(testCase.ViewModelValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [Test]
        public void WhenStringFormatChanges()
        {
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            this.StringFormatBox.Text = "F1";

            doubleBox.Text = "1.23456";
            this.LoseFocusButton.Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.StringFormatBox.Text = "F4";
            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2346", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.StringFormatBox.Text = "F1";
            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual("1.23456", doubleBox.Text);
            Assert.AreEqual("1.2", doubleBox.FormattedView().Text);
            Assert.AreEqual("1.23456", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        public class TestCase
        {
            public TestCase(string text, string stringFormat, CultureInfo culture, string formatted, string viewModelValue)
            {
                this.Text = text;
                this.StringFormat = stringFormat;
                this.Culture = culture;
                this.Formatted = formatted;
                this.ViewModelValue = viewModelValue;
            }

            public string Text { get; }

            public string StringFormat { get; }

            public CultureInfo Culture { get; }

            public string Formatted { get; }

            public string ViewModelValue { get; }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Formatted}";
        }
    }
}