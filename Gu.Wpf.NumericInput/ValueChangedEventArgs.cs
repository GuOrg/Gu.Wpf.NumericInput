namespace Gu.Wpf.NumericInput
{
    using System.Windows;

    /// <summary>
    /// Information about a value change.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    /// <param name="sender">The sender.</param>
    /// <param name="e">The <see cref="ValueChangedEventArgs{T}"/>.</param>
#pragma warning disable CA1711 // Identifiers should not have incorrect suffix
#pragma warning disable CA1003 // Use generic event handler instances
    public delegate void ValueChangedEventHandler<T>(object? sender, ValueChangedEventArgs<T> e);
#pragma warning restore CA1003 // Use generic event handler instances
#pragma warning restore CA1711 // Identifiers should not have incorrect suffix

    /// <summary>
    /// Information about a value change.
    /// </summary>
    /// <typeparam name="T">The type of the value.</typeparam>
    public class ValueChangedEventArgs<T> : RoutedEventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="routedEvent">The <see cref="RoutedEvent"/>.</param>
        public ValueChangedEventArgs(T oldValue, T newValue, RoutedEvent routedEvent)
            : base(routedEvent)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueChangedEventArgs{T}"/> class.
        /// </summary>
        /// <param name="oldValue">The old value.</param>
        /// <param name="newValue">The new value.</param>
        /// <param name="routedEvent">The <see cref="RoutedEvent"/>.</param>
        /// <param name="source">The <see cref="object"/>.</param>
        public ValueChangedEventArgs(T oldValue, T newValue, RoutedEvent routedEvent, object source)
            : base(routedEvent, source)
        {
            this.OldValue = oldValue;
            this.NewValue = newValue;
        }

        /// <summary>
        /// Gets the old value.
        /// </summary>
        public T OldValue { get; }

        /// <summary>
        /// gets the new value.
        /// </summary>
        public T NewValue { get; }
    }
}
