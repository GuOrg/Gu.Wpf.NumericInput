namespace Gu.Wpf.NumericInput
{
    using System.Windows;

    public abstract partial class BaseBox
    {
        internal static readonly RoutedEvent FormatDirtyEvent = EventManager.RegisterRoutedEvent(
            "FormatDirty",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(BaseBox));

        internal static readonly RoutedEvent ValidationDirtyEvent = EventManager.RegisterRoutedEvent(
            "ValidationDirty",
            RoutingStrategy.Direct,
            typeof(RoutedEventHandler),
            typeof(BaseBox));

        protected static readonly RoutedEventArgs ValidationDirtyEventArgs = new RoutedEventArgs(ValidationDirtyEvent);
        protected static readonly RoutedEventArgs FormatDirtyEventArgs = new RoutedEventArgs(FormatDirtyEvent);

        internal event RoutedEventHandler FormatDirty
        {
            add { this.AddHandler(FormatDirtyEvent, value); }
            remove { this.RemoveHandler(FormatDirtyEvent, value); }
        }

        internal event RoutedEventHandler ValidationDirty
        {
            add { this.AddHandler(ValidationDirtyEvent, value); }
            remove { this.RemoveHandler(ValidationDirtyEvent, value); }
        }
    }
}
