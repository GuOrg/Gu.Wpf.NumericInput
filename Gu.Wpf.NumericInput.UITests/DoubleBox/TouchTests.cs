namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public static class TouchTests
    {
        private const string WindowName = "TouchWindow";
        private const string ExeFileName = "Gu.Wpf.NumericInput.Demo.exe";

        [Test]
        public static void Tap()
        {
            using var app = Application.Launch(ExeFileName, WindowName);
            var window = app.MainWindow;
            Touch.Tap(window.FindTextBox("TextBox1").GetClickablePoint());
            Wait.UntilInputIsProcessed();

            Touch.Tap(window.FindTextBox("TextBox2").GetClickablePoint());
            Wait.UntilInputIsProcessed();
        }
    }
}
