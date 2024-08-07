namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class DefaultCultureWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [Test]
        public static void OnLoad()
        {
            using var application = Application.Launch(ExeFileName, "DefaultCultureWindow");
            var window = application.MainWindow;
            var valueTextBox = window.FindTextBox("ValueTextBox");
            var spinnerDoubleBox = window.FindTextBox("SpinnerDoubleBox");
            var doubleBox = window.FindTextBox("DoubleBox");
            valueTextBox.Enter("1.234");
            Keyboard.Type(Key.TAB);
            Assert.That(spinnerDoubleBox.Text, Is.EqualTo("1.234"));
            Assert.That(doubleBox.Text, Is.EqualTo("1.234"));
        }
    }
}
