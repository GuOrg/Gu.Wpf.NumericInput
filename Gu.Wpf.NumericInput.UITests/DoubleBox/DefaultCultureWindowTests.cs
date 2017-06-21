namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using FlaUI.Core;
    using FlaUI.Core.AutomationElements;
    using FlaUI.Core.Input;
    using FlaUI.Core.WindowsAPI;
    using FlaUI.UIA3;
    using NUnit.Framework;

    public sealed class DefaultCultureWindowTests : IDisposable
    {
        private readonly Application application;
        private readonly UIA3Automation automation;

        private bool disposed;

        public DefaultCultureWindowTests()
        {
            var windowName = "DefaultCultureWindow";
            this.application = Application.Launch(Info.CreateStartInfo(windowName));
            this.automation = new UIA3Automation();
            this.Window = this.application.GetMainWindow(this.automation);
            this.ValueTextBox = this.Window.FindTextBox(nameof(this.ValueTextBox));
            this.SpinnerDoubleBox = this.Window.FindTextBox(nameof(this.SpinnerDoubleBox));
            this.DoubleBox = this.Window.FindTextBox(nameof(this.DoubleBox));
        }

        private Window Window { get; }

        private TextBox ValueTextBox { get; }

        private TextBox SpinnerDoubleBox { get; }

        private TextBox DoubleBox { get; }

        [Test]
        public void OnLoad()
        {
            this.ValueTextBox.Enter("1.234");
            Keyboard.Press(VirtualKeyShort.TAB);
            Assert.AreEqual("1.234", this.SpinnerDoubleBox.Text);
            Assert.AreEqual("1.234", this.DoubleBox.Text);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.application?.Dispose();
            this.automation.Dispose();
        }
    }
}