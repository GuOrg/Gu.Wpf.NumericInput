namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.WindowItems;
    using TestStack.White.WindowsAPI;

    public sealed class DefaultCultureWindowTests : IDisposable
    {
        private readonly Application application;
        private bool disposed;

        public DefaultCultureWindowTests()
        {
            var windowName = "DefaultCultureWindow";
            this.application = Application.Launch(Info.CreateStartInfo(windowName));
            this.Window = this.application.GetWindow(windowName);
            this.ValueTextBox = this.Window.Get<TextBox>(nameof(this.ValueTextBox));
            this.SpinnerDoubleBox = this.Window.Get<TextBox>(nameof(this.SpinnerDoubleBox));
            this.DoubleBox = this.Window.Get<TextBox>(nameof(this.DoubleBox));
        }

        private Window Window { get; }

        private TextBox ValueTextBox { get; }

        private TextBox SpinnerDoubleBox { get; }

        private TextBox DoubleBox { get; }

        [Test]
        public void OnLoad()
        {
            this.ValueTextBox.BulkText = "1.234";
            this.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
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
            this.Window?.Dispose();
        }
    }
}