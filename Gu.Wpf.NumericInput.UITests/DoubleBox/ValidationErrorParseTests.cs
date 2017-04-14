namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    public class ValidationErrorParseTests : DoubleBoxTestsBase
    {
        private static readonly TestCase[] TestCases =
            {
                new TestCase("abc", "0", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
                new TestCase("2,1", "2", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
            };

        private static readonly TestCase[] SwedishCases =
            {
                new TestCase("abc", "0", "ValidationError.CanParseValidationResult 'Vänligen ange en giltig siffra.'"),
                new TestCase("2.1", "2", "ValidationError.CanParseValidationResult 'Vänligen ange en giltig siffra.'"),
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
            this.LoseFocusButton.Click();
            this.Window.WaitWhileBusy();
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase data)
        {
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = "0";
            this.LoseFocusButton.Click(); // needed to reset explicitly here for some reason

            doubleBox.Text = data.Text;
            this.Window.WaitWhileBusy();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase data)
        {
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChanged(TestCase data)
        {
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            this.Window.WaitWhileBusy();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void PropertyChangedSwedish(TestCase data)
        {
            this.CultureBox.Select("sv-SE");
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedWhenNotLocalized(TestCase data)
        {
            this.CultureBox.Select("ja-JP");
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            this.Window.WaitWhileBusy();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputInvalid(string text, string infoMessage)
        {
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = text;
            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());

            this.AllowDecimalPointBox.Checked = false;
            if (infoMessage != null)
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
            }
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputValid(string text, string infoMessage)
        {
            this.AllowDecimalPointBox.Checked = false;
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = text;
            this.LoseFocusButton.Click();
            if (infoMessage != null)
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
            }

            this.AllowDecimalPointBox.Checked = true;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            //// Assert.AreEqual(text, this.ViewModelValueBox.Text);
            //// not sure about what to do here.
            //// calling UpdateSource() is easy enough but dunno what
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        public class TestCase
        {
            public TestCase(string text, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string Text { get; }

            public string Expected { get; }

            public string ExpectedInfoMessage { get; }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}