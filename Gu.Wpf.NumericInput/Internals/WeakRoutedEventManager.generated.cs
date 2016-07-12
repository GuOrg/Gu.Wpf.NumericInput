#pragma warning disable SA1402 // File may only contain a single class
namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Controls;

    internal static class WeakRoutedEventManager
    {
        internal static void AddWeakHandler(
            this UIElement source,
            RoutedEvent routedEvent,
            EventHandler<RoutedEventArgs> handler,
            bool handledEventsToo = false)
        {
            switch (routedEvent.Name)
            {
                case nameof(UIElement.GotKeyboardFocus):
                    GotKeyboardFocusEventManager.AddHandler(source, handler, handledEventsToo);
                    break;
                case nameof(UIElement.MouseUp):
                    MouseUpEventManager.AddHandler(source, handler, handledEventsToo);
                    break;
                case nameof(UIElement.PreviewMouseLeftButtonDown):
                    PreviewMouseLeftButtonDownEventManager.AddHandler(source, handler, handledEventsToo);
                    break;
                case nameof(UIElement.MouseLeftButtonDown):
                    MouseLeftButtonDownEventManager.AddHandler(source, handler, handledEventsToo);
                    break;
                case nameof(Control.MouseDoubleClick):
                    MouseDoubleClickEventManager.AddHandler(source, handler, handledEventsToo);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        internal static void RemoveWeakHandler(
            this UIElement source,
            RoutedEvent routedEvent,
            EventHandler<RoutedEventArgs> handler)
        {
            switch (routedEvent.Name)
            {
                case nameof(UIElement.GotKeyboardFocus):
                    GotKeyboardFocusEventManager.RemoveHandler(source, handler);
                    break;
                case nameof(UIElement.MouseUp):
                    MouseUpEventManager.RemoveHandler(source, handler);
                    break;
                case nameof(UIElement.PreviewMouseLeftButtonDown):
                    PreviewMouseLeftButtonDownEventManager.RemoveHandler(source, handler);
                    break;
                case nameof(UIElement.MouseLeftButtonDown):
                    MouseLeftButtonDownEventManager.RemoveHandler(source, handler);
                    break;
                case nameof(Control.MouseDoubleClick):
                    MouseDoubleClickEventManager.RemoveHandler(source, handler);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }

    internal class GotKeyboardFocusEventManager : WeakEventManager
    {
        private readonly RoutedEventHandler handler;
        private bool handledEventsToo;

        private GotKeyboardFocusEventManager()
        {
            this.handler = new RoutedEventHandler(this.OnGotKeyboardFocus);
        }

        // get the event manager for the current thread
        private static GotKeyboardFocusEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(GotKeyboardFocusEventManager);
                var manager = (GotKeyboardFocusEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new GotKeyboardFocusEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void AddListener(UIElement source, IWeakEventListener listener, bool handledEventsToo)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddListener(source, listener);
        }

        internal static void RemoveListener(UIElement source, IWeakEventListener listener)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        internal static void AddHandler(UIElement source, EventHandler<RoutedEventArgs> handler, bool handledEventsToo)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddHandler(source, handler);
        }

        internal static void RemoveHandler(UIElement source, EventHandler<RoutedEventArgs> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        protected override ListenerList NewListenerList()
        {
            return new ListenerList<RoutedEventArgs>();
        }

        protected override void StartListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.AddHandler(UIElement.GotKeyboardFocusEvent, this.handler, this.handledEventsToo);
        }

        protected override void StopListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.RemoveHandler(UIElement.GotKeyboardFocusEvent, this.handler);
        }

        // event handler for GotKeyboardFocus event
        private void OnGotKeyboardFocus(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }

    internal class MouseUpEventManager : WeakEventManager
    {
        private readonly RoutedEventHandler handler;
        private bool handledEventsToo;

        private MouseUpEventManager()
        {
            this.handler = new RoutedEventHandler(this.OnMouseUp);
        }

        // get the event manager for the current thread
        private static MouseUpEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(MouseUpEventManager);
                var manager = (MouseUpEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new MouseUpEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void AddListener(UIElement source, IWeakEventListener listener, bool handledEventsToo)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddListener(source, listener);
        }

        internal static void RemoveListener(UIElement source, IWeakEventListener listener)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        internal static void AddHandler(UIElement source, EventHandler<RoutedEventArgs> handler, bool handledEventsToo)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddHandler(source, handler);
        }

        internal static void RemoveHandler(UIElement source, EventHandler<RoutedEventArgs> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        protected override ListenerList NewListenerList()
        {
            return new ListenerList<RoutedEventArgs>();
        }

        protected override void StartListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.AddHandler(UIElement.MouseUpEvent, this.handler, this.handledEventsToo);
        }

        protected override void StopListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.RemoveHandler(UIElement.MouseUpEvent, this.handler);
        }

        // event handler for MouseUp event
        private void OnMouseUp(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }

    internal class PreviewMouseLeftButtonDownEventManager : WeakEventManager
    {
        private readonly RoutedEventHandler handler;
        private bool handledEventsToo;

        private PreviewMouseLeftButtonDownEventManager()
        {
            this.handler = new RoutedEventHandler(this.OnPreviewMouseLeftButtonDown);
        }

        // get the event manager for the current thread
        private static PreviewMouseLeftButtonDownEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(PreviewMouseLeftButtonDownEventManager);
                var manager = (PreviewMouseLeftButtonDownEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new PreviewMouseLeftButtonDownEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void AddListener(UIElement source, IWeakEventListener listener, bool handledEventsToo)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddListener(source, listener);
        }

        internal static void RemoveListener(UIElement source, IWeakEventListener listener)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        internal static void AddHandler(UIElement source, EventHandler<RoutedEventArgs> handler, bool handledEventsToo)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddHandler(source, handler);
        }

        internal static void RemoveHandler(UIElement source, EventHandler<RoutedEventArgs> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        protected override ListenerList NewListenerList()
        {
            return new ListenerList<RoutedEventArgs>();
        }

        protected override void StartListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.AddHandler(UIElement.PreviewMouseLeftButtonDownEvent, this.handler, this.handledEventsToo);
        }

        protected override void StopListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.RemoveHandler(UIElement.PreviewMouseLeftButtonDownEvent, this.handler);
        }

        // event handler for PreviewMouseLeftButtonDown event
        private void OnPreviewMouseLeftButtonDown(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }

    internal class MouseLeftButtonDownEventManager : WeakEventManager
    {
        private readonly RoutedEventHandler handler;
        private bool handledEventsToo;

        private MouseLeftButtonDownEventManager()
        {
            this.handler = new RoutedEventHandler(this.OnMouseLeftButtonDown);
        }

        // get the event manager for the current thread
        private static MouseLeftButtonDownEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(MouseLeftButtonDownEventManager);
                var manager = (MouseLeftButtonDownEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new MouseLeftButtonDownEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void AddListener(UIElement source, IWeakEventListener listener, bool handledEventsToo)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddListener(source, listener);
        }

        internal static void RemoveListener(UIElement source, IWeakEventListener listener)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        internal static void AddHandler(UIElement source, EventHandler<RoutedEventArgs> handler, bool handledEventsToo)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddHandler(source, handler);
        }

        internal static void RemoveHandler(UIElement source, EventHandler<RoutedEventArgs> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        protected override ListenerList NewListenerList()
        {
            return new ListenerList<RoutedEventArgs>();
        }

        protected override void StartListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.AddHandler(UIElement.MouseLeftButtonDownEvent, this.handler, this.handledEventsToo);
        }

        protected override void StopListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.RemoveHandler(UIElement.MouseLeftButtonDownEvent, this.handler);
        }

        // event handler for MouseLeftButtonDown event
        private void OnMouseLeftButtonDown(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }

    internal class MouseDoubleClickEventManager : WeakEventManager
    {
        private readonly RoutedEventHandler handler;
        private bool handledEventsToo;

        private MouseDoubleClickEventManager()
        {
            this.handler = new RoutedEventHandler(this.OnMouseDoubleClick);
        }

        // get the event manager for the current thread
        private static MouseDoubleClickEventManager CurrentManager
        {
            get
            {
                var managerType = typeof(MouseDoubleClickEventManager);
                var manager = (MouseDoubleClickEventManager)GetCurrentManager(managerType);

                // at first use, create and register a new manager
                if (manager == null)
                {
                    manager = new MouseDoubleClickEventManager();
                    SetCurrentManager(managerType, manager);
                }

                return manager;
            }
        }

        internal static void AddListener(UIElement source, IWeakEventListener listener, bool handledEventsToo)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddListener(source, listener);
        }

        internal static void RemoveListener(UIElement source, IWeakEventListener listener)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (listener == null)
            {
                throw new ArgumentNullException(nameof(listener));
            }

            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        internal static void AddHandler(UIElement source, EventHandler<RoutedEventArgs> handler, bool handledEventsToo)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            var manager = CurrentManager;
            manager.handledEventsToo = handledEventsToo;
            manager.ProtectedAddHandler(source, handler);
        }

        internal static void RemoveHandler(UIElement source, EventHandler<RoutedEventArgs> handler)
        {
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            CurrentManager.ProtectedRemoveHandler(source, handler);
        }

        protected override ListenerList NewListenerList()
        {
            return new ListenerList<RoutedEventArgs>();
        }

        protected override void StartListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.AddHandler(Control.MouseDoubleClickEvent, this.handler, this.handledEventsToo);
        }

        protected override void StopListening(object source)
        {
            var uiElement = (UIElement)source;
            uiElement.RemoveHandler(Control.MouseDoubleClickEvent, this.handler);
        }

        // event handler for MouseDoubleClick event
        private void OnMouseDoubleClick(object sender, RoutedEventArgs args)
        {
            this.DeliverEvent(sender, args);
        }
    }
}
