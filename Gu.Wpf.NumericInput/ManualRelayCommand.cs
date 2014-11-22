namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Input;

    internal class ManualRelayCommand : ICommand
    {
        private readonly Action<object> _action;
        private readonly Func<object, bool> _condition;
        private bool? _previousCanExecute = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="condition"></param>
        internal ManualRelayCommand(Action<object> action, Func<object, bool> condition)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (condition == null)
            {
                throw new ArgumentNullException("condition");
            }
            _action = action;
            _condition = condition;
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                InternalCanExecuteChangedEventManager.AddHandler(this, value);
                _previousCanExecute = null;
            }
            remove
            {
                InternalCanExecuteChangedEventManager.RemoveHandler(this, value);
            }
        }

        private event EventHandler InternalCanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            var canExecute = CanExecute(null);
            if (canExecute == _previousCanExecute)
            {
                return;
            }
            _previousCanExecute = canExecute;

            var handler = InternalCanExecuteChanged;
            if (handler != null)
            {
                var application = Application.Current;
                if (application != null && application.Dispatcher != null)
                {
                    application.Dispatcher.BeginInvoke(new Action(() => handler(this, EventArgs.Empty)));
                }
                else
                {
                    handler(this, new EventArgs());
                }
            }
        }

        public bool CanExecute(object parameter)
        {
            return _condition(parameter);
        }

        public void Execute(object parameter)
        {
            _action(parameter);
            RaiseCanExecuteChanged();
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
                ((ManualRelayCommand)source).InternalCanExecuteChanged += DeliverEvent;
            }
            protected override void StopListening(object source)
            {
                ((ManualRelayCommand)source).InternalCanExecuteChanged -= DeliverEvent;
            }
        }
    }
}
