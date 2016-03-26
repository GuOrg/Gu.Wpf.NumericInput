namespace Gu.Wpf.NumericInput.UITests
{
    using System.Collections.Generic;
    using Gu.Wpf.NumericInput.Demo;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;
    using TestStack.White.WindowsAPI;

    public partial class DoubleBoxTests
    {
        public class Spinners
        {
            [TestCaseSource(nameof(BoxContainerIds))]
            public void UpdatesViewModel(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    groupBox.Get<TextBox>(AutomationIds.DigitsBox).Enter("1");
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = container.Get<Button>(SpinnerDecorator.IncreaseButtonName);
                    var decreaseButton = container.Get<Button>(SpinnerDecorator.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    inputBox.Enter("1.23");
                    vmValueBox.Click();
                    Assert.AreEqual("1.23", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual("1.23", vmValueBox.Text);
                    Assert.AreEqual("1.23", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
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
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    groupBox.Get<TextBox>(AutomationIds.IncrementBox).Enter("5");
                    vmValueBox.Click();
                    increaseButton.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("7.23", inputBox.EditText());
                    Assert.AreEqual("7.2", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("7.23", vmValueBox.Text);
                    Assert.AreEqual("7.23", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    decreaseButton.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("2.23", inputBox.EditText());
                    Assert.AreEqual("2.2", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("2.23", vmValueBox.Text);
                    Assert.AreEqual("2.23", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void TruncatesToMax(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    groupBox.Get<TextBox>(AutomationIds.IncrementBox).Enter("5");
                    groupBox.Get<TextBox>(AutomationIds.MaxBox).Enter("3");
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = container.Get<Button>(SpinnerDecorator.IncreaseButtonName);
                    var decreaseButton = container.Get<Button>(SpinnerDecorator.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                    Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    increaseButton.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("3", inputBox.EditText());
                    Assert.AreEqual("3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("3", vmValueBox.Text);
                    Assert.AreEqual("3", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    Assert.AreEqual(false, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void TruncatesToMin(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    groupBox.Get<TextBox>(AutomationIds.IncrementBox).Enter("5");
                    groupBox.Get<TextBox>(AutomationIds.MinBox).Enter("-3");
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = container.Get<Button>(SpinnerDecorator.IncreaseButtonName);
                    var decreaseButton = container.Get<Button>(SpinnerDecorator.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                    Assert.AreEqual(TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    decreaseButton.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("-3", inputBox.EditText());
                    Assert.AreEqual("-3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("-3", vmValueBox.Text);
                    Assert.AreEqual("-3", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(false, decreaseButton.Enabled);
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void DecreasesWhenGreaterThanMax(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    groupBox.Get<TextBox>(AutomationIds.MaxBox).Enter("3");
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = container.Get<Button>(SpinnerDecorator.IncreaseButtonName);
                    var decreaseButton = container.Get<Button>(SpinnerDecorator.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    inputBox.Enter("5");
                    vmValueBox.Click();
                    Assert.AreEqual("5", inputBox.EditText());
                    Assert.AreEqual("5", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    Assert.AreEqual(false, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    decreaseButton.Click();
                    Assert.AreEqual("4", inputBox.EditText());
                    Assert.AreEqual("4", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    Assert.AreEqual(false, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);

                    decreaseButton.Click();
                    Assert.AreEqual("3", inputBox.EditText());
                    Assert.AreEqual("3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("3", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void IncreasesWhenLessThanMin(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    groupBox.Get<TextBox>(AutomationIds.MinBox).Enter("-3");
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = container.Get<Button>(SpinnerDecorator.IncreaseButtonName);
                    var decreaseButton = container.Get<Button>(SpinnerDecorator.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    inputBox.Enter("-5");
                    vmValueBox.Click();
                    Assert.AreEqual("-5", inputBox.EditText());
                    Assert.AreEqual("-5", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());
                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(false, decreaseButton.Enabled);

                    increaseButton.Click();
                    Assert.AreEqual("-4", inputBox.EditText());
                    Assert.AreEqual("-4", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());
                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(false, decreaseButton.Enabled);

                    increaseButton.Click();
                    Assert.AreEqual("-3", inputBox.EditText());
                    Assert.AreEqual("-3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("-3", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void Undo(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = container.Get<Button>(SpinnerDecorator.IncreaseButtonName);
                    //var decreaseButton = container.Get<Button>(SpinnerDecorator.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    increaseButton.Click();
                    Assert.AreEqual("1", inputBox.EditText());
                    Assert.AreEqual("1", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("1", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());

                    inputBox.Click();
                    var keyboard = window.Keyboard;
                    keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    keyboard.Enter("z");
                    keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    vmValueBox.Click();
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                    Assert.AreEqual(TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Status.Idle, inputBox.Status());
                }
            }

            private static readonly IReadOnlyList<string> BoxContainerIds = new[]
            {
                AutomationIds.VanillaGroupBox,
                AutomationIds.DataTemplateGroupBox,
                AutomationIds.ControlTemplate
            };
        }
    }
}