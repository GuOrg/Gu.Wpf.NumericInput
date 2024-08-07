namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class SpinnersTests
    {
        private const string WindowName = "SpinnerWindow";
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
            window.FindButton("Reset").Invoke();
            window.WaitUntilResponsive();
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
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            window.FindTextBox("DigitsBox").Text = "1";
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = window.FindTextBox("VmValueBox");
            inputBox.Text = "1.23";
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("1.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
                Assert.That(vmValueBox.Text, Is.EqualTo("1.23"));
                Assert.That(inputBox.Value(), Is.EqualTo("1.23"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

                Assert.That(increaseButton.IsEnabled, Is.EqualTo(true));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(true));
            });
            increaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("2.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("2.2"));
                Assert.That(vmValueBox.Text, Is.EqualTo("1.23"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            });
            vmValueBox.Click();

            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("2.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("2.2"));
                Assert.That(vmValueBox.Text, Is.EqualTo("2.23"));
                Assert.That(inputBox.Value(), Is.EqualTo("2.23"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });

            window.FindTextBox("IncrementBox").Enter("5");
            vmValueBox.Click();
            increaseButton.Click();
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("7.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("7.2"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("7.23"));
                Assert.That(inputBox.Value(), Is.EqualTo("7.23"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });

            decreaseButton.Click();
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("2.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("2.2"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("2.23"));
                Assert.That(inputBox.Value(), Is.EqualTo("2.23"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void UpdatesViewModelSpinUpdateModePropertyChanged(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            window.FindTextBox("DigitsBox").Text = "1";
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            _ = window.Window.FindComboBox("SpinUpdateMode").Select("PropertyChanged");
            inputBox.Click();
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = window.FindTextBox("VmValueBox");
            inputBox.Text = "1.23";
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("1.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("1.2"));
                Assert.That(vmValueBox.Text, Is.EqualTo("1.23"));
                Assert.That(inputBox.Value(), Is.EqualTo("1.23"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

                Assert.That(increaseButton.IsEnabled, Is.EqualTo(true));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(true));
            });
            increaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("2.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("2.2"));
                Assert.That(vmValueBox.Text, Is.EqualTo("2.23"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
            });
            vmValueBox.Click();

            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("2.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("2.2"));
                Assert.That(vmValueBox.Text, Is.EqualTo("2.23"));
                Assert.That(inputBox.Value(), Is.EqualTo("2.23"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });

            window.FindTextBox("IncrementBox").Enter("5");
            vmValueBox.Click();
            increaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("7.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("7.2"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("7.23"));
                Assert.That(inputBox.Value(), Is.EqualTo("7.23"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });

            decreaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("2.23"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("2.2"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("2.23"));
                Assert.That(inputBox.Value(), Is.EqualTo("2.23"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void TruncatesToMax(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            window.FindTextBox("IncrementBox").Text = "5";
            window.FindTextBox("MaxBox").Text = "3";
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = window.FindTextBox("VmValueBox");
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("0"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("0"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

                Assert.That(increaseButton.IsEnabled, Is.EqualTo(true));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(true));
            });
            increaseButton.Click();
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("3"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("3"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("3"));
                Assert.That(inputBox.Value(), Is.EqualTo("3"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

                Assert.That(increaseButton.IsEnabled, Is.EqualTo(false));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(true));
            });
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void TruncatesToMin(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            window.FindTextBox("IncrementBox").Text = "5";
            window.FindTextBox("MinBox").Text = "-3";
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = window.FindTextBox("VmValueBox");
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("0"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("0"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.ValueBinding));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

                Assert.That(increaseButton.IsEnabled, Is.EqualTo(true));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(true));
            });
            decreaseButton.Click();
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("-3"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("-3"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("-3"));
                Assert.That(inputBox.Value(), Is.EqualTo("-3"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

                Assert.That(increaseButton.IsEnabled, Is.EqualTo(true));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(false));
            });
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void DecreasesWhenGreaterThanMax(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            window.FindTextBox("MaxBox").Text = "3";
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = window.FindTextBox("VmValueBox");
            vmValueBox.Click();
            inputBox.Text = "5";
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("5"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("5"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("0"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

                Assert.That(increaseButton.IsEnabled, Is.EqualTo(false));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(true));
            });
            decreaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("4"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("4"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("0"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));

                Assert.That(increaseButton.IsEnabled, Is.EqualTo(false));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(true));
            });

            decreaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("3"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("3"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("3"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void IncreasesWhenLessThanMin(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            window.FindTextBox("MinBox").Text = "-3";
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = window.FindTextBox("VmValueBox");
            vmValueBox.Click();
            inputBox.Text = "-5";
            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("-5"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("-5"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("0"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
                Assert.That(increaseButton.IsEnabled, Is.EqualTo(true));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(false));
            });

            increaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("-4"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("-4"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(true));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("0"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
                Assert.That(increaseButton.IsEnabled, Is.EqualTo(true));
                Assert.That(decreaseButton.IsEnabled, Is.EqualTo(false));
            });

            increaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("-3"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("-3"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("-3"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void Undo(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var vmValueBox = window.FindTextBox("VmValueBox");
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("0"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));
            });
            increaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("1"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("1"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("1"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });

            using (Keyboard.Hold(Key.CONTROL))
            {
                Keyboard.Type("z");
            }

            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("0"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("0"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public static void UndoWhenSpinUpdateModePropertyChanged(string containerId)
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            var container = window.Window.FindGroupBox(containerId);
            var inputBox = container.FindTextBox("InputBox");
            _ = window.Window.FindComboBox("SpinUpdateMode").Select("PropertyChanged");
            inputBox.Click();
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var vmValueBox = window.FindTextBox("VmValueBox");
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("0"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));
            });
            increaseButton.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("1"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("1"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("1"));
                Assert.That(inputBox.Value(), Is.EqualTo("1"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });

            using (Keyboard.Hold(Key.CONTROL))
            {
                Keyboard.Type("z");
            }

            vmValueBox.Click();
            Assert.Multiple(() =>
            {
                Assert.That(inputBox.EditText(), Is.EqualTo("0"));
                Assert.That(inputBox.FormattedText(), Is.EqualTo("0"));
                Assert.That(inputBox.HasValidationError(), Is.EqualTo(false));
                Assert.That(vmValueBox.Text, Is.EqualTo("0"));
                Assert.That(inputBox.Value(), Is.EqualTo("0"));
                Assert.That(inputBox.TextSource(), Is.EqualTo(TextSource.UserInput));
                Assert.That(inputBox.Status(), Is.EqualTo("Idle"));
            });
        }
    }
}
