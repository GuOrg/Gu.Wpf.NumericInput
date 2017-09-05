namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using Gu.Wpf.UiAutomation;
    using NUnit.Framework;

    public sealed class DefaultCultureWindowTests : IDisposable
    {
        private readonly Application application;

        private bool disposed;

        public DefaultCultureWindowTests()
        {
            var windowName = "DefaultCultureWindow";
            this.application = Application.Launch(Info.CreateStartInfo(windowName));
            this.Window = this.application.MainWindow;
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
            Keyboard.Type(Key.TAB);
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
        }
    }
}