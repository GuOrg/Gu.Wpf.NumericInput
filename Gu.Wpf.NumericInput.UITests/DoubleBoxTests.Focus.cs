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
        public class Focus
        {
            [Test]
            public void NoSpinnersNoSuffix()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    using (var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache))
                    {
                        var page = window.Get<TabPage>(AutomationIds.FocusTab);
                        page.Select();
                        var doubleBoxes = page.Get<GroupBox>(AutomationIds.DoubleBoxes);
                        var textBoxes = page.Get<GroupBox>(AutomationIds.TextBoxes);
                        var textBox1 = doubleBoxes.Get<TextBox>(AutomationIds.TextBox1);
                        var doubleBox1 = doubleBoxes.Get<TextBox>(AutomationIds.DoubleBox1);
                        var doubleBox2 = doubleBoxes.Get<TextBox>(AutomationIds.DoubleBox2);
                        doubleBox1.Click();

                        Assert.AreEqual(false, textBox1.IsFocussed);
                        Assert.AreEqual(true, doubleBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox2.IsFocussed);
                        doubleBox1.Enter("2");
                        Assert.AreEqual("2.345", textBoxes.Get<TextBox>(AutomationIds.TextBox2).Text);

                        window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                        Assert.AreEqual("2", textBoxes.Get<TextBox>(AutomationIds.TextBox2).Text);
                        Assert.AreEqual(false, textBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox1.IsFocussed);
                        Assert.AreEqual(true, doubleBox2.IsFocussed);

                        window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                        Assert.AreEqual(true, textBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox2.IsFocussed);

                        window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                        Assert.AreEqual(false, textBox1.IsFocussed);
                        Assert.AreEqual(true, doubleBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox2.IsFocussed);
                    }
                }
            }

            [Test]
            public void WithSpinners()
            {
                using (var app = Application.AttachOrLaunch(Info.ProcessStartInfo))
                {
                    using (var window = app.GetWindow(AutomationIds.MainWindow, InitializeOption.NoCache))
                    {
                        var page = window.Get<TabPage>(AutomationIds.FocusTab);
                        page.Select();
                        var doubleBoxes = page.Get<GroupBox>(AutomationIds.DoubleBoxes);
                        var textBoxes = page.Get<GroupBox>(AutomationIds.TextBoxes);
                        var textBox1 = doubleBoxes.Get<TextBox>(AutomationIds.TextBox1);
                        var doubleBox1 = doubleBoxes.Get<TextBox>(AutomationIds.DoubleBox1);
                        var doubleBox2 = doubleBoxes.Get<TextBox>(AutomationIds.DoubleBox2);
                        page.Get<CheckBox>(AutomationIds.AllowSpinnersBox).Checked = true;
                        doubleBox1.Click();
                        Assert.AreEqual(false, textBox1.IsFocussed);
                        Assert.AreEqual(true, doubleBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox2.IsFocussed);
                        doubleBox1.Enter("2");
                        Assert.AreEqual("2.345", textBoxes.Get<TextBox>(AutomationIds.TextBox2).Text);

                        window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                        Assert.AreEqual("2", textBoxes.Get<TextBox>(AutomationIds.TextBox2).Text);
                        Assert.AreEqual(false, textBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox1.IsFocussed);
                        Assert.AreEqual(true, doubleBox2.IsFocussed);
                        doubleBox1.IncreaseButton().Click();
                        Assert.AreEqual("3", doubleBox1.EditText());
                        Assert.AreEqual("2", textBoxes.Get<TextBox>(AutomationIds.TextBox2).Text);

                        window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                        Assert.AreEqual("3", doubleBox1.EditText());
                        Assert.AreEqual("3", textBoxes.Get<TextBox>(AutomationIds.TextBox2).Text);
                        Assert.AreEqual(false, textBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox1.IsFocussed);
                        Assert.AreEqual(true, doubleBox2.IsFocussed);

                        window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
                        Assert.AreEqual(true, textBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox1.IsFocussed);
                        Assert.AreEqual(false, doubleBox2.IsFocussed);
                    }
                }
            }
        }
    }
}
