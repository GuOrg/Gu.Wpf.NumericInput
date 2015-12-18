namespace Gu.Wpf.NumericInput
{
    using System.Windows;

    public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> e);

    public class ValueChangedEventArgs<T> : RoutedEventArgs
    {
        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public ValueChangedEventArgs(T oldValue, T newValue, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public ValueChangedEventArgs(T oldValue, T newValue, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        public T OldValue { get; private set; }

        public T NewValue { get; private set; }
    }
}
