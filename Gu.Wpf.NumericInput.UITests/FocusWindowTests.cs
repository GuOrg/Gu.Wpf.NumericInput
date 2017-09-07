namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class FocusWindowTests : IDisposable
    {
        private readonly Application application;

        private bool disposed;

        public FocusWindowTests()
        {
            this.application = Application.Launch(Info.CreateStartInfo("FocusWindow"));
            this.Window = this.application.MainWindow;
        }

        private Window Window { get; }

        [SetUp]
        public void SetUp()
        {
            this.Window.FindTextBox("TextBox2").Enter("2.345");
            this.Window.FindCheckBox("AllowSpinnersBox").IsChecked = false;
        }

        [Test]
        public void NoSpinnersNoSuffix()
        {
            var doubleBoxes = this.Window.FindGroupBox("DoubleBoxes");
            var textBoxes = this.Window.FindGroupBox("TextBoxes");
            var textBox1 = doubleBoxes.FindTextBox("TextBox1");
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            doubleBox1.Click();

            Assert.AreEqual(false, textBox1.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
            doubleBox1.Enter("2");
            Assert.AreEqual("2.345", textBoxes.FindTextBox("TextBox2").Text);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual("2", textBoxes.FindTextBox("TextBox2").Text);
            Assert.AreEqual(false, textBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual(true, textBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            this.Window.WaitUntilResponsive();
            Assert.AreEqual(false, textBox1.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
        }

        [Test]
        public void WithSpinners()
        {
            var doubleBoxes = this.Window.FindGroupBox("DoubleBoxes");
            var textBoxes = this.Window.FindGroupBox("TextBoxes");
            var textBox1 = doubleBoxes.FindTextBox("TextBox1");
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            this.Window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
            doubleBox1.Click();
            Assert.AreEqual(false, textBox1.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
            doubleBox1.Enter("2");
            Assert.AreEqual("2.345", textBoxes.FindTextBox("TextBox2").Text);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual("2", textBoxes.FindTextBox("TextBox2").Text);
            Assert.AreEqual(false, textBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.HasKeyboardFocus);
            doubleBox1.IncreaseButton().Click();
            Assert.AreEqual("3", doubleBox1.EditText());
            Assert.AreEqual("2", textBoxes.FindTextBox("TextBox2").Text);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual("3", doubleBox1.EditText());
            Assert.AreEqual("3", textBoxes.FindTextBox("TextBox2").Text);
            Assert.AreEqual(false, textBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual(true, textBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.application?.Dispose();
        }
    }
}
