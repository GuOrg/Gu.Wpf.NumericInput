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
            Assert.That(inputBox.Text, Is.EqualTo("0"));
            Assert.That(inputBox.Value(), Is.EqualTo("0"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            inputBox.Enter("1.2");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void UpdatesFromViewModel(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            Assert.That(inputBox.Text, Is.EqualTo(window.FindTextBox("VmValueBox").Text));
            Assert.That(inputBox.Text, Is.EqualTo("0"));
            window.FindTextBox("VmValueBox").Enter("1.2");
            inputBox.Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
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
            Assert.That(inputBox.Text, Is.EqualTo("0"));
            inputBox.Enter(string.Empty);
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo(string.Empty));
            Assert.That(inputBox.FormattedText(), Is.EqualTo(string.Empty));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo(string.Empty));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(inputBox.Value(), Is.EqualTo(string.Empty));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Enter("1");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1"));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(inputBox.Value(), Is.EqualTo("1"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            canBeNullBox.IsChecked = false;
            inputBox.Enter(string.Empty);
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo(string.Empty));
            Assert.That(inputBox.FormattedText(), Is.EqualTo(string.Empty));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(inputBox.Value(), Is.EqualTo("1"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            canBeNullBox.IsChecked = true;
            Assert.That(inputBox.EditText(), Is.EqualTo(string.Empty));
            Assert.That(inputBox.FormattedText(), Is.EqualTo(string.Empty));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(inputBox.Value(), Is.EqualTo(string.Empty));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Click();
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo(string.Empty));
            Assert.That(inputBox.FormattedText(), Is.EqualTo(string.Empty));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo(string.Empty));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(inputBox.Value(), Is.EqualTo(string.Empty));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
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
            Assert.That(inputBox.EditText(), Is.EqualTo("0"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));
            inputBox.Enter("1.2");
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));

            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            _ = cultureBox.Select("sv-SE");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1,2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1,2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Enter("2.3");
            Assert.That(inputBox.EditText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            cultureBox.Focus();
            _ = cultureBox.Select("en-US");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("2.3"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Click();
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("2.3"));
            Assert.That(inputBox.Value(), Is.EqualTo("2.3"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Enter("5.6a");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("5.6a"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("5.6a"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("2.3"));
            Assert.That(inputBox.Value(), Is.EqualTo("2.3"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            _ = cultureBox.Select("sv-SE");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("5.6a"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("5.6a"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("2.3"));
            Assert.That(inputBox.Value(), Is.EqualTo("2.3"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void ValidationTriggerLostFocus(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            _ = window
                .Window.FindComboBox("ValidationTriggerBox")
                .Select(ValidationTrigger.LostFocus.ToString());
            Assert.That(inputBox.Text, Is.EqualTo("0"));
            Assert.That(inputBox.Value(), Is.EqualTo("0"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            inputBox.Enter("1.2");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Enter("ggg");
            Assert.That(inputBox.EditText(), Is.EqualTo("ggg"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("ggg"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("ggg"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
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
            Assert.That(inputBox.Text, Is.EqualTo("-1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("-1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("-1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            signBox.IsChecked = false;
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("-1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("-1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("-1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            signBox.IsChecked = true;
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("-1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("-1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("-1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
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
            Assert.That(inputBox.Text, Is.EqualTo("0"));
            inputBox.Enter("1.2");
            Assert.That(inputBox.Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("0"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            maxBox.Enter("1");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            digitsBox.Enter("4");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2000"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void DecimalDigits(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var digitsBox = window.FindTextBox("DigitsBox");
            Assert.That(inputBox.Text, Is.EqualTo("0"));
            digitsBox.Enter("4");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("0"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("0.0000"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("0"));
            Assert.That(inputBox.Value(), Is.EqualTo("0"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            window.FindTextBox("VmValueBox").Enter("1.2");
            inputBox.Click();
            Assert.That(inputBox.Text, Is.EqualTo("1.2"));
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2000"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            digitsBox.Enter("0");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            digitsBox.Enter("-3");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Enter("1.234567");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.234567"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.235"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.234567"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.234567"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            digitsBox.Enter("4");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.234567"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2346"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.234567"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.234567"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void StringFormat(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var stringFormatBox = window.Window.FindTextBox("StringFormatBox");

            Assert.That(inputBox.EditText(), Is.EqualTo("0"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));

            inputBox.Enter("123456.78");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("123456.78"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("123456.78"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("123456.78"));
            Assert.That(inputBox.Value(), Is.EqualTo("123456.78"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            window.Window.FindCheckBox("AllowThousandsBox").IsChecked = true;
            stringFormatBox.Enter("N3");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("123456.78"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("123,456.780"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("123456.78"));
            Assert.That(inputBox.Value(), Is.EqualTo("123456.78"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Enter("2222.33333");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("2222.33333"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("2,222.333"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("2222.33333"));
            Assert.That(inputBox.Value(), Is.EqualTo("2222.33333"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            window.FindTextBox("VmValueBox").Enter("4444.5555");
            inputBox.Click();
            Assert.That(inputBox.Text, Is.EqualTo("4444.5555"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("4,444.556"));

            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("4444.5555"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("4444.5555"));
            Assert.That(inputBox.Value(), Is.EqualTo("4444.5555"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void Max(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var maxBox = window.FindTextBox("MaxBox");
            Assert.That(inputBox.EditText(), Is.EqualTo("0"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));
            inputBox.Enter("1.2");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            maxBox.Enter("-1");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Enter("2.3");
            Assert.That(inputBox.EditText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));

            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.EditText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("1.2"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            maxBox.Enter("6");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("2.3"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));
            Assert.That(inputBox.Value(), Is.EqualTo("2.3"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            inputBox.Click();
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("2.3"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("2.3"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("2.3"));
            Assert.That(inputBox.Value(), Is.EqualTo("2.3"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            window.FindTextBox("VmValueBox").Enter("7.8");
            inputBox.Click();
            Assert.That(inputBox.Text, Is.EqualTo("7.8"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("7.8"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("7.8"));
            Assert.That(inputBox.Value(), Is.EqualTo("7.8"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

            maxBox.Enter("10");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("7.8"));
            Assert.That(inputBox.FormattedText(), Is.EqualTo("7.8"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("7.8"));
            Assert.That(inputBox.Value(), Is.EqualTo("7.8"));
            Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
            Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
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
            Assert.That(inputBox.Text, Is.EqualTo("1.2"));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));

            minBox.Enter("4");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(inputBox.Text, Is.EqualTo("1.2"));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));

            inputBox.Enter("2.3");
            Assert.That(inputBox.Text, Is.EqualTo("2.3"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2"));

            minBox.Enter("-2");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("2.3"));
            Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("1.2")); // maybe we want to update source here idk.

            inputBox.Enter("5.6");
            window.FindTextBox("VmValueBox").Click();
            Assert.That(inputBox.Text, Is.EqualTo("5.6"));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("5.6"));
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void Undo(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            Assert.That(inputBox.Text, Is.EqualTo("0"));
            inputBox.Click();
            Keyboard.Type("1");
            Assert.That(inputBox.Text, Is.EqualTo("10"));
            using (Keyboard.Hold(Key.CONTROL))
            {
                Keyboard.Type("z");
            }

            Assert.That(inputBox.Text, Is.EqualTo("0"));
            Assert.That(window.FindTextBox("VmValueBox").Text, Is.EqualTo("0"));
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
            Assert.That(clipboardText, Is.EqualTo("1.2"));
        }
    }
}
