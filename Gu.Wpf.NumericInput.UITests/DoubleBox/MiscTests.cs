namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Threading;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class MiscTests
    {
        private const string WindowName = "MiscTestsWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly IReadOnlyList<string> BoxContainerIds = new[]
        {
            "VanillaGroupBox",
            "DataTemplateGroupBox",
            "ControlTemplate",
        };

        [SetUp]
        public static void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("VmValueBox").Text = "0";
            window.FindButton("Reset").Invoke();
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void UpdatesViewModel(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
            inputBox.Enter("1.2");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void UpdatesFromViewModel(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            Assert.AreEqual(window.FindTextBox("VmValueBox").Text, inputBox.Text);
            Assert.AreEqual("0", inputBox.Text);
            window.FindTextBox("VmValueBox").Enter("1.2");
            inputBox.Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void CanBeNull(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var canBeNullBox = window.Window.FindCheckBox("CanBeNullBox");

            canBeNullBox.IsChecked = true;
            Assert.AreEqual("0", inputBox.Text);
            inputBox.Enter(string.Empty);
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual(string.Empty, window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("1");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1", inputBox.EditText());
            Assert.AreEqual("1", inputBox.FormattedText());
            Assert.AreEqual("1", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            canBeNullBox.IsChecked = false;
            inputBox.Enter(string.Empty);
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual("1", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            canBeNullBox.IsChecked = true;
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual("1", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Click();
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual(string.Empty, inputBox.EditText());
            Assert.AreEqual(string.Empty, inputBox.FormattedText());
            Assert.AreEqual(string.Empty, window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(string.Empty, inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void Culture(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.Window.FindCheckBox("AllowThousandsBox").IsChecked = false;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var cultureBox = window.Window.FindComboBox("CultureBox");
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());
            inputBox.Enter("1.2");
            Assert.AreEqual(false, inputBox.HasValidationError());
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());

            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            _ = cultureBox.Select("sv-SE");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1,2", inputBox.EditText());
            Assert.AreEqual("1,2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            cultureBox.Focus();
            _ = cultureBox.Select("en-US");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Click();
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2.3", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("5.6a");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("5.6a", inputBox.EditText());
            Assert.AreEqual("5.6a", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("2.3", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            _ = cultureBox.Select("sv-SE");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("5.6a", inputBox.EditText());
            Assert.AreEqual("5.6a", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("2.3", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void ValidationTriggerLostFocus(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            _ = window.Window.FindComboBox("ValidationTriggerBox").Select(ValidationTrigger.LostFocus.ToString());
            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
            inputBox.Enter("1.2");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("ggg");
            Assert.AreEqual("ggg", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("ggg", inputBox.EditText());
            Assert.AreEqual("ggg", inputBox.FormattedText());
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void NumberStylesAllowLeadingSign(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var signBox = window.Window.FindCheckBox("AllowLeadingSignBox");
            inputBox.Enter("-1.2");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            signBox.IsChecked = false;
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            signBox.IsChecked = true;
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("-1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("-1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("-1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void UpdateDigitsWhenGreaterThanMax(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var digitsBox = window.FindTextBox("DigitsBox");
            var maxBox = window.FindTextBox("MaxBox");
            Assert.AreEqual("0", inputBox.Text);
            inputBox.Enter("1.2");
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("0", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            maxBox.Enter("1");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            digitsBox.Enter("4");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2000", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void DecimalDigits(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var digitsBox = window.FindTextBox("DigitsBox");
            Assert.AreEqual("0", inputBox.Text);
            digitsBox.Enter("4");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0.0000", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("0", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            window.FindTextBox("VmValueBox").Enter("1.2");
            inputBox.Click();
            Assert.AreEqual("1.2", inputBox.Text);
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2000", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            digitsBox.Enter("0");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            digitsBox.Enter("-3");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("1.234567");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.234567", inputBox.EditText());
            Assert.AreEqual("1.235", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.234567", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.234567", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            digitsBox.Enter("4");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.234567", inputBox.EditText());
            Assert.AreEqual("1.2346", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.234567", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.234567", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void StringFormat(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var stringFormatBox = window.Window.FindTextBox("StringFormatBox");

            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());

            inputBox.Enter("123456.78");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("123456.78", inputBox.Text);
            Assert.AreEqual("123456.78", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("123456.78", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("123456.78", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            window.Window.FindCheckBox("AllowThousandsBox").IsChecked = true;
            stringFormatBox.Enter("N3");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("123456.78", inputBox.EditText());
            Assert.AreEqual("123,456.780", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("123456.78", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("123456.78", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("2222.33333");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("2222.33333", inputBox.EditText());
            Assert.AreEqual("2,222.333", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2222.33333", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("2222.33333", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            window.FindTextBox("VmValueBox").Enter("4444.5555");
            inputBox.Click();
            Assert.AreEqual("4444.5555", inputBox.Text);
            Assert.AreEqual("4,444.556", inputBox.FormattedText());

            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("4444.5555", inputBox.EditText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("4444.5555", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("4444.5555", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void Max(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var maxBox = window.FindTextBox("MaxBox");
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());
            inputBox.Enter("1.2");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            maxBox.Enter("-1");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());

            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("2.3", inputBox.EditText());
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("1.2", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            maxBox.Enter("6");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            inputBox.Click();
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual("2.3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2.3", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("2.3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            window.FindTextBox("VmValueBox").Enter("7.8");
            inputBox.Click();
            Assert.AreEqual("7.8", inputBox.Text);
            Assert.AreEqual("7.8", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("7.8", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("7.8", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            maxBox.Enter("10");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("7.8", inputBox.Text);
            Assert.AreEqual("7.8", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("7.8", window.FindTextBox("VmValueBox").Text);
            Assert.AreEqual("7.8", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void Min(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var minBox = window.FindTextBox("MinBox");
            Assert.AreNotEqual("1.2", inputBox.Text);
            inputBox.Enter("1.2");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);

            minBox.Enter("4");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", inputBox.Text);
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);

            inputBox.Enter("2.3");
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text);

            minBox.Enter("-2");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("2.3", inputBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("1.2", window.FindTextBox("VmValueBox").Text); // maybe we want to update source here idk.

            inputBox.Enter("5.6");
            window.FindTextBox("VmValueBox").Click();
            Assert.AreEqual("5.6", inputBox.Text);
            Assert.AreEqual("5.6", window.FindTextBox("VmValueBox").Text);
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void Undo(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            Assert.AreEqual("0", inputBox.Text);
            inputBox.Click();
            Keyboard.Type("1");
            Assert.AreEqual("10", inputBox.Text);
            using (Keyboard.Hold(Key.CONTROL))
            {
                Keyboard.Type("z");
            }

            Assert.AreEqual("0", inputBox.Text);
            Assert.AreEqual("0", window.FindTextBox("VmValueBox").Text);
        }

        [Apartment(ApartmentState.STA)]
        [Test]
        public static void CopyTest()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var inputBox = window.Window.FindTextBox("InputBox");
            inputBox.Text = "1.2";
            using (Keyboard.Hold(Key.CONTROL))
            {
                Keyboard.Type("ac");
            }

            Thread.Sleep(100);
            var clipboardText = System.Windows.Clipboard.GetText();
            Assert.AreEqual("1.2", clipboardText);
        }
    }
}
