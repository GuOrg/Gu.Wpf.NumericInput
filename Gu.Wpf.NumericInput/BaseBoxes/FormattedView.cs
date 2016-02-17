namespace Gu.Wpf.NumericInput
{
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Documents;
    using System.Windows.Input;
    using System.Windows.Media;

    internal class FormattedView
    {
        private readonly BaseBox baseBox;
        private TextBlock formattedBox;
        private FrameworkElement textView;
        public const string FormattedName = "PART_FormattedText";

        static FormattedView()
        {
            EventManager.RegisterClassHandler(typeof(BaseBox), Keyboard.PreviewLostKeyboardFocusEvent, new RoutedEventHandler(OnPreviewLostKeyboardFocus));
            EventManager.RegisterClassHandler(typeof(BaseBox), Keyboard.PreviewGotKeyboardFocusEvent, new RoutedEventHandler(OnPreviewGotKeyboardFocus));
        }

        public FormattedView(BaseBox baseBox)
        {
            this.baseBox = baseBox;
        }

        public void UpdateView()
        {
            if (!this.baseBox.IsLoaded || string.IsNullOrEmpty(this.baseBox.StringFormat))
            {
                return;
            }

            var chrome = this.baseBox.VisualChildren().SingleOrNull<Decorator>();
            var scrollViewer = this.baseBox.Template?.FindName("PART_ContentHost", this.baseBox) as ScrollViewer;
            var scrollContentPresenter = scrollViewer?.NestedChildren().SingleOrNull<ScrollContentPresenter>();
            var grid = scrollContentPresenter?.Parent as Grid;
            this.textView = scrollContentPresenter?.VisualChildren().SingleOrNull<IScrollInfo>() as FrameworkElement;

            if (scrollViewer == null || scrollContentPresenter == null || grid == null || this.textView == null || chrome == null)
            {
                {
                    if (this.formattedBox != null)
                    {
                        grid = this.formattedBox.Parent as Grid;
                        grid?.Children.Remove(this.formattedBox);
                    }

                    this.formattedBox = null;
                    this.textView = null;
                    
                    // Failing to no formatting
                    return;
                }
            }

            this.formattedBox = grid.Children.OfType<TextBlock>().SingleOrDefault(x => x.Name == FormattedName);
            if (this.formattedBox == null)
            {
                this.baseBox.HasFormattedView = true;
                this.formattedBox = new TextBlock
                {
                    Name = FormattedName,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsHitTestVisible = false
                };

                this.formattedBox.Bind(TextBlock.TextProperty)
                    .OneWayTo(this.baseBox, BaseBox.FormattedTextProperty);

                this.formattedBox.Bind(FrameworkElement.MarginProperty)
                    .OneWayTo(scrollContentPresenter, FrameworkElement.MarginProperty, FormattedTextBlockMarginConverter.Default, scrollContentPresenter);

                this.UpdateVisibility();

                grid.Children.Add(this.formattedBox);
            }
            else
            {
                this.UpdateVisibility();
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
            if (this.formattedBox != null)
            {
                this.formattedBox.Visibility = Visibility.Hidden;
            }

            this.textView?.InvalidateProperty(UIElement.OpacityProperty);
        }

        private void ShowFormatted()
        {
            if (this.formattedBox != null)
            {
                this.formattedBox.Visibility = Visibility.Visible;
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
