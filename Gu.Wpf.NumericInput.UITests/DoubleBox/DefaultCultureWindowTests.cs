namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public class DefaultCultureWindowTests
    {
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [Test]
        public void OnLoad()
        {
            using (var application = Application.Launch(ExeFileName, "DefaultCultureWindow"))
            {
                var window = application.MainWindow;
                var valueTextBox = window.FindTextBox("ValueTextBox");
                var spinnerDoubleBox = window.FindTextBox("SpinnerDoubleBox");
                var doubleBox = window.FindTextBox("DoubleBox");
                valueTextBox.Enter("1.234");
                Keyboard.Type(Key.TAB);
                Assert.AreEqual("1.234", spinnerDoubleBox.Text);
                Assert.AreEqual("1.234", doubleBox.Text);
            }
        }
    }
}