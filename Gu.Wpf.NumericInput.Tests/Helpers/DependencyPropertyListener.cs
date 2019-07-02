namespace Gu.Wpf.NumericInput.Tests
{
    using System;
    using System.Collections.Concurrent;
    using System.Windows;
    using System.Windows.Data;

    public sealed class DependencyPropertyListener : DependencyObject, IDisposable
    {
        private static readonly ConcurrentDictionary<DependencyProperty, PropertyPath> Cache = new ConcurrentDictionary<DependencyProperty, PropertyPath>();

        private static readonly DependencyProperty ProxyProperty = DependencyProperty.Register(
            "Proxy",
            typeof(object),
            typeof(DependencyPropertyListener),
            new PropertyMetadata(null, OnProxyChanged));

        private readonly Action<DependencyPropertyChangedEventArgs> onChanged;
        private bool disposed;

        public DependencyPropertyListener(
            DependencyObject source,
            DependencyProperty property,
            Action<DependencyPropertyChangedEventArgs> onChanged = null)
            : this(source, Cache.GetOrAdd(property, x => new PropertyPath(x)), onChanged)
        {
        }

        public DependencyPropertyListener(
            DependencyObject source,
            PropertyPath property,
            Action<DependencyPropertyChangedEventArgs> onChanged)
        {
            this.Binding = new Binding
            {
                Source = source,
                Path = property,
                Mode = BindingMode.OneWay,
                UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged,
            };
            this.BindingExpression = (BindingExpression)BindingOperations.SetBinding(this, ProxyProperty, this.Binding);
            this.onChanged = onChanged;
        }

        public event EventHandler<DependencyPropertyChangedEventArgs> Changed;

        public BindingExpression BindingExpression { get; }

        public Binding Binding { get; }

        public DependencyObject Source => (DependencyObject)this.Binding.Source;

        public void Dispose()
        {
            if (this.disposed)
            {
                return;
            }

            this.disposed = true;
            BindingOperations.ClearBinding(this, ProxyProperty);
        }

        private static void OnProxyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listener = (DependencyPropertyListener)d;
            if (listener.disposed)
            {
                return;
            }

            listener.onChanged?.Invoke(e);
            listener.OnChanged(e);
        }

        private void OnChanged(DependencyPropertyChangedEventArgs e)
        {
            this.Changed?.Invoke(this, e);
        }
    }
}
