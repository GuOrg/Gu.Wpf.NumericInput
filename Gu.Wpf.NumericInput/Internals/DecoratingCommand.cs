namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows.Input;

    internal sealed class DecoratingCommand : ICommand, IDisposable
    {
        private readonly ICommand inner;
        private readonly Action<object> before;
        private bool disposed;

        public DecoratingCommand(ICommand inner, Action<object> before)
        {
            this.inner = inner;
            this.inner.CanExecuteChanged += this.InnerOnCanExecuteChanged;
            this.before = before;
        }

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter) => this.inner.CanExecute(parameter);

        public void Execute(object parameter)
        {
            this.before(parameter);
            this.inner.Execute(parameter);
        }

        /// <summary>
        /// Make the class sealed when using this.
        /// Call VerifyDisposed at the start of all public methods
        /// </summary>
        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            this.inner.CanExecuteChanged -= this.InnerOnCanExecuteChanged;
        }

        private void VerifyDisposed()
        {
            if (this.disposed)
            {
                throw new ObjectDisposedException(this.GetType().FullName);
            }
        }

        private void InnerOnCanExecuteChanged(object sender, EventArgs eventArgs)
        {
            this.OnCanExecuteChanged();
        }

        private void OnCanExecuteChanged()
        {
            this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
