namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Globalization;

    using Gu.Wpf.UiAutomation;

    using NUnit.Framework;

    public static class FormatTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly CultureInfo EnUs = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo SvSe = CultureInfo.GetCultureInfo("sv-SE");

        private static readonly TestCaseData[] TestCases = new[]
        {
            new TestCaseData("1", "F1", EnUs, "1.0", "1"),
            new TestCaseData("1", "F1", SvSe, "1,0", "1"),
            new TestCaseData("1.23456", "F3", EnUs, "1.235", "1.23456"),
            new TestCaseData("1.23456", "F4", EnUs, "1.2346", "1.23456"),
            new TestCaseData("1", "0.#", EnUs, "1", "1"),
            new TestCaseData("1.23456", "0.###", EnUs, "1.235", "1.23456"),
        };

        [SetUp]
        public static void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindButton("Reset").Invoke();
            window.WaitUntilResponsive();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestCaseSource(nameof(TestCases))]
        public static void WithStringFormat(string text, string stringFormat, CultureInfo culture, string formatted, string viewModelValue)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            window.FindTextBox("StringFormat").Text = stringFormat;
            _ = window.FindComboBox("Culture").Select(culture.Name);

            doubleBox.Enter(text);
            window.FindButton("lose focus").Click();

            Assert.Multiple(() =>
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo(text));
                Assert.That(doubleBox.FormattedView().Text, Is.EqualTo(formatted));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo(viewModelValue));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            });
        }

        [Test]
        public static void WhenStringFormatChangesBindingLostFocusValidateOnPropertyChanged()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            window.FindTextBox("StringFormat").Text = "F1";

            doubleBox.Text = "1.23456";
            window.FindButton("lose focus").Click();

            Assert.Multiple(() =>
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.FormattedView().Text, Is.EqualTo("1.2"));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            });

            window.FindTextBox("StringFormat").Text = "F4";
            window.FindButton("lose focus").Click();
            Assert.Multiple(() =>
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.FormattedView().Text, Is.EqualTo("1.2346"));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            });

            window.FindTextBox("StringFormat").Text = "F1";
            window.FindButton("lose focus").Click();
            Assert.Multiple(() =>
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.FormattedView().Text, Is.EqualTo("1.2"));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            });
        }

        [Test]
        public static void WhenStringFormatChangesBindingPropertyChangedValidateOnPropertyChanged()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            window.FindTextBox("StringFormat").Text = "F1";

            doubleBox.Text = "1.23456";
            window.FindButton("lose focus").Click();

            Assert.Multiple(() =>
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.FormattedView().Text, Is.EqualTo("1.2"));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            });

            window.FindTextBox("StringFormat").Text = "F4";
            window.FindButton("lose focus").Click();
            Assert.Multiple(() =>
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.FormattedView().Text, Is.EqualTo("1.2346"));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            });

            window.FindTextBox("StringFormat").Text = "F1";
            window.FindButton("lose focus").Click();
            Assert.Multiple(() =>
            {
                Assert.That(doubleBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(doubleBox.Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.FormattedView().Text, Is.EqualTo("1.2"));
                Assert.That(window.FindTextBox("ViewModelValue").Text, Is.EqualTo("1.23456"));
                Assert.That(doubleBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            });
        }
    }
}
