namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;
    using NUnit.Framework;
    using TestStack.White.UIItems;

    public class ValidationErrorRegexTests : ValidationTestsBase
    {
        public static readonly RegexData[] RegexSource =
            {
                new RegexData("1.2", @"^\d\.\d$", "1.2", null),
                new RegexData("12.34", @"^\d\.\d$", "0", "ValidationError.RegexValidationResult 'Please provide valid input.'"),
            };

        public static readonly RegexData[] SwedishRegexSource =
            {
                new RegexData("1,2",  @"^\d,\d$", "1.2", null),
                new RegexData("12,34",  @"^\d,\d$", "0", "ValidationError.RegexValidationResult 'Vänligen ange ett giltigt värde.'"),
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
            this.RegexPatternBox.Text = "";
            this.LoseFocusButton.Click();
        }

        [TestCaseSource(nameof(RegexSource))]
        public void LostFocusValidateOnLostFocus(RegexData data)
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

        [TestCaseSource(nameof(RegexSource))]
        public void LostFocusValidateOnLostFocusWhenPatternChanges(RegexData data)
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

        [TestCaseSource(nameof(RegexSource))]
        public void LostFocusValidateOnPropertyChanged(RegexData data)
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

        [TestCaseSource(nameof(RegexSource))]
        public void PropertyChanged(RegexData data)
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

        [TestCaseSource(nameof(SwedishRegexSource))]
        public void PropertyChangedSwedish(RegexData data)
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

        [TestCaseSource(nameof(RegexSource))]
        public void PropertyChangedWhenNotLocalized(RegexData data)
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

        public class RegexData
        {
            public readonly string Text;
            public readonly string Pattern;
            public readonly string Expected;
            public readonly string ExpectedInfoMessage;

            public RegexData(string text, string pattern, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.Pattern = pattern;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, Pattern: {this.Pattern}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}