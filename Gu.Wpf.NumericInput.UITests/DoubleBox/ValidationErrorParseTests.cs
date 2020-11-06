namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;

    using Gu.Wpf.UiAutomation;

    using NUnit.Framework;

    public static class ValidationErrorParseTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly TestCaseData[] TestCases = new[]
            {
                new TestCaseData("abc", "0", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
                new TestCaseData("2,1", "2", "ValidationError.CanParseValidationResult 'Please enter a valid number.'"),
            };

        private static readonly TestCaseData[] SwedishCases = new[]
            {
                new TestCaseData("abc", "0", "ValidationError.CanParseValidationResult 'Vänligen ange en giltig siffra.'"),
                new TestCaseData("2.1", "2", "ValidationError.CanParseValidationResult 'Vänligen ange en giltig siffra.'"),
            };

        [SetUp]
        public static void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindButton("Reset").Invoke();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestCaseSource(nameof(TestCases))]
#pragma warning disable CA1801, IDE0060 // Review unused parameters
        public static void LostFocusValidateOnLostFocus(string text, string expected, string expectedInfoMessage)
#pragma warning restore CA1801, IDE0060 // Review unused parameters
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = "0";
            window.FindButton("lose focus").Click(); // needed to reset explicitly here for some reason

            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
#pragma warning disable CA1801, IDE0060 // Review unused parameters
        public static void LostFocusValidateOnPropertyChanged(string text, string expected, string expectedInfoMessage)
#pragma warning restore CA1801, IDE0060 // Review unused parameters
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public static void PropertyChanged(string text, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Enter(text);
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishCases))]
        public static void PropertyChangedSwedish(string text, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Enter(text);
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public static void PropertyChangedWhenNotLocalized(string text, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("ja-JP");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Enter(text);
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public static void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputInvalid(string text, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
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
                Assert.AreEqual(GetErrorMessage(infoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
            }
        }

        [TestCase("2.1", "ValidationError.CanParseValidationResult 'Please enter a valid number.'")]
        [TestCase("2", null)]
        public static void LostFocusValidateOnPropertyChangedWhenAllowDecimalPointChangesMakingInputValid(string text, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("AllowDecimalPoint").IsChecked = false;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            window.FindButton("lose focus").Click();
            if (infoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(infoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
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

        public static string GetErrorMessage(string infoMessage)
        {
            return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
        }
    }
}
