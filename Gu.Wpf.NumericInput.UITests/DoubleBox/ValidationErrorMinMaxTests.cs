namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;

    using NUnit.Framework;

    public class ValidationErrorMinMaxTests : DoubleBoxTestsBase
    {
        private static readonly TestCase[] TestCases =
        {
            new TestCase("-2", "-1", string.Empty, "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.'"),
            new TestCase("-2.1", "-1.1", string.Empty, "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.1.'"),
            new TestCase("-2", "-1", "1", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value between -1 and 1.'"),
            new TestCase("-2.1", "-1.1", "1.1", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value between -1.1 and 1.1.'"),
            new TestCase("2", string.Empty, "1", "1", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.'"),
            new TestCase("2.1", string.Empty, "1.1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.1.'"),
            new TestCase("2", "-1", "1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value between -1 and 1.'"),
            new TestCase("2.1", "-1.1", "1.1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value between -1.1 and 1.1.'"),
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
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase data)
        {
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.StartValue;
            this.LoseFocusButton.Click();
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;

            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase data)
        {
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.StartValue;
            this.LoseFocusButton.Click();
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedValidateOnPropertyChanged(TestCase data)
        {
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.StartValue;
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCase("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Vänligen ange ett värde mindre än eller lika med 2,2.'")]
        [TestCase("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Vänligen ange ett värde större än eller lika med -2,1.'")]
        public void PropertyChangedSwedish(string value, string min, string max, string infoMessage)
        {
            this.CultureBox.Select("sv-SE");
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = "1";
            this.MinBox.Text = min;
            this.MaxBox.Text = max;
            doubleBox.Text = value;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual("1", this.ViewModelValueBox.Text);
        }

        [TestCase("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [TestCase("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void PropertyChangedWhenNotLocalized(string value, string min, string max, string infoMessage)
        {
            this.CultureBox.Select("ja-JP");
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = "1";
            this.MinBox.Text = min;
            this.MaxBox.Text = max;
            doubleBox.Text = value;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual("1", this.ViewModelValueBox.Text);
        }

        [TestCase("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [TestCase("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void LostFocusValidateOnLostFocusWhenMinAndMaxChanges(string value, string min, string max, string infoMessage)
        {
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = value;
            Assert.AreEqual(false, doubleBox.HasValidationError());

            this.MinBox.Text = min;
            this.MaxBox.Text = max;
            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual(value, this.ViewModelValueBox.Text);
        }

        [TestCase("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [TestCase("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void PropertyChangedWhenMinAndMaxChanges(string value, string min, string max, string infoMessage)
        {
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = value;
            Assert.AreEqual(false, doubleBox.HasValidationError());

            this.MinBox.Text = min;
            this.MaxBox.Text = max;
            this.LoseFocusButton.Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual(value, this.ViewModelValueBox.Text);
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedWhenNull(TestCase data)
        {
            this.CanValueBeNullBox.Checked = true;
            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = string.Empty;
            this.MinBox.Text = data.Min;
            this.MaxBox.Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(string.Empty, this.ViewModelValueBox.Text);
        }

        public class TestCase
        {
            public TestCase(string text, string min, string max, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.Min = min;
                this.Max = max;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string Text { get; }

            public string Min { get; }

            public string Max { get; }

            public string Expected { get; }

            public string ExpectedInfoMessage { get; }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public string StartValue => string.IsNullOrEmpty(this.Min)
                                            ? this.Max
                                            : this.Min;

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, Min: {this.Min}, Max: {this.Max}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}