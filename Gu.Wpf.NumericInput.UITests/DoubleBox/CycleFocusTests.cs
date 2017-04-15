namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.WindowItems;
    using TestStack.White.WindowsAPI;

    public sealed class CycleFocusTests : IDisposable
    {
        private readonly Application application;
        private bool disposed;

        public CycleFocusTests()
        {
            var windowName = "CycleFocusWindow";
            this.application = Application.AttachOrLaunch(Info.CreateStartInfo(windowName));
            this.application.WaitWhileBusy();
            this.Window = this.application.GetWindow(windowName);
            this.Window.WaitWhileBusy();
        }

        private Window Window { get; }

        [TestCase(true)]
        [TestCase(false)]
        public void WithSpinners(bool withSpinners)
        {
            this.Window.WaitWhileBusy();
            var doubleBoxes = this.Window.GetByText<GroupBox>("DoubleBoxes");
            var textBox = doubleBoxes.Get<TextBox>("TextBox1");
            textBox.Click();
            this.Window.GetByText<GroupBox>("Settings").Get<CheckBox>("AllowSpinners").Checked = withSpinners;
            this.Window.WaitWhileBusy();
            var doubleBox1 = doubleBoxes.Get<TextBox>("DoubleBox1");
            var doubleBox2 = doubleBoxes.Get<TextBox>("DoubleBox2");
            var doubleBox3 = doubleBoxes.Get<TextBox>("DoubleBox3");

            textBox.Click();
            Assert.AreEqual(true, textBox.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            Assert.AreEqual(false, doubleBox3.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(false, textBox.IsFocussed);
            Assert.AreEqual(true, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            Assert.AreEqual(false, doubleBox3.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(false, textBox.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(true, doubleBox2.IsFocussed);
            Assert.AreEqual(false, doubleBox3.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(false, textBox.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            Assert.AreEqual(true, doubleBox3.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(true, textBox.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            Assert.AreEqual(false, doubleBox3.IsFocussed);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.application?.Dispose();
            this.Window?.Dispose();
        }
    }
}