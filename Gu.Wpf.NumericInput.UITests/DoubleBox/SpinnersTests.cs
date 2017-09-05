namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using System.Collections.Generic;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class SpinnersTests : IDisposable
    {
        private static readonly IReadOnlyList<string> BoxContainerIds = new[]
        {
            "VanillaGroupBox",
            "DataTemplateGroupBox",
            "ControlTemplate"
        };

        private readonly DoubleBoxView view;

        private bool disposed;

        public SpinnersTests()
        {
            this.view = new DoubleBoxView("SpinnerWindow");
        }

        [SetUp]
        public void SetUp()
        {
            this.view.Reset();
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UpdatesViewModel(string containerId)
        {
            this.view.AllowSpinnersBox.IsChecked = true;
            this.view.DigitsBox.Enter("1");
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = this.view.VmValueBox;
            inputBox.Enter("1.23");
            vmValueBox.Click();
            Assert.AreEqual("1.23", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.23", vmValueBox.Text);
            Assert.AreEqual("1.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(true, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(true, decreaseButton.Properties.IsEnabled.Value);
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

            this.view.IncrementBox.Enter("5");
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
            this.view.AllowSpinnersBox.IsChecked = true;
            this.view.DigitsBox.Enter("1");
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            this.view.Window.FindComboBox("SpinUpdateMode").Select("PropertyChanged");
            inputBox.Click();
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = this.view.VmValueBox;
            inputBox.Enter("1.23");
            vmValueBox.Click();
            Assert.AreEqual("1.23", inputBox.EditText());
            Assert.AreEqual("1.2", inputBox.FormattedText());
            Assert.AreEqual("1.23", vmValueBox.Text);
            Assert.AreEqual("1.23", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(true, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(true, decreaseButton.Properties.IsEnabled.Value);
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

            this.view.IncrementBox.Enter("5");
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
            this.view.AllowSpinnersBox.IsChecked = true;
            this.view.IncrementBox.Enter("5");
            this.view.MaxBox.Enter("3");
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = this.view.VmValueBox;
            vmValueBox.Click();
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());
            Assert.AreEqual("0", vmValueBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(true, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(true, decreaseButton.Properties.IsEnabled.Value);
            increaseButton.Click();
            vmValueBox.Click();
            Assert.AreEqual("3", inputBox.EditText());
            Assert.AreEqual("3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("3", vmValueBox.Text);
            Assert.AreEqual("3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(false, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(true, decreaseButton.Properties.IsEnabled.Value);
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void TruncatesToMin(string containerId)
        {
            this.view.AllowSpinnersBox.IsChecked = true;
            this.view.IncrementBox.Enter("5");
            this.view.MinBox.Enter("-3");
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = this.view.VmValueBox;
            vmValueBox.Click();
            Assert.AreEqual("0", inputBox.EditText());
            Assert.AreEqual("0", inputBox.FormattedText());
            Assert.AreEqual("0", vmValueBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(true, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(true, decreaseButton.Properties.IsEnabled.Value);
            decreaseButton.Click();
            vmValueBox.Click();
            Assert.AreEqual("-3", inputBox.EditText());
            Assert.AreEqual("-3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("-3", vmValueBox.Text);
            Assert.AreEqual("-3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(true, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(false, decreaseButton.Properties.IsEnabled.Value);
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void DecreasesWhenGreaterThanMax(string containerId)
        {
            this.view.AllowSpinnersBox.IsChecked = true;
            this.view.MaxBox.Enter("3");
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = this.view.VmValueBox;
            vmValueBox.Click();
            inputBox.Enter("5");
            vmValueBox.Click();
            Assert.AreEqual("5", inputBox.EditText());
            Assert.AreEqual("5", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("0", vmValueBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(false, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(true, decreaseButton.Properties.IsEnabled.Value);
            decreaseButton.Click();
            Assert.AreEqual("4", inputBox.EditText());
            Assert.AreEqual("4", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("0", vmValueBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());

            Assert.AreEqual(false, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(true, decreaseButton.Properties.IsEnabled.Value);

            decreaseButton.Click();
            Assert.AreEqual("3", inputBox.EditText());
            Assert.AreEqual("3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("0", vmValueBox.Text);
            Assert.AreEqual("3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void IncreasesWhenLessThanMin(string containerId)
        {
            this.view.AllowSpinnersBox.IsChecked = true;
            this.view.MinBox.Enter("-3");
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var decreaseButton = container.FindButton(SpinnerDecorator.DecreaseButtonName);
            var vmValueBox = this.view.VmValueBox;
            vmValueBox.Click();
            inputBox.Enter("-5");
            vmValueBox.Click();
            Assert.AreEqual("-5", inputBox.EditText());
            Assert.AreEqual("-5", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("0", vmValueBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
            Assert.AreEqual(true, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(false, decreaseButton.Properties.IsEnabled.Value);

            increaseButton.Click();
            Assert.AreEqual("-4", inputBox.EditText());
            Assert.AreEqual("-4", inputBox.FormattedText());
            Assert.AreEqual(true, inputBox.HasValidationError());
            Assert.AreEqual("0", vmValueBox.Text);
            Assert.AreEqual("0", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
            Assert.AreEqual(true, increaseButton.Properties.IsEnabled.Value);
            Assert.AreEqual(false, decreaseButton.Properties.IsEnabled.Value);

            increaseButton.Click();
            Assert.AreEqual("-3", inputBox.EditText());
            Assert.AreEqual("-3", inputBox.FormattedText());
            Assert.AreEqual(false, inputBox.HasValidationError());
            Assert.AreEqual("0", vmValueBox.Text);
            Assert.AreEqual("-3", inputBox.Value());
            Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
            Assert.AreEqual("Idle", inputBox.Status());
        }

        [TestCaseSource(nameof(BoxContainerIds))]
        public void Undo(string containerId)
        {
            this.view.AllowSpinnersBox.IsChecked = true;
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var vmValueBox = this.view.VmValueBox;
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

            using (Keyboard.Pressing(Key.CONTROL))
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

        [TestCaseSource(nameof(BoxContainerIds))]
        public void UndoWhenSpinUpdateModePropertyChanged(string containerId)
        {
            this.view.AllowSpinnersBox.IsChecked = true;
            var container = this.view.Window.FindByNameOrId(containerId);
            var inputBox = container.FindTextBox("InputBox");
            this.view.Window.FindComboBox("SpinUpdateMode").Select("PropertyChanged");
            inputBox.Click();
            var increaseButton = container.FindButton(SpinnerDecorator.IncreaseButtonName);
            var vmValueBox = this.view.VmValueBox;
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

            using (Keyboard.Pressing(Key.CONTROL))
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

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.view?.Dispose();
        }
    }
}