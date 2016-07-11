namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;
    using NUnit.Framework;

    public class ValidationErrorRequiredTests : DoubleBoxTestsBase
    {
        public static readonly RequiredData[] RequiredSource =
            {
                new RequiredData("1.2", true, "1.2", null),
                new RequiredData("1.2", false, "1.2", null),
                new RequiredData("", false, "0", "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'"),
                new RequiredData("", true, "", null),
            };

        public static readonly RequiredData[] SwedishRequiredSource =
            {
                new RequiredData("1,2", true, "1.2", null),
                new RequiredData("1,2", false, "1.2", null),
                new RequiredData("", false, "0", "ValidationError.RequiredButMissingValidationResult 'Vänligen ange en siffra.'"),
                new RequiredData("", true, "", null),
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

        [TestCaseSource(nameof(RequiredSource))]
        public void LostFocusValidateOnLostFocus(RequiredData data)
        {
            this.CanValueBeNullBox.Checked = data.CanValueBeNull;

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

        [TestCase("1.2", null)]
        [TestCase("", "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'")]
        public void LostFocusValidateOnLostFocusWhenIsRequiredChangesMakingInputInvalid(string text, string infoMessage)
        {
            this.CanValueBeNullBox.Checked = true;
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = text;
            this.LoseFocusButton.Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(text, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.CanValueBeNullBox.Checked = false;
            this.LoseFocusButton.Click();
            if (infoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(RequiredData.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCase("1.2", null)]
        [TestCase("", "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'")]
        public void LostFocusValidateOnLostFocusWhenIsRequiredChangesMakingInputValid(string text, string infoMessage)
        {
            this.CanValueBeNullBox.Checked = false;
            var boxes = this.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = text;
            this.LoseFocusButton.Click();
            if (infoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(RequiredData.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual("0", this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                this.Window.WaitWhileBusy();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, this.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }

            this.CanValueBeNullBox.Checked = true;
            this.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            //Assert.AreEqual(text, this.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(RequiredSource))]
        public void LostFocusValidateOnPropertyChanged(RequiredData data)
        {
            this.CanValueBeNullBox.Checked = data.CanValueBeNull;
            var boxes = this.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;

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

        [TestCaseSource(nameof(RequiredSource))]
        public void PropertyChanged(RequiredData data)
        {
            this.CanValueBeNullBox.Checked = data.CanValueBeNull;

            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
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

        [TestCaseSource(nameof(SwedishRequiredSource))]
        public void PropertyChangedSwedish(RequiredData data)
        {
            this.CultureBox.Select("sv-SE");

            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            this.CanValueBeNullBox.Checked = data.CanValueBeNull;

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

        [TestCaseSource(nameof(RequiredSource))]
        public void PropertyChangedWhenNotLocalized(RequiredData data)
        {
            this.CultureBox.Select("ja-JP");

            var boxes = this.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            this.CanValueBeNullBox.Checked = data.CanValueBeNull;
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

        public class RequiredData
        {
            public readonly string Text;
            public readonly bool CanValueBeNull;
            public readonly string Expected;
            public readonly string ExpectedInfoMessage;

            public RequiredData(string text, bool canValueBeNull, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.CanValueBeNull = canValueBeNull;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, CanValueBeNull: {this.CanValueBeNull}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}