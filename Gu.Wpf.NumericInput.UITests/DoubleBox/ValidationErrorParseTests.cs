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
        }

        [TestCaseSource(nameof(ParseSource))]
        public void LostFocus(ParseData data)
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