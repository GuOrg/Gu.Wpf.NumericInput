namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using System.Windows;
    using Gu.Wpf.NumericInput.Demo;
    using NUnit.Framework;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.ListBoxItems;
    using TestStack.White.UIItems.TabItems;
    using TestStack.White.Utility;
    using TestStack.White.WindowsAPI;
    using Application = TestStack.White.Application;

    public class DoubleBoxTests
    {
        public class Plain
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
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreNotEqual("1.2", inputBox.Text);
                    inputBox.Enter("1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", vmValueBox.Text);
                }
            }

            [Test]
            public void UpdatesFromViewModel()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual(vmValueBox.Text, inputBox.Text);
                    Assert.AreNotEqual("1.2", inputBox.Text);
                    vmValueBox.Enter("1.2");
                    inputBox.Click();
                    Assert.AreEqual("1.2", inputBox.Text);
                }
            }

            [Test]
            public void Culture()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var cultureBox = groupBox.Get<ComboBox>(AutomationIds.CultureBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreNotEqual("1.2", inputBox.Text);
                    inputBox.Enter("1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    cultureBox.Select("sv-SE");
                    vmValueBox.Click();
                    Assert.AreEqual("1,2", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    inputBox.Enter("2.3");
                    Assert.AreEqual("2.3", inputBox.Text);
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    cultureBox.Select("en-US");
                    vmValueBox.Click();
                    Assert.AreEqual("2.3", inputBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text); // maybe we want to update source here idk.
                }
            }

            [Test]
            public void NumberStylesAllowLeadingSign()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var signBox = groupBox.Get<CheckBox>(AutomationIds.AllowLeadingSignBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    inputBox.Enter("-1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("-1.2", inputBox.Text);
                    Assert.AreEqual("-1.2", vmValueBox.Text);

                    signBox.Checked = false;
                    vmValueBox.Click();
                    Assert.AreEqual("-1.2", inputBox.Text);
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("-1.2", vmValueBox.Text);

                    signBox.Checked = true;
                    vmValueBox.Click();
                    Assert.AreEqual("-1.2", inputBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("-1.2", vmValueBox.Text);
                }
            }

            [Test]
            public void DecimalDigits()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var digitsBox = groupBox.Get<TextBox>(AutomationIds.DigitsBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    digitsBox.Enter("4");
                    Assert.AreNotEqual("1.2", inputBox.Text);
                    inputBox.Enter("1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2000", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    digitsBox.Enter("0");
                    vmValueBox.Click();
                    Assert.AreEqual("1", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    digitsBox.Enter("-3");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    inputBox.Enter("1.234567");
                    vmValueBox.Click();
                    Assert.AreEqual("1.235", inputBox.Text);
                    Assert.AreEqual("1.234567", vmValueBox.Text);

                    digitsBox.Enter("4");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2346", inputBox.Text);
                    Assert.AreEqual("1.234567", vmValueBox.Text);
                }
            }

            [Test]
            public void Max()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var maxBox = groupBox.Get<TextBox>(AutomationIds.MaxBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreNotEqual("1.2", inputBox.Text);
                    inputBox.Enter("1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    maxBox.Enter("-1");
                    vmValueBox.Click();
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    inputBox.Enter("2.3");
                    Assert.AreEqual("2.3", inputBox.Text);
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    maxBox.Enter("6");
                    vmValueBox.Click();
                    Assert.AreEqual("2.3", inputBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text); // maybe we want to update source here idk.

                    inputBox.Enter("5.6");
                    vmValueBox.Click();
                    Assert.AreEqual("5.6", inputBox.Text);
                    Assert.AreEqual("5.6", vmValueBox.Text);
                }
            }

            [Test]
            public void Min()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var minBox = groupBox.Get<TextBox>(AutomationIds.MinBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreNotEqual("1.2", inputBox.Text);
                    inputBox.Enter("1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    minBox.Enter("4");
                    vmValueBox.Click();
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    inputBox.Enter("2.3");
                    Assert.AreEqual("2.3", inputBox.Text);
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);

                    minBox.Enter("-2");
                    vmValueBox.Click();
                    Assert.AreEqual("2.3", inputBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text); // maybe we want to update source here idk.

                    inputBox.Enter("5.6");
                    vmValueBox.Click();
                    Assert.AreEqual("5.6", inputBox.Text);
                    Assert.AreEqual("5.6", vmValueBox.Text);
                }
            }

            [Test]
            public void Focus()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.FocusTab);
                    page.Select();
                    var doubleBox1 = page.Get<TextBox>(AutomationIds.DoubleBox1);
                    var doubleBox2 = page.Get<TextBox>(AutomationIds.DoubleBox2);
                    var doubleBox3 = page.Get<TextBox>(AutomationIds.DoubleBox3);
                    doubleBox1.Click();
                    Assert.AreEqual(true, doubleBox1.IsFocussed);
                    Assert.AreEqual(false, doubleBox2.IsFocussed);
                    Assert.AreEqual(false, doubleBox3.IsFocussed);

                    window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                    Assert.AreEqual(false, doubleBox1.IsFocussed);
                    Assert.AreEqual(true, doubleBox2.IsFocussed);
                    Assert.AreEqual(false, doubleBox3.IsFocussed);

                    window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                    Assert.AreEqual(false, doubleBox1.IsFocussed);
                    Assert.AreEqual(false, doubleBox2.IsFocussed);
                    Assert.AreEqual(true, doubleBox3.IsFocussed);

                    window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                    Assert.AreEqual(true, doubleBox1.IsFocussed);
                    Assert.AreEqual(false, doubleBox2.IsFocussed);
                    Assert.AreEqual(false, doubleBox3.IsFocussed);
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
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.Text);
                    var keyboard = window.Keyboard;
                    inputBox.Click();
                    keyboard.Enter("1");
                    Assert.AreEqual("10", inputBox.Text);
                    keyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    keyboard.Enter("z");
                    keyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);
                    Assert.AreEqual("0", inputBox.Text);
                    Assert.AreEqual("0", vmValueBox.Text);
                }
            }

            [Test, RequiresSTA]
            public void CopyTest()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var inputBox = groupBox.Get<TextBox>(AutomationIds.InputBox);
                    var attachedKeyboard = window.Keyboard;
                    inputBox.Text = "1.2";
                    attachedKeyboard.HoldKey(KeyboardInput.SpecialKeys.CONTROL);
                    attachedKeyboard.Enter("ac");
                    attachedKeyboard.LeaveKey(KeyboardInput.SpecialKeys.CONTROL);

                    //check the text is the same as that on the clipboard
                    Retry.For(() =>
                    {
                        var clipboardText = Clipboard.GetText();
                        Assert.AreEqual("1.2", clipboardText);
                    }, TimeSpan.FromSeconds(5));
                }
            }
        }
    }
}
