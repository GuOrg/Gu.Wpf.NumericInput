namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class CycleFocusWindowTests
    {
        private const string WindowName = "CycleFocusWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [TestCase(true)]
        [TestCase(false)]
        public static void WithSpinners(bool withSpinners)
        {
            using var application = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = application.MainWindow;
            var doubleBoxes = window.FindGroupBox("DoubleBoxes");
            var textBox = doubleBoxes.FindTextBox("TextBox1");
            textBox.Click();
            window.FindGroupBox("Settings").FindCheckBox("AllowSpinners").IsChecked = withSpinners;
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            var doubleBox3 = doubleBoxes.FindTextBox("DoubleBox3");

            textBox.Click();
            Assert.That(textBox.HasKeyboardFocus, Is.EqualTo(true));
            Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox3.HasKeyboardFocus, Is.EqualTo(false));

            Keyboard.Type(Key.TAB);
            Assert.That(textBox.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(true));
            Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox3.HasKeyboardFocus, Is.EqualTo(false));

            Keyboard.Type(Key.TAB);
            Assert.That(textBox.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(true));
            Assert.That(doubleBox3.HasKeyboardFocus, Is.EqualTo(false));

            Keyboard.Type(Key.TAB);
            Assert.That(textBox.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox3.HasKeyboardFocus, Is.EqualTo(true));

            Keyboard.Type(Key.TAB);
            window.WaitUntilResponsive();
            Assert.That(textBox.HasKeyboardFocus, Is.EqualTo(true));
            Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            Assert.That(doubleBox3.HasKeyboardFocus, Is.EqualTo(false));
        }
    }
}
