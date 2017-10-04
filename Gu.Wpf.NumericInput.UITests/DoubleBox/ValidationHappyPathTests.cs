namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class ValidationHappyPathTests
    {
        private const string WindowName = "DoubleBoxValidationWindow";

        private static readonly TestCase[] TestCases =
        {
            new TestCase("1", "1"),
            new TestCase(" 1", "1"),
            new TestCase("1 ", "1"),
            new TestCase(" 1 ", "1"),
            new TestCase("1.2", "1.2"),
            new TestCase("-1.2", "-1.2"),
            new TestCase("+1.2", "1.2"),
            new TestCase(".1", "0.1"),
            new TestCase("-.1", "-0.1"),
            new TestCase("0.1", "0.1"),
            new TestCase("1e1", "10"),
            new TestCase("1e0", "1"),
            new TestCase("1e-1", "0.1"),
            new TestCase("1E1", "10"),
            new TestCase("1E0", "1"),
            new TestCase("1E-1", "0.1"),
            new TestCase("-1e1", "-10"),
            new TestCase("-1e0", "-1"),
            new TestCase("-1e-1", "-0.1"),
            new TestCase("-1E1", "-10"),
            new TestCase("-1E0", "-1"),
            new TestCase("-1E-1", "-0.1"),
        };

        private static readonly TestCase[] SwedishCases =
            {
                new TestCase("1", "1"),
                new TestCase(" 1", "1"),
                new TestCase("1 ", "1"),
                new TestCase(" 1 ", "1"),
                new TestCase("1,2", "1.2"),
                new TestCase("-1,2", "-1.2"),
                new TestCase("+1,2", "1.2"),
                new TestCase(",1", "0.1"),
                new TestCase("-,1", "-0.1"),
                new TestCase("0,1", "0.1"),
                new TestCase("1e1", "10"),
                new TestCase("1e0", "1"),
                new TestCase("1e-1", "0.1"),
                new TestCase("1E1", "10"),
                new TestCase("1E0", "1"),
                new TestCase("1E-1", "0.1"),
                new TestCase("-1e1", "-10"),
                new TestCase("-1e0", "-1"),
                new TestCase("-1e-1", "-0.1"),
                new TestCase("-1E1", "-10"),
                new TestCase("-1E0", "-1"),
                new TestCase("-1E-1", "-0.1"),
            };

        private static readonly MinMaxData[] MinMaxSource =
            {
                new MinMaxData("1", string.Empty, string.Empty, "1"),
                new MinMaxData("-1", "-1", string.Empty, "-1"),
                new MinMaxData("-1", "-10", string.Empty, "-1"),
                new MinMaxData("1", string.Empty, "1", "1"),
                new MinMaxData("1", string.Empty, "10", "1"),
                new MinMaxData("-2", "-2", "2", "-2"),
                new MinMaxData("-1", "-2", "2", "-1"),
                new MinMaxData("1", "-2", "2", "1"),
                new MinMaxData("2", "-2", "2", "2"),
            };

        [SetUp]
        public void SetUp()
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                window.FindButton("Reset").Invoke();
                window.WaitUntilResponsive();
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Application.KillLaunched(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"));
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnLostFocus(TestCase testCase)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
                doubleBox.Text = testCase.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual(testCase.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void LostFocusValidateOnPropertyChanged(TestCase testCase)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
                doubleBox.Text = testCase.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual(testCase.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(TestCases))]
        public void PropertyChangedValidateOnPropertyChanged(TestCase testCase)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
                doubleBox.Text = testCase.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual(testCase.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void SwedishLostFocusValidateOnLostFocus(TestCase testCase)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                 window.FindComboBox("Culture").Select("sv-SE");
                var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
                doubleBox.Text = testCase.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual(testCase.Expected, window.FindTextBox("ViewModelValue").Text);
                Assert.AreEqual(TextSource.UserInput, doubleBox.TextSource());
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void SwedishLostFocusValidateOnPropertyChanged(TestCase testCase)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                 window.FindComboBox("Culture").Select("sv-SE");
                var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
                doubleBox.Text = testCase.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual(testCase.Expected, window.FindTextBox("ViewModelValue").Text);
            }
        }

        [TestCaseSource(nameof(SwedishCases))]
        public void SwedishPropertyChangedValidateOnPropertyChanged(TestCase testCase)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                 window.FindComboBox("Culture").Select("sv-SE");
                var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
                doubleBox.Text = testCase.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(testCase.Text, doubleBox.Text);
                Assert.AreEqual(testCase.Expected, window.FindTextBox("ViewModelValue").Text);
            }
        }

        [Test]
        public void WhenNullLostFocusValidateOnLostFocus()
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
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
        }

        [Test]
        public void WhenNullLostFocusValidateOnPropertyChanged()
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
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
        }

        [Test]
        public void WheNullPropertyChanged()
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                window.FindCheckBox("CanValueBeNull").IsChecked = true;
                var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
                doubleBox.Text = string.Empty;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(string.Empty, doubleBox.Text);
                Assert.AreEqual(string.Empty, window.FindTextBox("ViewModelValue").Text);
            }
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxLostFocus(MinMaxData data)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                window.FindTextBox("Min").Text = data.Min;
                window.FindTextBox("Max").Text = data.Max;
                var doubleBox = window.FindTextBox("LostFocusValidateOnLostFocusBox");
                doubleBox.Text = data.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
            }
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxLostFocusValidateOnPropertyChanged(MinMaxData data)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                window.FindTextBox("Min").Text = data.Min;
                window.FindTextBox("Max").Text = data.Max;
                var doubleBox = window.FindTextBox("LostFocusValidateOnPropertyChangedBox");
                doubleBox.Text = data.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual("0", window.FindTextBox("ViewModelValue").Text);

                window.FindButton("lose focus").Click();
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
            }
        }

        [TestCaseSource(nameof(MinMaxSource))]
        public void MinMaxPropertyChanged(MinMaxData data)
        {
            using (var app = Application.AttachOrLaunch(Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"), WindowName))
            {
                var window = app.MainWindow;
                window.FindTextBox("Min").Text = data.Min;
                window.FindTextBox("Max").Text = data.Max;
                var doubleBox = window.FindTextBox("PropertyChangedValidateOnPropertyChangedBox");
                doubleBox.Text = data.Text;
                Assert.AreEqual(false, doubleBox.HasValidationError());
                Assert.AreEqual(data.Text, doubleBox.Text);
                Assert.AreEqual(data.Expected, window.FindTextBox("ViewModelValue").Text);
            }
        }

        public class TestCase
        {
            public TestCase(string text, string expected)
            {
                this.Text = text;
                this.Expected = expected;
            }

            public string Text { get; }

            public string Expected { get; }

            public override string ToString() => $"Text: {this.Text}, Expected: {this.Expected}";
        }

        public class MinMaxData
        {
            public MinMaxData(string text, string min, string max, string expected)
            {
                this.Text = text;
                this.Min = min;
                this.Max = max;
                this.Expected = expected;
            }

            public string Text { get; }

            public string Min { get; }

            public string Max { get; }

            public string Expected { get; }

            public override string ToString() => $"Text: {this.Text}, Min: {this.Min}, Max: {this.Max}, Expected: {this.Expected}";
        }
    }
}