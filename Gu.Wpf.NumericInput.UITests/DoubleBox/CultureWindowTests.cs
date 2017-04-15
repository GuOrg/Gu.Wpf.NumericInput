namespace Gu.Wpf.NumericInput.UITests.DoubleBox
{
    using System;
    using NUnit.Framework;
    using TestStack.White;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.WindowItems;
    using TestStack.White.WindowsAPI;

    public sealed class CultureWindowTests : IDisposable
    {
        private readonly CultureView view;
        private bool disposed;

        public CultureWindowTests()
        {
            this.view = new CultureView();
        }

        [Test]
        public void TestCultures()
        {
            this.view.ValueTextBox.BulkText = "1.234";
            this.view.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual("1,234", this.view.SpinnerDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.InheritingCultureDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.SvSeDoubleBox.Text);
            Assert.AreEqual("1.234", this.view.EnUsDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.BoundCultureDoubleBox.Text);

            this.view.CultureTextBox.Text = "en-us";
            this.view.Window.Keyboard.PressSpecialKey(KeyboardInput.SpecialKeys.TAB);
            Assert.AreEqual("1,234", this.view.SpinnerDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.InheritingCultureDoubleBox.Text);
            Assert.AreEqual("1,234", this.view.SvSeDoubleBox.Text);
            Assert.AreEqual("1.234", this.view.EnUsDoubleBox.Text);
            Assert.AreEqual("1.234", this.view.BoundCultureDoubleBox.Text);
        }

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.view.Dispose();
        }

        public sealed class CultureView : IDisposable
        {
            private readonly Application application;
            private bool disposed;

            public CultureView()
            {
                var windowName = "CultureWindow";
                this.application = Application.Launch(Info.CreateStartInfo(windowName));
                this.application.WaitWhileBusy();
                this.Window = this.application.GetWindow(windowName);
                this.Window.WaitWhileBusy();
                this.ValueTextBox = this.Window.Get<TextBox>(nameof(this.ValueTextBox));
                this.SpinnerDoubleBox = this.Window.Get<TextBox>(nameof(this.SpinnerDoubleBox));
                this.InheritingCultureDoubleBox = this.Window.Get<TextBox>(nameof(this.InheritingCultureDoubleBox));
                this.SvSeDoubleBox = this.Window.Get<TextBox>(nameof(this.SvSeDoubleBox));
                this.EnUsDoubleBox = this.Window.Get<TextBox>(nameof(this.EnUsDoubleBox));
                this.BoundCultureDoubleBox = this.Window.Get<TextBox>(nameof(this.BoundCultureDoubleBox));
                this.CultureTextBox = this.Window.Get<TextBox>(nameof(this.CultureTextBox));
            }

            public Window Window { get; }

            public TextBox ValueTextBox { get; }

            public TextBox SpinnerDoubleBox { get; }

            public TextBox InheritingCultureDoubleBox { get; }

            public TextBox SvSeDoubleBox { get; }

            public TextBox EnUsDoubleBox { get; }

            public TextBox BoundCultureDoubleBox { get; }

            public TextBox CultureTextBox { get; }

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
}