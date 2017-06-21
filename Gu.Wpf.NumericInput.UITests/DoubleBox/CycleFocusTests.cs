namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using FlaUI.Core;
    using FlaUI.Core.AutomationElements;
    using FlaUI.Core.Definitions;
    using FlaUI.Core.Input;
    using FlaUI.Core.WindowsAPI;
    using FlaUI.UIA3;
    using NUnit.Framework;

    public sealed class CycleFocusTests : IDisposable
    {
        private readonly Application application;
        private readonly UIA3Automation automation;

        private bool disposed;

        public CycleFocusTests()
        {
            this.application = Application.Launch(Info.CreateStartInfo("CycleFocusWindow"));
            this.automation = new UIA3Automation();
            this.Window = this.application.GetMainWindow(this.automation);
        }

        private Window Window { get; }

        [TestCase(true)]
        [TestCase(false)]
        public void WithSpinners(bool withSpinners)
        {
            var doubleBoxes = this.Window.FindGroupBox("DoubleBoxes");
            var textBox = doubleBoxes.FindTextBox("TextBox1");
            textBox.Click();
            this.Window.FindGroupBox("Settings").FindCheckBox("AllowSpinners").State = withSpinners ? ToggleState.On : ToggleState.Off;
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            var doubleBox3 = doubleBoxes.FindTextBox("DoubleBox3");

            textBox.Click();
            Assert.AreEqual(true, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.Properties.HasKeyboardFocus);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual(false, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.Properties.HasKeyboardFocus);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual(false, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.Properties.HasKeyboardFocus);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual(false, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox3.Properties.HasKeyboardFocus);

            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual(true, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.Properties.HasKeyboardFocus);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.application?.Dispose();
            this.automation.Dispose();
        }
    }
}