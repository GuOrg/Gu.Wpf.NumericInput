namespace Gu.Wpf.NumericInput
{
    using System.Windows;

    /// <summary>
    /// Routed events for <see cref="BaseBox"/>.
    /// </summary>
    public abstract partial class BaseBox
    {
        /// <summary>Identifies the <see cref="FormatDirty"/> routed event.</summary>
        internal static readonly RoutedEvent FormatDirtyEvent = EventManager.RegisterRoutedEvent(
            nameof(FormatDirty),
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(BaseBox));

        /// <summary>Identifies the <see cref="ValidationDirty"/> routed event.</summary>
        internal static readonly RoutedEvent ValidationDirtyEvent = EventManager.RegisterRoutedEvent(
            nameof(ValidationDirty),
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(BaseBox));

        private static readonly RoutedEventArgs ValidationDirtyEventArgs = new RoutedEventArgs(ValidationDirtyEvent);
        private static readonly RoutedEventArgs FormatDirtyEventArgs = new RoutedEventArgs(FormatDirtyEvent);

        internal event RoutedEventHandler FormatDirty
        {
            add => this.AddHandler(FormatDirtyEvent, value);
            remove => this.RemoveHandler(FormatDirtyEvent, value);
        }

        internal event RoutedEventHandler ValidationDirty
        {
            add => this.AddHandler(ValidationDirtyEvent, value);
            remove => this.RemoveHandler(ValidationDirtyEvent, value);
        }
    }
}
