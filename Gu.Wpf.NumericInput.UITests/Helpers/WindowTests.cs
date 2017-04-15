// ReSharper disable UnusedMember.Global
namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using System.Collections.Concurrent;
    using System.Runtime.CompilerServices;
    using NUnit.Framework;

    using TestStack.White;
    using TestStack.White.UIItems;
    using TestStack.White.UIItems.WindowItems;

    public abstract class WindowTests : IDisposable
    {
        private readonly ConcurrentDictionary<string, IUIItem> itemCache = new ConcurrentDictionary<string, IUIItem>();
        private Application application;
        private bool disposed;

        protected Window Window { get; private set; }

        protected abstract string WindowName { get; }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    this.application?.WaitWhileBusy();
                    this.Window?.Dispose();
                    this.application?.Dispose();

                    this.application = Application.AttachOrLaunch(Info.CreateStartInfo(this.WindowName));
                    this.application.WaitWhileBusy();
                    this.Window = this.application.GetWindow(this.WindowName);
                    this.Window.WaitWhileBusy();
                    return;
                }
                catch
                {
                    // We get this on AppVeyor.
                    // Testing a retry strategy :)
                }
            }
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            try
            {
                this.application?.WaitWhileBusy();
                this.Window?.Dispose();
                this.application?.Dispose();
            }
            catch
            {
                // We get this on AppVeyor.
            }
        }

        public TextBox GetCachedTextBox([CallerMemberName]string name = null)
        {
            return this.GetCached<TextBox>(name);
        }

        public Button GetCachedButton([CallerMemberName]string name = null)
        {
            Assert.NotNull(name);
            return this.GetCached<Button>(name);
        }

        public T GetCached<T>(string name)
            where T : IUIItem
        {
            return (T)this.itemCache.GetOrAdd(name, n => this.Window.Get<T>(n));
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            if (disposing)
            {
                try
                {
                    this.application?.WaitWhileBusy();
                    this.Window?.Dispose();
                    this.application?.Dispose();
                }
                catch
                {
                    // We get this on AppVeyor.
                }
            }
        }
    }
}