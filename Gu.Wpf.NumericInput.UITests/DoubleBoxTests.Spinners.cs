﻿namespace Gu.Wpf.NumericInput.UITests
{
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
            [Test]
            public void UpdatesViewModel()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    groupBox.Get<TextBox>(AutomationIds.DigitsBox).Enter("1");
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    inputBox.Enter("1.23");
                    vmValueBox.Click();
                    Assert.AreEqual("1.23", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual("1.23", vmValueBox.Text);
                    Assert.AreEqual("1.23", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

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
                    Assert.AreEqual("2.23", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

                    groupBox.Get<TextBox>(AutomationIds.IncrementBox).Enter("5");
                    vmValueBox.Click();
                    increaseButton.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("7.23", inputBox.EditText());
                    Assert.AreEqual("7.2", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("7.23", vmValueBox.Text);
                    Assert.AreEqual("7.23", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

                    decreaseButton.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("2.23", inputBox.EditText());
                    Assert.AreEqual("2.2", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("2.23", vmValueBox.Text);
                    Assert.AreEqual("2.23", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());
                }
            }

            [Test]
            public void TruncatesToMax()
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
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    increaseButton.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("3", inputBox.EditText());
                    Assert.AreEqual("3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("3", vmValueBox.Text);
                    Assert.AreEqual("3", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

                    Assert.AreEqual(false, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                }
            }

            [Test]
            public void TruncatesToMin()
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
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    decreaseButton.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("-3", inputBox.EditText());
                    Assert.AreEqual("-3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("-3", vmValueBox.Text);
                    Assert.AreEqual("-3", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(false, decreaseButton.Enabled);
                }
            }

            [Test]
            public void DecreasesWhenGreaterThanMax()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    groupBox.Get<TextBox>(AutomationIds.MaxBox).Enter("3");
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    inputBox.Enter("5");
                    vmValueBox.Click();
                    Assert.AreEqual("5", inputBox.EditText());
                    Assert.AreEqual("5", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

                    Assert.AreEqual(false, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    decreaseButton.Click();
                    Assert.AreEqual("4", inputBox.EditText());
                    Assert.AreEqual("4", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

                    Assert.AreEqual(false, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);

                    decreaseButton.Click();
                    Assert.AreEqual("3", inputBox.EditText());
                    Assert.AreEqual("3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("3", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());
                }
            }

            [Test]
            public void IncreasesWhenLessThanMin()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    groupBox.Get<TextBox>(AutomationIds.MinBox).Enter("-3");
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    inputBox.Enter("-5");
                    vmValueBox.Click();
                    Assert.AreEqual("-5", inputBox.EditText());
                    Assert.AreEqual("-5", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());
                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(false, decreaseButton.Enabled);

                    increaseButton.Click();
                    Assert.AreEqual("-4", inputBox.EditText());
                    Assert.AreEqual("-4", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());
                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(false, decreaseButton.Enabled);

                    increaseButton.Click();
                    Assert.AreEqual("-3", inputBox.EditText());
                    Assert.AreEqual("-3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("-3", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());
                }
            }

            [Test]
            public void Undo()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    groupBox.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    increaseButton.Click();
                    Assert.AreEqual("1", inputBox.EditText());
                    Assert.AreEqual("1", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("1", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());

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
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, groupBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, groupBox.Status());
                }
            }
        }
    }
}