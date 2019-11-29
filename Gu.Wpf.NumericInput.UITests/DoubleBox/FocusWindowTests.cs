namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class FocusWindowTests
    {
        private const string WindowName = "FocusWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [SetUp]
        public void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("TextBox2").Enter("2.345");
            window.FindCheckBox("AllowSpinnersBox").IsChecked = false;
            window.WaitUntilResponsive();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [Test]
        public void NoSpinnersNoSuffix()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBoxes = window.FindGroupBox("DoubleBoxes");
            var textBoxes = window.FindGroupBox("TextBoxes");
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
            window.WaitUntilResponsive();
            Assert.AreEqual(false, textBox1.HasKeyboardFocus);
            Assert.AreEqual(true, doubleBox1.HasKeyboardFocus);
            Assert.AreEqual(false, doubleBox2.HasKeyboardFocus);
        }

        [Test]
        public void WithSpinners()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBoxes = window.FindGroupBox("DoubleBoxes");
            var textBoxes = window.FindGroupBox("TextBoxes");
            var textBox1 = doubleBoxes.FindTextBox("TextBox1");
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            window.FindCheckBox("AllowSpinnersBox").IsChecked = true;
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
    }
}
