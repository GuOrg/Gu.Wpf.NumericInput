namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class ValidationErrorRegexTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly IReadOnlyList<TestCase> TestCases = new[]
        {
            new TestCase("1.2", @"^\d\.\d$", "1.2", null),
            new TestCase("12.34", @"^\d\.\d$", "0", "ValidationError.RegexValidationResult 'Please provide valid input.'"),
        };

        private static readonly IReadOnlyList<TestCase> SwedishCases = new[]
        {
            new TestCase("1,2",  @"^\d,\d$", "1.2", null),
            new TestCase("12,34",  @"^\d,\d$", "0", "ValidationError.RegexValidationResult 'Vänligen ange ett giltigt värde.'"),
        };

        [SetUp]
        public void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("ViewModelValue").Text = "0";
            window.FindButton("Reset").Invoke();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("RegexPattern").Text = data.Pattern;

            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocusWhenPatternChanges(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = data.Text;
            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.Text, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindTextBox("RegexPattern").Text = data.Pattern;
            window.FindButton("lose focus").Click();
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = data.Pattern;

            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChanged(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = data.Pattern;

            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void PropertyChangedSwedish(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = data.Pattern;

            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedWhenNotLocalized(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("ja-JP");

            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = data.Pattern;
            doubleBox.Text = data.Text;
            if (data.ExpectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
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
