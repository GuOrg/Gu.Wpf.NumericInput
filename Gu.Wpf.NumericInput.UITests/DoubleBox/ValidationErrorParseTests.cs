namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using TestStack.White.UIItems;

    public class ValidationErrorParseTests : ValidationTestsBase
    {
        public static readonly ParseData[] ParseSource =
            {
                new ParseData("abc", "0", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
                new ParseData("2,1", "2", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
            };

        public static readonly ParseData[] SwedishParseSource =
            {
                new ParseData("abc", "0", "ValidationError.CanParseValidationResult 'Vänligen ange en giltig siffra.'"),
                new ParseData("2.1", "2", "ValidationError.CanParseValidationResult 'Vänligen ange en giltig siffra.'"),
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

            this.MinBox.Text = "";
            this.MaxBox.Text = "";
            this.LoseFocusButton.Click();
            this.Window.WaitWhileBusy();
        }

        [TestCase(nameof(ParseSource))]
        public void LostFocusValidateOnLostFocus(ParseData data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnLostFocusBox");
            doubleBox.BulkText = "0";
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
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(ParseSource))]
        public void LostFocusValidateOnPropertyChanged(ParseData data)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");

            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, this.Window.Get<Label>("LostFocusValidateOnPropertyChangedBoxError").Text);
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

        [TestCaseSource(nameof(ParseSource))]
        public void PropertyChanged(ParseData data)
        {
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = data.Text;
            this.Window.WaitWhileBusy();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, this.Window.Get<Label>("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishParseSource))]
        public void PropertyChangedSwedish(ParseData data)
        {
            this.CultureBox.Select("sv-SE");
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, this.Window.Get<Label>("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(ParseSource))]
        public void PropertyChangedWhenNotLocalized(ParseData data)
        {
            this.CultureBox.Select("ja-JP");
            var doubleBox = this.Window.Get<TextBox>("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = data.Text;
            this.Window.WaitWhileBusy();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, this.Window.Get<Label>("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Expected, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputInvalid(string text, string infoMessage)
        {
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");

            doubleBox.Text = text;
            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());

            this.AllowDecimalPointBox.Checked = false;
            if (infoMessage != null)
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(ParseData.GetErrorMessage(infoMessage), this.Window.Get<Label>("LostFocusValidateOnPropertyChangedBoxError").Text);
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
            var doubleBox = this.Window.Get<TextBox>("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            this.LoseFocusButton.Click();
            if (infoMessage != null)
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(ParseData.GetErrorMessage(infoMessage), this.Window.Get<Label>("LostFocusValidateOnPropertyChangedBoxError").Text);
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
            }

            this.AllowDecimalPointBox.Checked = true;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            // Assert.AreEqual(text, this.ViewModelValueBox.Text); 
            // not sure about what to do here.
            // calling UpdateSource() is easy enough but dunno what 
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        public class ParseData
        {
            public readonly string Text;
            public readonly string Expected;
            public readonly string ExpectedInfoMessage;

            public ParseData(string text, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}