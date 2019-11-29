namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class ValidationErrorMinMaxTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        private static readonly IReadOnlyList<TestCase> TestCases = new[]
        {
            new TestCase("-2", "-1", string.Empty, "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.'"),
            new TestCase("-2.1", "-1.1", string.Empty, "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -1.1.'"),
            new TestCase("-2", "-1", "1", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value between -1 and 1.'"),
            new TestCase("-2.1", "-1.1", "1.1", "-1", "ValidationError.IsLessThanValidationResult 'Please enter a value between -1.1 and 1.1.'"),
            new TestCase("2", string.Empty, "1", "1", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.'"),
            new TestCase("2.1", string.Empty, "1.1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 1.1.'"),
            new TestCase("2", "-1", "1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value between -1 and 1.'"),
            new TestCase("2.1", "-1.1", "1.1", "1",  "ValidationError.IsGreaterThanValidationResult 'Please enter a value between -1.1 and 1.1.'"),
        };

        [SetUp]
        public void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindButton("Reset").Invoke();
            window.WaitUntilResponsive();
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
            var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
            doubleBox.Text = data.StartValue;
            window.FindTextBox("Min").Text = data.Min;
            window.FindTextBox("Max").Text = data.Max;

            doubleBox.Text = data.Text;
            Assert.AreEqual(false, doubleBox.HasValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("LostFocusValidateOnLostFocusBoxError").Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = data.StartValue;
            window.FindButton("lose focus").Click();
            window.FindTextBox("Min").Text = data.Min;
            window.FindTextBox("Max").Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedValidateOnPropertyChanged(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = data.StartValue;
            window.FindTextBox("Min").Text = data.Min;
            window.FindTextBox("Max").Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(data.StartValue, window.FindTextBox("ViewModelValue").Text);
            Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
        }

        [TestCase("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Vänligen ange ett värde mindre än eller lika med 2,2.'")]
        [TestCase("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Vänligen ange ett värde större än eller lika med -2,1.'")]
        public void PropertyChangedSwedish(string value, string min, string max, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("sv-SE");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "1";
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            doubleBox.Text = value;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual("1", window.FindTextBox("ViewModelValue").Text);
        }

        [TestCase("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [TestCase("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void PropertyChangedWhenNotLocalized(string value, string min, string max, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            _ = window.FindComboBox("Culture").Select("ja-JP");
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = "1";
            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            doubleBox.Text = value;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual("1", window.FindTextBox("ViewModelValue").Text);
        }

        [TestCase("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [TestCase("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void LostFocusValidateOnLostFocusWhenMinAndMaxChanges(string value, string min, string max, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
            doubleBox.Text = value;
            Assert.AreEqual(false, doubleBox.HasValidationError());

            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), window.FindTextBlock("LostFocusValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual(value, window.FindTextBox("ViewModelValue").Text);
        }

        [TestCase("3", "", "2.2", "ValidationError.IsGreaterThanValidationResult 'Please enter a value less than or equal to 2.2.'")]
        [TestCase("-3", "-2.1", "", "ValidationError.IsLessThanValidationResult 'Please enter a value greater than or equal to -2.1.'")]
        public void PropertyChangedWhenMinAndMaxChanges(string value, string min, string max, string infoMessage)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = value;
            Assert.AreEqual(false, doubleBox.HasValidationError());

            window.FindTextBox("Min").Text = min;
            window.FindTextBox("Max").Text = max;
            window.FindButton("lose focus").Click();
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(TestCase.GetErrorMessage(infoMessage), window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(infoMessage, doubleBox.ValidationError());
            Assert.AreEqual(value, doubleBox.Text);
            Assert.AreEqual(value, window.FindTextBox("ViewModelValue").Text);
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedWhenNull(TestCase data)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("CanValueBeNull").IsChecked = true;
            var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
            doubleBox.Text = string.Empty;
            window.FindTextBox("Min").Text = data.Min;
            window.FindTextBox("Max").Text = data.Max;
            doubleBox.Text = data.Text;
            Assert.AreEqual(true, doubleBox.HasValidationError());
            Assert.AreEqual(data.ExpectedInfoMessage, doubleBox.ValidationError());
            Assert.AreEqual(data.ErrorMessage, window.FindTextBlock("PropertyChangedValidateOnPropertyChangedBoxError").Text);
            Assert.AreEqual(data.Text, doubleBox.Text);
            Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
        }

        public class TestCase
        {
            public TestCase(string text, string min, string max, string expected, string expectedInfoMessage)
            {
                this.Text = text;
                this.Min = min;
                this.Max = max;
                this.Expected = expected;
                this.ExpectedInfoMessage = expectedInfoMessage;
            }

            public string Text { get; }

            public string Min { get; }

            public string Max { get; }

            public string Expected { get; }

            public string ExpectedInfoMessage { get; }

            public string ErrorMessage => GetErrorMessage(this.ExpectedInfoMessage);

            public string StartValue => string.IsNullOrEmpty(this.Min)
                                            ? this.Max
                                            : this.Min;

            public static string GetErrorMessage(string infoMessage)
            {
                return Regex.Match(infoMessage, "[^']+'(?<inner>[^']+)'.*").Groups["inner"].Value;
            }

            public override string ToString() => $"Text: {this.Text}, Min: {this.Min}, Max: {this.Max}, Expected: {this.Expected}, ExpectedMessage: {this.ExpectedInfoMessage}";
        }
    }
}
