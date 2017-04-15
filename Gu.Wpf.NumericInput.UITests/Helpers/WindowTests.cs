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

    [SingleThreaded]
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
            for (var i = 0; i < 3; i++)
            {
                try
                {
                    this.itemCache.Clear();
                    this.Window?.WaitWhileBusy();
                    this.Window?.Dispose();

                    this.application?.WaitWhileBusy();
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
                this.itemCache.Clear();
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
            Assert.NotNull(name, "name != null");
            return this.GetCached<TextBox>(name);
        }

        public Button GetCachedButton([CallerMemberName]string name = null)
        {
            Assert.NotNull(name, "name != null");
            return this.GetCached<Button>(name);
        }

        public T GetCached<T>(string name)
            where T : IUIItem
        {
            Assert.NotNull(name, "name != null");
            var cached = (T)this.itemCache.GetOrAdd(name, n => this.Window.Get<T>(n));
            Assert.NotNull(cached, "cached != null");
            return cached;
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
                    this.itemCache.Clear();
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