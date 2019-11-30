namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class ValidationHappyPathTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly TestCaseData[] EnglishCases =
        {
            new TestCaseData("1", "1"),
            new TestCaseData(" 1", "1"),
            new TestCaseData("1 ", "1"),
            new TestCaseData(" 1 ", "1"),
            new TestCaseData("1.2", "1.2"),
            new TestCaseData("-1.2", "-1.2"),
            new TestCaseData("+1.2", "1.2"),
            new TestCaseData(".1", "0.1"),
            new TestCaseData("-.1", "-0.1"),
            new TestCaseData("0.1", "0.1"),
            new TestCaseData("1e1", "10"),
            new TestCaseData("1e0", "1"),
            new TestCaseData("1e-1", "0.1"),
            new TestCaseData("1E1", "10"),
            new TestCaseData("1E0", "1"),
            new TestCaseData("1E-1", "0.1"),
            new TestCaseData("-1e1", "-10"),
            new TestCaseData("-1e0", "-1"),
            new TestCaseData("-1e-1", "-0.1"),
            new TestCaseData("-1E1", "-10"),
            new TestCaseData("-1E0", "-1"),
            new TestCaseData("-1E-1", "-0.1"),
        };

        private static readonly TestCaseData[] SwedishCases =
        {
            new TestCaseData("1", "1"),
            new TestCaseData(" 1", "1"),
            new TestCaseData("1 ", "1"),
            new TestCaseData(" 1 ", "1"),
            new TestCaseData("1,2", "1.2"),
            new TestCaseData("-1,2", "-1.2"),
            new TestCaseData("+1,2", "1.2"),
            new TestCaseData(",1", "0.1"),
            new TestCaseData("-,1", "-0.1"),
            new TestCaseData("0,1", "0.1"),
            new TestCaseData("1e1", "10"),
            new TestCaseData("1e0", "1"),
            new TestCaseData("1e-1", "0.1"),
            new TestCaseData("1E1", "10"),
            new TestCaseData("1E0", "1"),
            new TestCaseData("1E-1", "0.1"),
            new TestCaseData("-1e1", "-10"),
            new TestCaseData("-1e0", "-1"),
            new TestCaseData("-1e-1", "-0.1"),
            new TestCaseData("-1E1", "-10"),
            new TestCaseData("-1E0", "-1"),
            new TestCaseData("-1E-1", "-0.1"),
        };

        private static readonly TestCaseData[] MinMaxSource =
        {
            new TestCaseData("1", string.Empty, string.Empty, "1"),
            new TestCaseData("-1", "-1", string.Empty, "-1"),
            new TestCaseData("-1", "-10", string.Empty, "-1"),
            new TestCaseData("1", string.Empty, "1", "1"),
            new TestCaseData("1", string.Empty, "10", "1"),
            new TestCaseData("-2", "-2", "2", "-2"),
            new TestCaseData("-1", "-2", "2", "-1"),
            new TestCaseData("1", "-2", "2", "1"),
            new TestCaseData("2", "-2", "2", "2"),
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

        [TestCaseSource(nameof(EnglishCases))]
        public static void LostFocusValidateOnLostFocus(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(EnglishCases))]
        public static void LostFocusValidateOnPropertyChanged(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(EnglishCases))]
        public static void PropertyChangedValidateOnPropertyChanged(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishCases))]
        public static void SwedishLostFocusValidateOnLostFocus(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(SwedishCases))]
        public static void SwedishLostFocusValidateOnPropertyChanged(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }

        [TestCaseSource(nameof(SwedishCases))]
        public static void SwedishPropertyChangedValidateOnPropertyChanged(string text, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }

        [Test]
        public static void WhenNullLostFocusValidateOnLostFocus()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
        }

        [Test]
        public static void WhenNullLostFocusValidateOnPropertyChanged()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
        }

        [Test]
        public static void WheNullPropertyChanged()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = string.Empty;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(string.Empty, doubleBox.Text);
            Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public static void MinMaxLostFocus(string text, string min, string max, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public static void MinMaxLostFocusValidateOnPropertyChanged(string text, string min, string max, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

            window.FindButton("lose focus").Click();
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public static void MinMaxPropertyChanged(string text, string min, string max, string expected)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(text, doubleBox.Text);
            Assert.AreEqual(expected, window.FindTextBox("ViewModelValue").Text);
        }
    }
}
