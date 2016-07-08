namespace Gu.Wpf.NumericInput.UITests
{
    using NUnit.Framework;

    using TestStack.White;
    using TestStack.White.UIItems.WindowItems;

    public abstract class WindowTests
    {
        private Application application;

        protected Window Window { get; private set; }

        protected abstract string WindowName { get; }

        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            this.application = Application.AttachOrLaunch(Info.CreateStartInfo(this.WindowName));
            this.Window = this.application.GetWindow(this.WindowName);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            this.application?.Dispose();
        }
    }
}