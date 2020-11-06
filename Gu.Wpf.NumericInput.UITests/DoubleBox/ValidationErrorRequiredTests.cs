namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;

    using Gu.Wpf.UiAutomation;

    using NUnit.Framework;

    public static class ValidationErrorRequiredTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly TestCaseData[] TestCases = new[]
        {
            new TestCaseData("1.2", true, "1.2", null),
            new TestCaseData("1.2", false, "1.2", null),
            new TestCaseData(string.Empty, false, "0", "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'"),
            new TestCaseData(string.Empty, true, string.Empty, null),
        };

        private static readonly TestCaseData[] SwedishCases = new[]
        {
            new TestCaseData("1,2", true, "1.2", null),
            new TestCaseData("1,2", false, "1.2", null),
            new TestCaseData(string.Empty, false, "0", "ValidationError.RequiredButMissingValidationResult 'Vänligen ange en siffra.'"),
            new TestCaseData(string.Empty, true, string.Empty, null),
        };

        [SetUp]
        public static void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("ViewModelValue").Text = "0";
            window.FindButton("Reset").Invoke();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestCaseSource(nameof(TestCases))]
        public static void LostFocusValidateOnLostFocus(string text, bool canValueBeNull, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = canValueBeNull;

            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCase("1.2", null)]
        [TestCase("", "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'")]
        public static void LostFocusValidateOnLostFocusWhenIsRequiredChangesMakingInputInvalid(string text, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            window.FindButton("lose focus").Click();

            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindCheckBox("CanValueBeNull").IsChecked = false;
            window.FindButton("lose focus").Click();
            if (infoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(infoMessage), window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCase("1.2", null)]
        [TestCase("", "ValidationError.RequiredButMissingValidationResult 'Please enter a number.'")]
        public static void LostFocusValidateOnLostFocusWhenIsRequiredChangesMakingInputValid(string text, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = false;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            window.FindButton("lose focus").Click();
            if (infoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(infoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(infoMessage), window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }

            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            ////Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public static void LostFocusValidateOnPropertyChanged(string text, bool canValueBeNull, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = canValueBeNull;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");

            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public static void PropertyChanged(string text, bool canValueBeNull, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = canValueBeNull;

            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(text, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public static void PropertyChangedSwedish(string text, bool canValueBeNull, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");

            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindCheckBox("CanValueBeNull").IsChecked = canValueBeNull;

            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public static void PropertyChangedWhenNotLocalized(string text, bool canValueBeNull, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("ja-JP");

            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindCheckBox("CanValueBeNull").IsChecked = canValueBeNull;
            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.AreEqual(true, doubleBox.HasValidationError());
                Assert.AreEqual(GetErrorMessage(expectedInfoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
                Assert.AreEqual(expectedInfoMessage, doubleBox.ValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
            else
            {
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(text, doubleBox.Text);
                Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        public static string GetErrorMessage(string infoMessage)
        {
            return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
        }
    }
}
