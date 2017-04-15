namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.WindowItems;
    using TestStack.White.WindowsAPI;

    public sealed class FocusWindowTests : IDisposable
    {
        private readonly Application application;
        private bool disposed;

        public FocusWindowTests()
        {
            var windowName = "FocusWindow";
            this.application = Application.Launch(Info.CreateStartInfo(windowName));
            this.application.WaitWhileBusy();
            this.Window = this.application.GetWindow(windowName);
            this.Window.WaitWhileBusy();
        }

        private Window Window { get; }

        [SetUp]
        public void SetUp()
        {
            this.Window.Get<TextBox>("TextBox2").Enter("2.345");
            this.Window.Get<CheckBox>("AllowSpinnersBox").Checked = false;
        }

        [Test]
        public void NoSpinnersNoSuffix()
        {
            var doubleBoxes = this.Window.Get<GroupBox>("DoubleBoxes");
            var textBoxes = this.Window.Get<GroupBox>("TextBoxes");
            var textBox1 = doubleBoxes.Get<TextBox>("TextBox1");
            var doubleBox1 = doubleBoxes.Get<TextBox>("DoubleBox1");
            var doubleBox2 = doubleBoxes.Get<TextBox>("DoubleBox2");
            doubleBox1.Click();

            Assert.AreEqual(false, textBox1.IsFocussed);
            Assert.AreEqual(true, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            doubleBox1.Enter("2");
            Assert.AreEqual("2.345", textBoxes.Get<TextBox>("TextBox2").Text);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual("2", textBoxes.Get<TextBox>("TextBox2").Text);
            Assert.AreEqual(false, textBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(true, doubleBox2.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(true, textBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(false, textBox1.IsFocussed);
            Assert.AreEqual(true, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
        }

        [Test]
        public void WithSpinners()
        {
            var doubleBoxes = this.Window.Get<GroupBox>("DoubleBoxes");
            var textBoxes = this.Window.Get<GroupBox>("TextBoxes");
            var textBox1 = doubleBoxes.Get<TextBox>("TextBox1");
            var doubleBox1 = doubleBoxes.Get<TextBox>("DoubleBox1");
            var doubleBox2 = doubleBoxes.Get<TextBox>("DoubleBox2");
            this.Window.Get<CheckBox>("AllowSpinnersBox").Checked = true;
            doubleBox1.Click();
            Assert.AreEqual(false, textBox1.IsFocussed);
            Assert.AreEqual(true, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
            doubleBox1.Enter("2");
            Assert.AreEqual("2.345", textBoxes.Get<TextBox>("TextBox2").Text);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual("2", textBoxes.Get<TextBox>("TextBox2").Text);
            Assert.AreEqual(false, textBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(true, doubleBox2.IsFocussed);
            doubleBox1.IncreaseButton().Click();
            Assert.AreEqual("3", doubleBox1.EditText());
            Assert.AreEqual("2", textBoxes.Get<TextBox>("TextBox2").Text);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual("3", doubleBox1.EditText());
            Assert.AreEqual("3", textBoxes.Get<TextBox>("TextBox2").Text);
            Assert.AreEqual(false, textBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(true, doubleBox2.IsFocussed);

            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual(true, textBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox1.IsFocussed);
            Assert.AreEqual(false, doubleBox2.IsFocussed);
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
