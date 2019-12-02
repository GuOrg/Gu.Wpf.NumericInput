namespace Gu.Wpf.NumericInput
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Input;

    internal class FormattedView
    {
        private readonly BaseBox baseBox;
        private TextBlock formattedBox;
        private FrameworkElement textView;
        internal const string FormattedName = "PART_FormattedText";

        static FormattedView()
        {
            EventManager.RegisterClassHandler(typeof(BaseBox), Keyboard.PreviewLostKeyboardFocusEvent, new RoutedEventHandler(OnPreviewLostKeyboardFocus));
            EventManager.RegisterClassHandler(typeof(BaseBox), Keyboard.PreviewGotKeyboardFocusEvent, new RoutedEventHandler(OnPreviewGotKeyboardFocus));
        }

        internal FormattedView(BaseBox baseBox)
        {
            this.baseBox = baseBox;
        }

        internal void UpdateView()
        {
            if (!this.baseBox.IsLoaded || string.IsNullOrEmpty(this.baseBox.StringFormat))
            {
                return;
            }

            if (this.baseBox.VisualChildren().SingleOrNull<Decorator>() is { } &&
                this.baseBox.Template?.FindName("PART_ContentHost", this.baseBox) is ScrollViewer scrollViewer &&
                scrollViewer.NestedChildren().SingleOrNull<ScrollContentPresenter>() is { Parent: Grid grid } scrollContentPresenter)
            {
                this.textView = scrollContentPresenter.VisualChildren().SingleOrNull<IScrollInfo>() as FrameworkElement;
                this.formattedBox = grid.Children.OfType<TextBlock>().SingleOrDefault(x => x.Name == FormattedName);
                if (this.formattedBox == null)
                {
                    this.baseBox.HasFormattedView = true;
                    this.formattedBox = new TextBlock
                    {
                        Name = FormattedName,
                        VerticalAlignment = VerticalAlignment.Center,
                        IsHitTestVisible = false,
                    };

                    _ = this.formattedBox.Bind(TextBlock.TextProperty)
                            .OneWayTo(this.baseBox, BaseBox.FormattedTextProperty);

                    _ = this.formattedBox.Bind(FrameworkElement.MarginProperty)
                            .OneWayTo(scrollContentPresenter, FrameworkElement.MarginProperty, FormattedTextBlockMarginConverter.Default, scrollContentPresenter);

                    this.UpdateVisibility();

                    grid.Children.Add(this.formattedBox);
                }
                else
                {
                    this.UpdateVisibility();
                }
            }
            else
            {
                // Failing to no formatting
                if (this.formattedBox is { Parent: Grid parent })
                {
                    parent.Children.Remove(this.formattedBox);
                }

                this.formattedBox = null;
                this.textView = null;
            }
        }

        private static void OnPreviewGotKeyboardFocus(object sender, RoutedEventArgs e)
        {
            var box = (BaseBox)sender;
            box.FormattedView.HideFormatted();
        }

        private static void OnPreviewLostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            var box = (BaseBox)sender;
            box.FormattedView.ShowFormatted();
        }

        private void HideFormatted()
        {
            this.formattedBox?.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Hidden);
            this.textView?.InvalidateProperty(UIElement.OpacityProperty);
        }

        private void ShowFormatted()
        {
            if (this.formattedBox != null)
            {
                this.formattedBox.SetCurrentValue(UIElement.VisibilityProperty, Visibility.Visible);
                this.textView?.SetCurrentValue(UIElement.OpacityProperty, 0.0);
            }
            else
            {
                this.textView?.InvalidateProperty(UIElement.OpacityProperty);
            }
        }

        private void UpdateVisibility()
        {
            if (this.baseBox.IsKeyboardFocusWithin)
            {
                this.HideFormatted();
            }
            else
            {
                this.ShowFormatted();
            }
        }
    }
}
