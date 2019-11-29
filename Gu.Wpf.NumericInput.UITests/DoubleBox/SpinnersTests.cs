namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System.Collections.Generic;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class SpinnersTests
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

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdatesViewModel(string containerId)
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
            Assert.AreEqual("1.23", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.23", vmValueBox.Text);
            Assert.AreEqual("1.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(true, increaseButton.IsEnabled);
            Assert.AreEqual(true, decreaseButton.IsEnabled);
            increaseButton.Click();
            Assert.AreEqual("2.23", inputBox.EditText());
            Assert.AreEqual("2.2", inputBox.FormattedText());
            Assert.AreEqual("1.23", vmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            vmValueBox.Click();

            Assert.AreEqual("2.23", inputBox.EditText());
            Assert.AreEqual("2.2", inputBox.FormattedText());
            Assert.AreEqual("2.23", vmValueBox.Text);
            Assert.AreEqual("2.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            window.FindTextBox("IncrementBox").Enter("5");
            vmValueBox.Click();
            increaseButton.Click();
            vmValueBox.Click();
            Assert.AreEqual("7.23", inputBox.EditText());
            Assert.AreEqual("7.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("7.23", vmValueBox.Text);
            Assert.AreEqual("7.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            decreaseButton.Click();
            vmValueBox.Click();
            Assert.AreEqual("2.23", inputBox.EditText());
            Assert.AreEqual("2.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2.23", vmValueBox.Text);
            Assert.AreEqual("2.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdatesViewModelSpinUpdateModePropertyChanged(string containerId)
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
            Assert.AreEqual("1.23", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.23", vmValueBox.Text);
            Assert.AreEqual("1.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(true, increaseButton.IsEnabled);
            Assert.AreEqual(true, decreaseButton.IsEnabled);
            increaseButton.Click();
            Assert.AreEqual("2.23", inputBox.EditText());
            Assert.AreEqual("2.2", inputBox.FormattedText());
            Assert.AreEqual("2.23", vmValueBox.Text);
            Assert.AreEqual(false, inputBox.HasValidationError());
            vmValueBox.Click();

            Assert.AreEqual("2.23", inputBox.EditText());
            Assert.AreEqual("2.2", inputBox.FormattedText());
            Assert.AreEqual("2.23", vmValueBox.Text);
            Assert.AreEqual("2.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            window.FindTextBox("IncrementBox").Enter("5");
            vmValueBox.Click();
            increaseButton.Click();
            Assert.AreEqual("7.23", inputBox.EditText());
            Assert.AreEqual("7.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("7.23", vmValueBox.Text);
            Assert.AreEqual("7.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            decreaseButton.Click();
            Assert.AreEqual("2.23", inputBox.EditText());
            Assert.AreEqual("2.2", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("2.23", vmValueBox.Text);
            Assert.AreEqual("2.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void TruncatesToMax(string containerId)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
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
                Assert.AreEqual("0", inputBox.EditText());
                Assert.AreEqual("0", inputBox.FormattedText());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("0", inputBox.Value());
                Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());

                Assert.AreEqual(true, increaseButton.IsEnabled);
                Assert.AreEqual(true, decreaseButton.IsEnabled);
                increaseButton.Click();
                vmValueBox.Click();
                Assert.AreEqual("3", inputBox.EditText());
                Assert.AreEqual("3", inputBox.FormattedText());
                Assert.AreEqual(false, inputBox.HasValidationError());
                Assert.AreEqual("3", vmValueBox.Text);
                Assert.AreEqual("3", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());

                Assert.AreEqual(false, increaseButton.IsEnabled);
                Assert.AreEqual(true, decreaseButton.IsEnabled);
            }
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void TruncatesToMin(string containerId)
        {
            using (var app = Application.AttachOrLaunch(
                ExeFileName,
                WindowName))
            {
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
                Assert.AreEqual("0", inputBox.EditText());
                Assert.AreEqual("0", inputBox.FormattedText());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("0", inputBox.Value());
                Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());

                Assert.AreEqual(true, increaseButton.IsEnabled);
                Assert.AreEqual(true, decreaseButton.IsEnabled);
                decreaseButton.Click();
                vmValueBox.Click();
                Assert.AreEqual("-3", inputBox.EditText());
                Assert.AreEqual("-3", inputBox.FormattedText());
                Assert.AreEqual(false, inputBox.HasValidationError());
                Assert.AreEqual("-3", vmValueBox.Text);
                Assert.AreEqual("-3", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());

                Assert.AreEqual(true, increaseButton.IsEnabled);
                Assert.AreEqual(false, decreaseButton.IsEnabled);
            }
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void DecreasesWhenGreaterThanMax(string containerId)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
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
                Assert.AreEqual("5", inputBox.EditText());
                Assert.AreEqual("5", inputBox.FormattedText());
                Assert.AreEqual(true, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("0", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());

                Assert.AreEqual(false, increaseButton.IsEnabled);
                Assert.AreEqual(true, decreaseButton.IsEnabled);
                decreaseButton.Click();
                Assert.AreEqual("4", inputBox.EditText());
                Assert.AreEqual("4", inputBox.FormattedText());
                Assert.AreEqual(true, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("0", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());

                Assert.AreEqual(false, increaseButton.IsEnabled);
                Assert.AreEqual(true, decreaseButton.IsEnabled);

                decreaseButton.Click();
                Assert.AreEqual("3", inputBox.EditText());
                Assert.AreEqual("3", inputBox.FormattedText());
                Assert.AreEqual(false, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("3", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());
            }
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void IncreasesWhenLessThanMin(string containerId)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
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
                Assert.AreEqual("-5", inputBox.EditText());
                Assert.AreEqual("-5", inputBox.FormattedText());
                Assert.AreEqual(true, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("0", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());
                Assert.AreEqual(true, increaseButton.IsEnabled);
                Assert.AreEqual(false, decreaseButton.IsEnabled);

                increaseButton.Click();
                Assert.AreEqual("-4", inputBox.EditText());
                Assert.AreEqual("-4", inputBox.FormattedText());
                Assert.AreEqual(true, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("0", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());
                Assert.AreEqual(true, increaseButton.IsEnabled);
                Assert.AreEqual(false, decreaseButton.IsEnabled);

                increaseButton.Click();
                Assert.AreEqual("-3", inputBox.EditText());
                Assert.AreEqual("-3", inputBox.FormattedText());
                Assert.AreEqual(false, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("-3", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());
            }
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Undo(string containerId)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
                var container = window.Window.FindGroupBox(containerId);
                var inputBox = container.FindTextBox("InputBox");
                var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
                var vmValueBox = window.FindTextBox("VmValueBox");
                Assert.AreEqual("0", inputBox.EditText());
                Assert.AreEqual("0", inputBox.FormattedText());
                increaseButton.Click();
                Assert.AreEqual("1", inputBox.EditText());
                Assert.AreEqual("1", inputBox.FormattedText());
                Assert.AreEqual(false, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("1", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());

                using (Keyboard.Hold(Key.CONTROL))
                {
                    Keyboard.Type("z");
                }

                vmValueBox.Click();
                Assert.AreEqual("0", inputBox.EditText());
                Assert.AreEqual("0", inputBox.FormattedText());
                Assert.AreEqual(false, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("0", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());
            }
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UndoWhenSpinUpdateModePropertyChanged(string containerId)
        {
            using (var app = Application.AttachOrLaunch(ExeFileName, WindowName))
            {
                var window = app.MainWindow;
                window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
                var container = window.Window.FindGroupBox(containerId);
                var inputBox = container.FindTextBox("InputBox");
                _ = window.Window.FindComboBox("SpinUpdateMode").Select("PropertyChanged");
                inputBox.Click();
                var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
                var vmValueBox = window.FindTextBox("VmValueBox");
                Assert.AreEqual("0", inputBox.EditText());
                Assert.AreEqual("0", inputBox.FormattedText());
                increaseButton.Click();
                Assert.AreEqual("1", inputBox.EditText());
                Assert.AreEqual("1", inputBox.FormattedText());
                Assert.AreEqual(false, inputBox.HasValidationError());
                Assert.AreEqual("1", vmValueBox.Text);
                Assert.AreEqual("1", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());

                using (Keyboard.Hold(Key.CONTROL))
                {
                    Keyboard.Type("z");
                }

                vmValueBox.Click();
                Assert.AreEqual("0", inputBox.EditText());
                Assert.AreEqual("0", inputBox.FormattedText());
                Assert.AreEqual(false, inputBox.HasValidationError());
                Assert.AreEqual("0", vmValueBox.Text);
                Assert.AreEqual("0", inputBox.Value());
                Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                Assert.AreEqual("Idle", inputBox.Status());
            }
        }
    }
}
