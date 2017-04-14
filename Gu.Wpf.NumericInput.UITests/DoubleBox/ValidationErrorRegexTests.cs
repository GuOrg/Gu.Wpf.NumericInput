namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    public class ValidationErrorRegexTests : DoubleBoxTestsBase
    {
        private static readonly TestCase[] TestCases =
        {
            new TestCase("1.2", @"^\d\.\d$", "1.2", null),
            new TestCase("12.34", @"^\d\.\d$", "0", "ValidationError.RegexValidationResult 'Please provide valid input.'"),
        };

        private static readonly TestCase[] SwedishCases =
        {
            new TestCase("1,2",  @"^\d,\d$", "1.2", null),
            new TestCase("12,34",  @"^\d,\d$", "0", "ValidationError.RegexValidationResult 'Vänligen ange ett giltigt värde.'"),
        };

        [SetUp]
        public void SetUp()
        {
            this.ViewModelValueBox.Text = "0";
            this.CultureBox.Select("en-US");
            this.CanValueBeNullBox.Checked = false;

            this.AllowLeadingWhiteBox.Checked = true;
            this.AllowTrailingWhiteBox.Checked = true;
            this.AllowLeadingSignBox.Checked = true;
            this.AllowDecimalPointBox.Checked = true;
            this.AllowThousandsBox.Checked = false;
            this.AllowExponentBox.Checked = true;

            this.MinBox.Text = string.Empty;
            this.MaxBox.Text = string.Empty;
            this.RegexPatternBox.Text = string.Empty;
            this.LoseFocusButton.Click();
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase data)
        {
            this.RegexPatternBox.Text = data.Pattern;

            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocusWhenPatternChanges(TestCase data)
        {
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Text, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.RegexPatternBox.Text = data.Pattern;
            this.LoseFocusButton.Click();
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase data)
        {
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            this.RegexPatternBox.Text = data.Pattern;

            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                this.LoseFocusButton.Click();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                this.LoseFocusButton.Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChanged(TestCase data)
        {
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            this.RegexPatternBox.Text = data.Pattern;

            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void PropertyChangedSwedish(TestCase data)
        {
            this.CultureBox.Select("sv-SE");
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            this.RegexPatternBox.Text = data.Pattern;

            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedWhenNotLocalized(TestCase data)
        {
            this.CultureBox.Select("ja-JP");

            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            this.RegexPatternBox.Text = data.Pattern;
            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        public class TestCase
        {
            public TestCase(string text, string pattern, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.Pattern = pattern;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string Text { get; }

            public string Pattern { get; }

            public string Expected { get; }

            public string ExpectedInfoMessage { get; }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public override string ToString() => $"Text: {this.Text}, Pattern: {this.Pattern}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";

            private static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }
        }
    }
}