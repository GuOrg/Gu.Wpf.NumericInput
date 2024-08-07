namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Text.RegularExpressions;

    using Gu.Wpf.UiAutomation;

    using NUnit.Framework;

    public static class ValidationErrorRegexTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly TestCaseData[] TestCases = new[]
        {
            new TestCaseData("1.2", @"^\d\.\d$", "1.2", null),
            new TestCaseData("12.34", @"^\d\.\d$", "0", "ValidationError.RegexValidationResult 'Please provide valid input.'"),
        };

        private static readonly TestCaseData[] SwedishCases = new[]
        {
            new TestCaseData("1,2",  @"^\d,\d$", "1.2", null),
            new TestCaseData("12,34",  @"^\d,\d$", "0", "ValidationError.RegexValidationResult 'Vänligen ange ett giltigt värde.'"),
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
        public static void LostFocusValidateOnLostFocus(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("RegexPattern").Text = pattern;

            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(doubleBox.Text, Is.EqualTo(text));
            Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("0"));
            Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));

            window.FindButton("lose focus").Click();
            if (expectedInfoMessage != null)
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(doubleBox.ValidationError(), Is.EqualTo(expectedInfoMessage));
                Assert.That(
                    window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text,
                    Is.EqualTo(GetErrorMessage(expectedInfoMessage))
                );
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(expected));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
            else
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(text));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
        }

        [TestCaseSource(nameof(TestCases))]
#pragma warning disable CA1801, IDE0060 // Review unused parameters
        public static void LostFocusValidateOnLostFocusWhenPatternChanges(string text, string pattern, string expected, string expectedInfoMessage)
#pragma warning restore CA1801, IDE0060 // Review unused parameters
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            window.FindButton("lose focus").Click();
            Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(doubleBox.Text, Is.EqualTo(text));
            Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(text));
            Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));

            window.FindTextBox("RegexPattern").Text = pattern;
            window.FindButton("lose focus").Click();
            if (expectedInfoMessage != null)
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(doubleBox.ValidationError(), Is.EqualTo(expectedInfoMessage));
                Assert.That(
                    window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text,
                    Is.EqualTo(GetErrorMessage(expectedInfoMessage))
                );
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(text));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
            else
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(text));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public static void LostFocusValidateOnPropertyChanged(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = pattern;

            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(doubleBox.ValidationError(), Is.EqualTo(expectedInfoMessage));
                Assert.That(
                    window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text,
                    Is.EqualTo(GetErrorMessage(expectedInfoMessage))
                );
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(expected));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));

                window.FindButton("lose focus").Click();
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(doubleBox.ValidationError(), Is.EqualTo(expectedInfoMessage));
                Assert.That(
                    window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text,
                    Is.EqualTo(GetErrorMessage(expectedInfoMessage))
                );
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("0"));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
            else
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("0"));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));

                window.FindButton("lose focus").Click();
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(text));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public static void PropertyChanged(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = pattern;

            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(
                    window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text,
                    Is.EqualTo(GetErrorMessage(expectedInfoMessage))
                );
                Assert.That(doubleBox.ValidationError(), Is.EqualTo(expectedInfoMessage));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(expected));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
            else
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(text));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public static void PropertyChangedSwedish(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = pattern;

            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(
                    window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text,
                    Is.EqualTo(GetErrorMessage(expectedInfoMessage))
                );
                Assert.That(doubleBox.ValidationError(), Is.EqualTo(expectedInfoMessage));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(expected));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
            else
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(expected));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public static void PropertyChangedWhenNotLocalized(string text, string pattern, string expected, string expectedInfoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("ja-JP");

            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("RegexPattern").Text = pattern;
            doubleBox.Text = text;
            if (expectedInfoMessage != null)
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(
                    window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text,
                    Is.EqualTo(GetErrorMessage(expectedInfoMessage))
                );
                Assert.That(doubleBox.ValidationError(), Is.EqualTo(expectedInfoMessage));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(expected));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
            else
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(expected));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            }
        }

        private static string GetErrorMessage(string infoMessage)
        {
            return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
        }
    }
}
