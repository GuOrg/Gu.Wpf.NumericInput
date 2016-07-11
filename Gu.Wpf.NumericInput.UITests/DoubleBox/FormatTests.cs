namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Globalization;
    using NUnit.Framework;

    public class FormatTests : DoubleBoxTestsBase
    {
        private static readonly CultureInfo EnUs = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo SvSe = CultureInfo.GetCultureInfo("sv-SE");

        public static readonly FormatData[] Source =
            {
                new FormatData("1", "F1", EnUs, "1.0","1"),
                new FormatData("1", "F1", SvSe, "1,0","1"),
                new FormatData("1.23456", "F3", EnUs, "1.235","1.23456"),
                new FormatData("1.23456", "F4", EnUs, "1.2346","1.23456"),
                new FormatData("1", "0.#", EnUs, "1","1"),
                new FormatData("1.23456", "0.###", EnUs, "1.235","1.23456"),
            };

        [SetUp]
        public void SetUp()
        {
            this.ViewModelValueBox.Text = "0";
            this.CultureBox.Select("en-US");
            this.StringFormatBox.Text = "";
            this.CanValueBeNullBox.Checked = false;

            this.AllowLeadingWhiteBox.Checked = true;
            this.AllowTrailingWhiteBox.Checked = true;
            this.AllowLeadingSignBox.Checked = true;
            this.AllowDecimalPointBox.Checked = true;
            this.AllowThousandsBox.Checked = false;
            this.AllowExponentBox.Checked = true;

            this.MinBox.Text = "";
            this.MaxBox.Text = "";

            this.LoseFocusButton.Click();
        }

        [TestCaseSource(nameof(Source))]
        public void WithStringFormat(FormatData formatData)
        {
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            this.StringFormatBox.Text = formatData.StringFormat;
            this.CultureBox.Select(formatData.Culture.Name);

            doubleBox.Text = formatData.Text;
            this.LoseFocusButton.Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(formatData.Text, doubleBox.Text);
            Assert.AreEqual(formatData.Formatted, doubleBox.FormattedView().Text);
            Assert.AreEqual(formatData.ViewModelValue, this.ViewModelValueBox.Text);
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

        public class FormatData
        {
            public readonly string Text;
            public readonly string StringFormat;
            public readonly CultureInfo Culture;
            public readonly string Formatted;
            public readonly string ViewModelValue;

            public FormatData(string text, string stringFormat, CultureInfo culture, string formatted, string viewModelValue)
            {
                this.Text = text;
                this.StringFormat = stringFormat;
                this.Culture = culture;
                this.Formatted = formatted;
                this.ViewModelValue = viewModelValue;
            }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Formatted}";
        }
    }
}