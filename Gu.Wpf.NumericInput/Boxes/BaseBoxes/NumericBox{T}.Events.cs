namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>Routed events for <see cref="NumericBox{T}"/>.</summary>
#pragma warning disable SA1619 // Generic type parameters should be documented partial class
    public abstract partial class NumericBox<T>
#pragma warning restore SA1619 // Generic type parameters should be documented partial class
    {
        /// <summary>Identifies the <see cref="ValueChanged"/> routed event.</summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            nameof(ValueChanged),
            RoutingStrategy.Direct,
            typeof(ValueChangedEventHandler<T?>),
            typeof(NumericBox<T>));

        /// <summary>
        /// Notifies about value changes.
        /// </summary>
        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public event ValueChangedEventHandler<T?> ValueChanged
        {
            add => this.AddHandler(ValueChangedEvent, value);
            remove => this.RemoveHandler(ValueChangedEvent, value);
        }
    }
}
