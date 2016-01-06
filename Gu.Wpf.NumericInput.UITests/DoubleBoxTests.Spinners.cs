namespace Gu.Wpf.NumericInput.UITests
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
                    var inputBox = groupBox.Get<TextBox>(BaseBox.ValueBoxName);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    inputBox.Enter("1.23");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual("1.23", vmValueBox.Text);
                    Assert.AreEqual("1.23", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    increaseButton.Click();
                    Assert.AreEqual("2.2", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("2.23", vmValueBox.Text);
                    Assert.AreEqual("2.23", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

                    groupBox.Get<TextBox>(AutomationIds.IncrementBox).Enter("5");
                    vmValueBox.Click();
                    increaseButton.Click();
                    Assert.AreEqual("7.2", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("7.23", vmValueBox.Text);
                    Assert.AreEqual("7.23", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

                    decreaseButton.Click();
                    Assert.AreEqual("2.2", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("2.23", vmValueBox.Text);
                    Assert.AreEqual("2.23", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);
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
                    var inputBox = groupBox.Get<TextBox>(BaseBox.ValueBoxName);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    Assert.AreEqual("0", inputBox.Text);
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    increaseButton.Click();
                    //vmValueBox.Click();
                    Assert.AreEqual("3", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("3", vmValueBox.Text);
                    Assert.AreEqual("3", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

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
                    var inputBox = groupBox.Get<TextBox>(BaseBox.ValueBoxName);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    Assert.AreEqual("0", inputBox.Text);
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    decreaseButton.Click();
                    //vmValueBox.Click();
                    Assert.AreEqual("-3", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("-3", vmValueBox.Text);
                    Assert.AreEqual("-3", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

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
                    var inputBox = groupBox.Get<TextBox>(BaseBox.ValueBoxName);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    inputBox.Enter("5");
                    vmValueBox.Click();
                    Assert.AreEqual("5", inputBox.Text);
                    Assert.AreEqual(true, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

                    Assert.AreEqual(false, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);
                    decreaseButton.Click();
                    Assert.AreEqual("4", inputBox.Text);
                    Assert.AreEqual(true, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

                    Assert.AreEqual(false, increaseButton.Enabled);
                    Assert.AreEqual(true, decreaseButton.Enabled);

                    decreaseButton.Click();
                    Assert.AreEqual("3", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("3", vmValueBox.Text);
                    Assert.AreEqual("3", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);
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
                    var inputBox = groupBox.Get<TextBox>(BaseBox.ValueBoxName);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    vmValueBox.Click();
                    inputBox.Enter("-5");
                    vmValueBox.Click();
                    Assert.AreEqual("-5", inputBox.Text);
                    Assert.AreEqual(true, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);
                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(false, decreaseButton.Enabled);

                    increaseButton.Click();
                    Assert.AreEqual("-4", inputBox.Text);
                    Assert.AreEqual(true, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);
                    Assert.AreEqual(true, increaseButton.Enabled);
                    Assert.AreEqual(false, decreaseButton.Enabled);

                    increaseButton.Click();
                    Assert.AreEqual("-3", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("-3", vmValueBox.Text);
                    Assert.AreEqual("-3", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);
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
                    var inputBox = groupBox.Get<TextBox>(BaseBox.ValueBoxName);
                    var increaseButton = groupBox.Get<Button>(BaseBox.IncreaseButtonName);
                    var decreaseButton = groupBox.Get<Button>(BaseBox.DecreaseButtonName);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.Text);
                    var keyboard = window.Keyboard;
                    increaseButton.Click();
                    Assert.AreEqual("1", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("1", vmValueBox.Text);
                    Assert.AreEqual("1", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);

                    inputBox.Click();
                    keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    keyboard.Enter("z");
                    keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    Assert.AreEqual("0", inputBox.Text);
                    Assert.AreEqual(false, groupBox.Get<TextBox>(AutomationIds.InputBox).HasValidationError());
                    Assert.AreEqual("1", vmValueBox.Text);
                    Assert.AreEqual("0", groupBox.Get<Label>(AutomationIds.ValueBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput.ToString(), groupBox.Get<Label>(AutomationIds.TextSourceBlock).Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle.ToString(), groupBox.Get<Label>(AutomationIds.StatusBlock).Text);
                }
            }
        }
    }
}