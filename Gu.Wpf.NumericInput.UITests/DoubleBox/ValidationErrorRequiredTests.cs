namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using System.Text.RegularExpressions;
    using FlaUI.Core.Definitions;
    using NUnit.Framework;

    public sealed class ValidationErrorRequiredTests : IDisposable
    {
        private static readonly TestCase[] TestCases =
        {
            new TestCase(text: "1.2", canValueBeNull: true, expected: "1.2", expectedInfoMessage: null),
            new TestCase(text: "1.2", canValueBeNull: false, expected: "1.2", expectedInfoMessage: null),
            new TestCase(text: string.Empty, canValueBeNull: false, expected: "0", expectedInfoMessage: "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'"),
            new TestCase(text: string.Empty, canValueBeNull: true, expected: string.Empty, expectedInfoMessage: null),
        };

        private static readonly TestCase[] SwedishCases =
        {
            new TestCase(text: "1,2", canValueBeNull: true, expected: "1.2", expectedInfoMessage: null),
            new TestCase(text: "1,2", canValueBeNull: false, expected: "1.2", expectedInfoMessage: null),
            new TestCase(text: string.Empty, canValueBeNull: false, expected: "0", expectedInfoMessage: "ValidationError.RequiredButMissingValidationResult 'Vänligen ange en siffra.'"),
            new TestCase(text: string.Empty, canValueBeNull: true, expected: string.Empty, expectedInfoMessage: null),
        };

        private readonly DoubleBoxValidationView view;
        private bool disposed;

        public ValidationErrorRequiredTests()
        {
            this.view = new DoubleBoxValidationView();
        }

        [SetUp]
        public void SetUp()
        {
            this.view.Reset();
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase data)
        {
            this.view.CanValueBeNullBox.State = data.CanValueBeNull ? ToggleState.On : ToggleState.Off;

            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.view.LoseFocusButton.Click();
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCase("1.2", null)]
        [TestCase("", "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'")]
        public void LostFocusValidateOnLostFocusWhenIsRequiredChangesMakingInputInvalid(string text, string infoMessage)
        {
            this.view.CanValueBeNullBox.State = ToggleState.On;
            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = text;
            this.view.LoseFocusButton.Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(text, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            this.view.CanValueBeNullBox.State = ToggleState.Off;
            this.view.LoseFocusButton.Click();
            if (infoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCase("1.2", null)]
        [TestCase("", "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'")]
        public void LostFocusValidateOnLostFocusWhenIsRequiredChangesMakingInputValid(string text, string infoMessage)
        {
            this.view.CanValueBeNullBox.State = ToggleState.Off;
            var boxes = this.view.LostFocusValidateOnLostFocusBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = text;
            this.view.LoseFocusButton.Click();
            if (infoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), boxes.ErrorBlock.Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }

            this.view.CanValueBeNullBox.State = ToggleState.On;
            this.view.LoseFocusButton.Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            ////Assert.AreEqual(text, this.view.ViewModelValueBox.Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase data)
        {
            this.view.CanValueBeNullBox.State = data.CanValueBeNull ? ToggleState.On : ToggleState.Off;
            var boxes = this.view.LostFocusValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;

            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                this.view.LoseFocusButton.Click();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                this.view.LoseFocusButton.Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChanged(TestCase data)
        {
            this.view.CanValueBeNullBox.State = data.CanValueBeNull ? ToggleState.On : ToggleState.Off;

            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void PropertyChangedSwedish(TestCase data)
        {
            this.view.CultureBox.Select("sv-SE");

            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            this.view.CanValueBeNullBox.State = data.CanValueBeNull ? ToggleState.On : ToggleState.Off;

            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedWhenNotLocalized(TestCase data)
        {
            this.view.CultureBox.Select("ja-JP");

            var boxes = this.view.PropertyChangedValidateOnPropertyChangedBoxes;
            var doubleBox = boxes.DoubleBox;
            this.view.CanValueBeNullBox.State = data.CanValueBeNull ? ToggleState.On : ToggleState.Off;
            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, boxes.ErrorBlock.Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, this.view.ViewModelValueBox.Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
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
            public TestCase(string text, bool canValueBeNull, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.CanValueBeNull = canValueBeNull;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string Text { get; }

            public bool CanValueBeNull { get; }

            public string Expected { get; }

            public string ExpectedInfoMessage { get; }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, CanValueBeNull: {this.CanValueBeNull}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}