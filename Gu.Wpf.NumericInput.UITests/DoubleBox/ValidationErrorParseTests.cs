namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class ValidationErrorParseTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly IReadOnlyList<TestCase> TestCases = new[]
            {
                new TestCase("abc", "0", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
                new TestCase("2,1", "2", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
            };

        private static readonly IReadOnlyList<TestCase> SwedishCases = new[]
            {
                new TestCase("abc", "0", "ValidationError.CanParseValidationResult 'Vänligen ange en giltig siffra.'"),
                new TestCase("2.1", "2", "ValidationError.CanParseValidationResult 'Vänligen ange en giltig siffra.'"),
            };

        [SetUp]
        public void SetUp()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                window.FindButton("Reset").Invoke();
                window.WaitUntilResponsive();
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase data)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
                doubleBox.Text = "0";
                window.FindButton("lose focus").Click(); // needed to reset explicitly here for some reason

                doubleBox.Text = data.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase data)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
                doubleBox.Text = data.Text;
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChanged(TestCase data)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
                doubleBox.Enter(data.Text);
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void PropertyChangedSwedish(TestCase data)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                _ = window.FindComboBox("Culture").Select("sv-SE");
                var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
                doubleBox.Enter(data.Text);
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedWhenNotLocalized(TestCase data)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                _ = window.FindComboBox("Culture").Select("ja-JP");
                var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
                doubleBox.Enter(data.Text);
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputInvalid(string text, string infoMessage)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
                doubleBox.Enter(text);
                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());

                window.FindCheckBox("AllowDecimalPoint").IsChecked = false;
                if (infoMessage != null)
                {
                    Assert.AreEqual(true, doubleBox.HasValidationError());
                    Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                    Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                }
                else
                {
                    Assert.AreEqual(false, doubleBox.HasValidationError());
                }
            }
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputValid(string text, string infoMessage)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                window.FindCheckBox("AllowDecimalPoint").IsChecked = false;
                var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
                doubleBox.Text = text;
                window.FindButton("lose focus").Click();
                if (infoMessage != null)
                {
                    Assert.AreEqual(true, doubleBox.HasValidationError());
                    Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                    Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                }
                else
                {
                    Assert.AreEqual(false, doubleBox.HasValidationError());
                }

                window.FindCheckBox("AllowDecimalPoint").IsChecked = true;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                //// Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                //// not sure about what to do here.
                //// calling UpdateSource() is easy enough but dunno
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
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
