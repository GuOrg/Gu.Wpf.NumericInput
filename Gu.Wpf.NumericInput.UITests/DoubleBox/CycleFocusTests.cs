namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class CycleFocusTests : IDisposable
    {
        private readonly Application application;

        private bool disposed;

        public CycleFocusTests()
        {
            this.application = Application.Launch(Info.CreateStartInfo("CycleFocusWindow"));
            this.Window = this.application.MainWindow;
        }

        private Window Window { get; }

        [TestCase(true)]
        [TestCase(false)]
        public void WithSpinners(bool withSpinners)
        {
            var doubleBoxes = this.Window.FindGroupBox("DoubleBoxes");
            var textBox = doubleBoxes.FindTextBox("TextBox1");
            textBox.Click();
            this.Window.FindGroupBox("Settings").FindCheckBox("AllowSpinners").IsChecked = withSpinners;
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            var doubleBox3 = doubleBoxes.FindTextBox("DoubleBox3");

            textBox.Click();
            Assert.AreEqual(true, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.Properties.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual(false, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.Properties.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual(false, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox3.Properties.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
            Assert.AreEqual(false, textBox.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox1.Properties.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.Properties.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox3.Properties.HasKeyboardFocus);

            Keyboard.Type(Key.TAB);
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
        }
    }
}