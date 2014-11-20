namespace Gu.Wpf.NumericControls
{
    using System.Windows;
    public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> e);
    public class ValueChangedEventArgs<T> : RoutedEventArgs
    {
        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public ValueChangedEventArgs(T oldValue, T newValue, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public ValueChangedEventArgs(T oldValue, T newValue, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }

        public T OldValue { get; private set; }
        public T NewValue { get; private set; }
    }
}
