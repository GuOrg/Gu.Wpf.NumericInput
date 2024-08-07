namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class FocusWindowTests
    {
        private const string WindowName = "FocusWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [SetUp]
        public static void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("TextBox2").Enter("2.345");
            window.FindCheckBox("AllowSpinners").IsChecked = false;
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [Test]
        public static void NoSpinnersNoSuffix()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBoxes = window.FindGroupBox("DoubleBoxes");
            var textBoxes = window.FindGroupBox("TextBoxes");
            var textBox1 = doubleBoxes.FindTextBox("TextBox1");
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            doubleBox1.Click();

            Assert.Multiple(() =>
            {
                Assert.That(textBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(true));
                Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            });
            doubleBox1.Enter("2");
            Assert.That(textBoxes.FindTextBox("TextBox2").Text, Is.EqualTo("2.345"));

            Keyboard.Type(Key.TAB);
            Assert.Multiple(() =>
            {
                Assert.That(textBoxes.FindTextBox("TextBox2").Text, Is.EqualTo("2"));
                Assert.That(textBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(true));
            });

            Keyboard.Type(Key.TAB);
            Assert.Multiple(() =>
            {
                Assert.That(textBox1.HasKeyboardFocus, Is.EqualTo(true));
                Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            });

            Keyboard.Type(Key.TAB);
            window.WaitUntilResponsive();
            Assert.Multiple(() =>
            {
                Assert.That(textBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(true));
                Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            });
        }

        [Test]
        public static void WithSpinners()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            var doubleBoxes = window.FindGroupBox("DoubleBoxes");
            var textBoxes = window.FindGroupBox("TextBoxes");
            var textBox1 = doubleBoxes.FindTextBox("TextBox1");
            var doubleBox1 = doubleBoxes.FindTextBox("DoubleBox1");
            var doubleBox2 = doubleBoxes.FindTextBox("DoubleBox2");
            window.FindCheckBox("AllowSpinners").IsChecked = true;
            doubleBox1.Click();
            Assert.Multiple(() =>
            {
                Assert.That(textBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(true));
                Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            });
            doubleBox1.Enter("2");
            Assert.That(textBoxes.FindTextBox("TextBox2").Text, Is.EqualTo("2.345"));

            Keyboard.Type(Key.TAB);
            Assert.Multiple(() =>
            {
                Assert.That(textBoxes.FindTextBox("TextBox2").Text, Is.EqualTo("2"));
                Assert.That(textBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(true));
            });
            doubleBox1.IncreaseButton().Click();
            Assert.Multiple(() =>
            {
                Assert.That(doubleBox1.EditText(), Is.EqualTo("3"));
                Assert.That(textBoxes.FindTextBox("TextBox2").Text, Is.EqualTo("2"));
            });

            Keyboard.Type(Key.TAB);
            Assert.Multiple(() =>
            {
                Assert.That(doubleBox1.EditText(), Is.EqualTo("3"));
                Assert.That(textBoxes.FindTextBox("TextBox2").Text, Is.EqualTo("3"));
                Assert.That(textBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(true));
            });

            Keyboard.Type(Key.TAB);
            Assert.Multiple(() =>
            {
                Assert.That(textBox1.HasKeyboardFocus, Is.EqualTo(true));
                Assert.That(doubleBox1.HasKeyboardFocus, Is.EqualTo(false));
                Assert.That(doubleBox2.HasKeyboardFocus, Is.EqualTo(false));
            });
        }
    }
}
