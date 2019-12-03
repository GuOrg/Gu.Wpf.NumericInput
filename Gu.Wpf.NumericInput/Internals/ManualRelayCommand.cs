namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    internal class ManualRelayCommand : ICommand
    {
        private readonly Action<object> action;
        private readonly Func<object, bool> condition;

        internal ManualRelayCommand(Action<object> action, Func<object, bool> condition)
        {
            this.action = action ?? throw new ArgumentNullException(nameof(action));
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public event EventHandler CanExecuteChanged
        {
            add => InternalCanExecuteChangedEventManager.AddHandler(this, value);
            remove => InternalCanExecuteChangedEventManager.RemoveHandler(this, value);
        }

        private event EventHandler? InternalCanExecuteChanged;

        internal void RaiseCanExecuteChanged()
        {
            var handler = this.InternalCanExecuteChanged;
            if (handler != null)
            {
                var application = Application.Current;
                if (application?.Dispatcher != null)
                {
                    _ = application.Dispatcher.BeginInvoke(new Action(() => handler(this, EventArgs.Empty)));
                }
                else
                {
                    handler(this, EventArgs.Empty);
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return this.condition(parameter);
        }

        public void Execute(object parameter)
        {
            this.action(parameter);
        }

        private class InternalCanExecuteChangedEventManager : WeakEventManager
        {
            private static readonly InternalCanExecuteChangedEventManager Manager = new InternalCanExecuteChangedEventManager();

            static InternalCanExecuteChangedEventManager()
            {
                SetCurrentManager(typeof(InternalCanExecuteChangedEventManager), Manager);
            }

            internal static void AddHandler(ManualRelayCommand source, EventHandler handler)
            {
                Manager.ProtectedAddHandler(source, handler);
            }

            internal static void RemoveHandler(ManualRelayCommand source, EventHandler handler)
            {
                Manager.ProtectedRemoveHandler(source, handler);
            }

            ////protected override ListenerList NewListenerList()
            ////{
            ////    return new ListenerList();
            ////}
            protected override void StartListening(object source)
            {
                ((ManualRelayCommand)source).InternalCanExecuteChanged += this.DeliverEvent;
            }

            protected override void StopListening(object source)
            {
                ((ManualRelayCommand)source).InternalCanExecuteChanged -= this.DeliverEvent;
            }
        }
    }
}
