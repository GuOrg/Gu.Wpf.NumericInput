namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Globalization;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class FormatTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly CultureInfo EnUs = CultureInfo.GetCultureInfo("en-US");
        private static readonly CultureInfo SvSe = CultureInfo.GetCultureInfo("sv-SE");

        private static readonly IReadOnlyList<TestCase> TestCases = new[]
        {
            new TestCase("1", "F1", EnUs, "1.0", "1"),
            new TestCase("1", "F1", SvSe, "1,0", "1"),
            new TestCase("1.23456", "F3", EnUs, "1.235", "1.23456"),
            new TestCase("1.23456", "F4", EnUs, "1.2346", "1.23456"),
            new TestCase("1", "0.#", EnUs, "1", "1"),
            new TestCase("1.23456", "0.###", EnUs, "1.235", "1.23456"),
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
        public void WithStringFormat(TestCase testCase)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
                window.FindTextBox("StringFormat").Text = testCase.StringFormat;
                window.FindComboBox("Culture").Select(testCase.Culture.Name);

                doubleBox.Enter(testCase.Text);
                window.FindButton("lose focus").Click();

                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual(testCase.Formatted, doubleBox.FormattedView().Text);
                Assert.AreEqual(testCase.ViewModelValue, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [Test]
        public void WhenStringFormatChanges()
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
                window.FindTextBox("StringFormat").Text = "F1";

                doubleBox.Text = "1.23456";
                window.FindButton("lose focus").Click();

                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual("1.23456", doubleBox.Text);
                Assert.AreEqual("1.2", doubleBox.FormattedView().Text);
                Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindTextBox("StringFormat").Text = "F4";
                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual("1.23456", doubleBox.Text);
                Assert.AreEqual("1.2346", doubleBox.FormattedView().Text);
                Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindTextBox("StringFormat").Text = "F1";
                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual("1.23456", doubleBox.Text);
                Assert.AreEqual("1.2", doubleBox.FormattedView().Text);
                Assert.AreEqual("1.23456", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        public class TestCase
        {
            public TestCase(string text, string stringFormat, CultureInfo culture, string formatted, string viewModelValue)
            {
                this.Text = text;
                this.StringFormat = stringFormat;
                this.Culture = culture;
                this.Formatted = formatted;
                this.ViewModelValue = viewModelValue;
            }

            public string Text { get; }

            public string StringFormat { get; }

            public CultureInfo Culture { get; }

            public string Formatted { get; }

            public string ViewModelValue { get; }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Formatted}";
        }
    }
}