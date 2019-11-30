namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class SelectTests
    {
        private const string WindowName = "SelectWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [SetUp]
        public static void SetUp()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindTextBox("TextBox1").ClearSelection();
            window.FindTextBox("TextBox2").ClearSelection();
            window.FindTextBox("TextBox3").ClearSelection();
            window.FindTextBox("TextBox4").ClearSelection();
            window.FindTextBox("DoubleBox1").ClearSelection();
            window.FindTextBox("DoubleBox2").ClearSelection();
            window.FindCheckBox("AllowSpinners").IsChecked = false;
            window.FindCheckBox("SelectAllOnFocus").IsChecked = false;
            window.FindCheckBox("SelectAllOnClick").IsChecked = false;
            window.FindCheckBox("SelectAllOnDoubleClick").IsChecked = false;
            window.FindCheckBox("MoveFocusOnEnter").IsChecked = false;
            window.FindCheckBox("LoseFocusOnEnter").IsChecked = false;
            window.FindTextBox("DigitsBox").Text = "1";
        }

        [OneTimeTearDown]
        public static void OneTimeTearDown()
        {
            Application.KillLaunched(ExeFileName);
        }

        [Test]
        public static void SelectAllOnFocus()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("SelectAllOnFocus").IsChecked = true;
            Assert.AreEqual(string.Empty, window.FindTextBox("TextBox1").SelectedText());
            window.FindTextBox("TextBox1").Click();
            Assert.AreEqual("1.234", window.FindTextBox("TextBox1").SelectedText());

            Assert.AreEqual(string.Empty, window.FindTextBox("TextBox2").SelectedText());
            Keyboard.Type(Key.TAB);
            Wait.UntilInputIsProcessed();
            Assert.AreEqual("2.345", window.FindTextBox("TextBox2").SelectedText());

            Assert.AreEqual(string.Empty, window.FindTextBox("TextBox4").SelectedText());
            window.FindTextBox("TextBox4").Click();
            Assert.AreEqual("1.234", window.FindTextBox("TextBox4").SelectedText());

            Keyboard.Type(Key.TAB);
            Wait.UntilInputIsProcessed();
            Assert.AreEqual("2.345", window.FindTextBox("DoubleBox1").SelectedText());

            Assert.AreEqual(string.Empty, window.FindTextBox("DoubleBox2").SelectedText());
            window.FindTextBox("DoubleBox2").Click();
            Assert.AreEqual("3.456", window.FindTextBox("DoubleBox2").SelectedText());
        }

        [Test]
        public static void SelectAllOnDoubleClick()
        {
            using var app = Application.AttachOrLaunch(ExeFileName, WindowName);
            var window = app.MainWindow;
            window.FindCheckBox("SelectAllOnDoubleClick").IsChecked = true;
            Assert.AreEqual(string.Empty, window.FindTextBox("TextBox1").SelectedText());
            window.FindTextBox("TextBox1").DoubleClick();
            Assert.AreEqual("1.234", window.FindTextBox("TextBox1").SelectedText());

            Assert.AreEqual(string.Empty, window.FindTextBox("TextBox4").SelectedText());
            window.FindTextBox("TextBox4").DoubleClick();
            Assert.AreEqual("1.234", window.FindTextBox("TextBox4").SelectedText());

            Assert.AreEqual(string.Empty, window.FindTextBox("DoubleBox1").SelectedText());
            window.FindTextBox("DoubleBox1").DoubleClick();
            Assert.AreEqual("2.345", window.FindTextBox("DoubleBox1").SelectedText());

            Assert.AreEqual(string.Empty, window.FindTextBox("DoubleBox2").SelectedText());
            window.FindTextBox("DoubleBox2").DoubleClick();
            Assert.AreEqual("3.456", window.FindTextBox("DoubleBox2").SelectedText());
        }
    }
}
