namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using System.Collections.Generic;
    using System.Windows;

    using Gu.Wpf.NumericInput.Demo;
    using NUnit.Framework;
    using TestStack.White.Factory;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.TabItems;
    using TestStack.White.Utility;
    using TestStack.White.WindowsAPI;
    using Application = TestStack.White.Application;
    using CheckBox = TestStack.White.UIItems.CheckBox;
    using ComboBox = TestStack.White.UIItems.ListBoxItems.ComboBox;
    using GroupBox = TestStack.White.UIItems.GroupBox;
    using TextBox = TestStack.White.UIItems.TextBox;

    public partial class DoubleBoxTests
    {
        public class Plain
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
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                    inputBox.Enter("1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void UpdatesFromViewModel(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual(vmValueBox.Text, inputBox.Text);
                    Assert.AreEqual("0", inputBox.Text);
                    vmValueBox.Enter("1.2");
                    inputBox.Click();
                    Assert.AreEqual("1.2", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void CanBeNull(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    var canBeNullBox = groupBox.Get<CheckBox>(AutomationIds.CanBeNullBox);

                    canBeNullBox.Checked = true;
                    Assert.AreEqual("0", inputBox.Text);
                    inputBox.Enter("");
                    vmValueBox.Click();
                    Assert.AreEqual("", inputBox.EditText());
                    Assert.AreEqual("", inputBox.FormattedText());
                    Assert.AreEqual("", vmValueBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Enter("1");
                    vmValueBox.Click();
                    Assert.AreEqual("1", inputBox.EditText());
                    Assert.AreEqual("1", inputBox.FormattedText());
                    Assert.AreEqual("1", vmValueBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    canBeNullBox.Checked = false;
                    inputBox.Enter("");
                    vmValueBox.Click();
                    Assert.AreEqual("", inputBox.EditText());
                    Assert.AreEqual("", inputBox.FormattedText());
                    Assert.AreEqual("1", vmValueBox.Text);
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    canBeNullBox.Checked = true;
                    Assert.AreEqual("", inputBox.EditText());
                    Assert.AreEqual("", inputBox.FormattedText());
                    Assert.AreEqual("1", vmValueBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("", inputBox.EditText());
                    Assert.AreEqual("", inputBox.FormattedText());
                    Assert.AreEqual("", vmValueBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void Culture(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    window.Get<CheckBox>(AutomationIds.AllowThousandsBox).Checked = false;
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var cultureBox = groupBox.Get<ComboBox>(AutomationIds.CultureBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    inputBox.Enter("1.2");
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual("1.2", inputBox.Value());

                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    cultureBox.Select("sv-SE");
                    vmValueBox.Click();
                    Assert.AreEqual("1,2", inputBox.EditText());
                    Assert.AreEqual("1,2", inputBox.FormattedText());
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Enter("2.3");
                    Assert.AreEqual("2.3", inputBox.EditText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    cultureBox.Select("en-US");
                    vmValueBox.Click();
                    Assert.AreEqual("2.3", inputBox.EditText());
                    Assert.AreEqual("2.3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("2.3", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("2.3", inputBox.EditText());
                    Assert.AreEqual("2.3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("2.3", vmValueBox.Text);
                    Assert.AreEqual("2.3", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Enter("5.6a");
                    vmValueBox.Click();
                    Assert.AreEqual("5.6a", inputBox.EditText());
                    Assert.AreEqual("5.6a", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("2.3", vmValueBox.Text);
                    Assert.AreEqual("2.3", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    cultureBox.Select("sv-SE");
                    vmValueBox.Click();
                    Assert.AreEqual("5.6a", inputBox.EditText());
                    Assert.AreEqual("5.6a", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("2.3", vmValueBox.Text);
                    Assert.AreEqual("2.3", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void ValidationTriggerLostFocus(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    groupBox.Get<ComboBox>(AutomationIds.ValidationTriggerBox).Select(ValidationTrigger.LostFocus.ToString());
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                    inputBox.Enter("1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Enter("ggg");
                    Assert.AreEqual("ggg", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    vmValueBox.Click();
                    Assert.AreEqual("ggg", inputBox.EditText());
                    Assert.AreEqual("ggg", inputBox.FormattedText());
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void NumberStylesAllowLeadingSign(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var signBox = groupBox.Get<CheckBox>(AutomationIds.AllowLeadingSignBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    inputBox.Enter("-1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("-1.2", inputBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("-1.2", vmValueBox.Text);
                    Assert.AreEqual("-1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    signBox.Checked = false;
                    vmValueBox.Click();
                    Assert.AreEqual("-1.2", inputBox.Text);
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("-1.2", vmValueBox.Text);
                    Assert.AreEqual("-1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    signBox.Checked = true;
                    vmValueBox.Click();
                    Assert.AreEqual("-1.2", inputBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("-1.2", vmValueBox.Text);
                    Assert.AreEqual("-1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void UpdateDigitsWhenGreaterThanMax(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var digitsBox = groupBox.Get<TextBox>(AutomationIds.DigitsBox);
                    var maxBox = groupBox.Get<TextBox>(AutomationIds.MaxBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.Text);
                    inputBox.Enter("1.2");
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    maxBox.Enter("1");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.Text);
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());


                    digitsBox.Enter("4");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.EditText());
                    Assert.AreEqual("1.2000", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void DecimalDigits(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var digitsBox = groupBox.Get<TextBox>(AutomationIds.DigitsBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.Text);
                    digitsBox.Enter("4");
                    vmValueBox.Click();
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0.0000", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("0", vmValueBox.Text);
                    Assert.AreEqual("0", inputBox.Value());
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    vmValueBox.Enter("1.2");
                    inputBox.Click();
                    Assert.AreEqual("1.2", inputBox.Text);
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.EditText());
                    Assert.AreEqual("1.2000", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    digitsBox.Enter("0");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.EditText());
                    Assert.AreEqual("1", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    digitsBox.Enter("-3");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Enter("1.234567");
                    vmValueBox.Click();
                    Assert.AreEqual("1.234567", inputBox.EditText());
                    Assert.AreEqual("1.235", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.234567", vmValueBox.Text);
                    Assert.AreEqual("1.234567", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    digitsBox.Enter("4");
                    vmValueBox.Click();
                    Assert.AreEqual("1.234567", inputBox.EditText());
                    Assert.AreEqual("1.2346", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.234567", vmValueBox.Text);
                    Assert.AreEqual("1.234567", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void StringFormat(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var stringFormatBox = groupBox.Get<TextBox>(AutomationIds.StringFormatBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);

                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());

                    inputBox.Enter("123456.78");
                    vmValueBox.Click();
                    Assert.AreEqual("123456.78", inputBox.Text);
                    Assert.AreEqual("123456.78", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("123456.78", vmValueBox.Text);
                    Assert.AreEqual("123456.78", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    groupBox.Get<CheckBox>(AutomationIds.AllowThousandsBox).Checked = true;
                    stringFormatBox.Enter("N3");
                    vmValueBox.Click();
                    Assert.AreEqual("123456.78", inputBox.EditText());
                    Assert.AreEqual("123,456.780", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("123456.78", vmValueBox.Text);
                    Assert.AreEqual("123456.78", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Enter("2222.33333");
                    vmValueBox.Click();
                    Assert.AreEqual("2222.33333", inputBox.EditText());
                    Assert.AreEqual("2,222.333", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("2222.33333", vmValueBox.Text);
                    Assert.AreEqual("2222.33333", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    vmValueBox.Enter("4444.5555");
                    inputBox.Click();
                    Assert.AreEqual("4444.5555", inputBox.Text);
                    Assert.AreEqual("4,444.556", inputBox.FormattedText());

                    vmValueBox.Click();
                    Assert.AreEqual("4444.5555", inputBox.EditText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("4444.5555", vmValueBox.Text);
                    Assert.AreEqual("4444.5555", inputBox.Value());
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void Max(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
                    var maxBox = groupBox.Get<TextBox>(AutomationIds.MaxBox);
                    var vmValueBox = groupBox.Get<TextBox>(AutomationIds.VmValueBox);
                    Assert.AreEqual("0", inputBox.EditText());
                    Assert.AreEqual("0", inputBox.FormattedText());
                    inputBox.Enter("1.2");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    maxBox.Enter("-1");
                    vmValueBox.Click();
                    Assert.AreEqual("1.2", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Enter("2.3");
                    Assert.AreEqual("2.3", inputBox.EditText());
                    Assert.AreEqual("1.2", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());

                    vmValueBox.Click();
                    Assert.AreEqual("2.3", inputBox.EditText());
                    Assert.AreEqual("2.3", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("1.2", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    maxBox.Enter("6");
                    vmValueBox.Click();
                    Assert.AreEqual("2.3", inputBox.Text);
                    Assert.AreEqual("2.3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("1.2", vmValueBox.Text);
                    Assert.AreEqual("2.3", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    inputBox.Click();
                    vmValueBox.Click();
                    Assert.AreEqual("2.3", inputBox.Text);
                    Assert.AreEqual("2.3", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("2.3", vmValueBox.Text);
                    Assert.AreEqual("2.3", inputBox.Value());
                    Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.UserInput, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    vmValueBox.Enter("7.8");
                    inputBox.Click();
                    Assert.AreEqual("7.8", inputBox.Text);
                    Assert.AreEqual("7.8", inputBox.FormattedText());
                    Assert.AreEqual(true, inputBox.HasValidationError());
                    Assert.AreEqual("7.8", vmValueBox.Text);
                    Assert.AreEqual("7.8", inputBox.Value());
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());

                    maxBox.Enter("10");
                    vmValueBox.Click();
                    Assert.AreEqual("7.8", inputBox.Text);
                    Assert.AreEqual("7.8", inputBox.FormattedText());
                    Assert.AreEqual(false, inputBox.HasValidationError());
                    Assert.AreEqual("7.8", vmValueBox.Text);
                    Assert.AreEqual("7.8", inputBox.Value());
                     Assert.AreEqual(Gu.Wpf.NumericInput.TextSource.ValueBinding, inputBox.TextSource());
                    Assert.AreEqual(Gu.Wpf.NumericInput.Status.Idle, inputBox.Status());
                }
            }

            [TestCaseSource(nameof(BoxContainerIds))]
            public void Min(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
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

            [TestCaseSource(nameof(BoxContainerIds))]
            public void Undo(string containerId)
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache);
                    var page = window.Get<TabPage>(AutomationIds.DebugTab);
                    page.Select();
                    var groupBox = window.Get<GroupBox>(AutomationIds.DoubleBoxGroupBox);
                    var container = groupBox.Get<UIItemContainer>(containerId);
                    var inputBox = container.Get<TextBox>(AutomationIds.InputBox);
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

            private static readonly IReadOnlyList<string> BoxContainerIds = new[]
            {
                AutomationIds.VanillaGroupBox,
                AutomationIds.DataTemplateGroupBox,
                AutomationIds.ControlTemplate
            };
        }
    }
}
