namespace Gu.Wpf.NumericInput
{
    using System.ComponentModel;
    using System.Windows;

    /// <summary>Routed events for <see cref="NumericBox{T}"/>.</summary>
    /// <typeparam name="T">The type of the numeric value.</typeparam>
    public abstract partial class NumericBox<T>
    {
        /// <summary>Identifies the ValueChanged event</summary>
        public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent(
            "ValueChanged",
            RoutingStrategy.Direct,
            typeof(ValueChangedEventHandler<T?>),
            typeof(NumericBox<T>));

        [Category(nameof(NumericBox))]
        [Browsable(true)]
        public event ValueChangedEventHandler<T?> ValueChanged
        {
            add { this.AddHandler(ValueChangedEvent, value); }
            remove { this.RemoveHandler(ValueChangedEvent, value); }
        }
    }
}
