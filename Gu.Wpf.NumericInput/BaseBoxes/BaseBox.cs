namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Media;

    /// <summary>
    /// The reason for having this stuff here is enabling a shared style
    /// </summary>
    [TemplatePart(Name = FormattedName, Type = typeof(TextBox))]
    public abstract partial class BaseBox : TextBox
    {
        private static readonly RoutedEventHandler LoadedHandler = new RoutedEventHandler(OnLoaded);

        // this is only used to create the binding expression needed for Validator
        private static readonly Binding ValidationBinding = new Binding { Mode = BindingMode.OneTime, Source = string.Empty, NotifyOnValidationError = true };

        public const string FormattedName = "PART_FormattedText";

        protected BaseBox()
        {
            this.AddHandler(LoadedEvent, LoadedHandler);
            this.TextBindingExpression = (BindingExpression)BindingOperations.SetBinding(this, NumericBox.TextProperty, ValidationBinding);
        }

        internal BindingExpression TextBindingExpression { get; }

        public abstract void UpdateValidation();

        protected override void OnPreviewLostKeyboardFocus(KeyboardFocusChangedEventArgs e)
        {
            base.OnPreviewLostKeyboardFocus(e);
            if (this.IsValidationDirty && this.ValidationTrigger == ValidationTrigger.LostFocus)
            {
                var status = this.Status;
                this.Status = Status.Validating;
                this.UpdateValidation();
                this.IsValidationDirty = false;
                this.Status = status;
            }
        }

        protected virtual void SetTextAndCreateUndoAction(string text)
        {
            this.TextSource = TextSource.UserInput;
            this.BeginChange();
            this.SetCurrentValue(TextProperty, text);
            this.EndChange();
        }

        protected virtual void SetTextClearUndo(string text)
        {
            var isUndoEnabled = this.IsUndoEnabled;
            this.IsUndoEnabled = false;
            this.SetCurrentValue(TextProperty, text);
            this.IsUndoEnabled = isUndoEnabled;
        }

        protected virtual void OnStringFormatChanged(string oldFormat, string newFormat)
        {
        }

        protected virtual void OnCultureChanged(IFormatProvider oldCulture, IFormatProvider newCulture)
        {
        }

        protected void UpdateView()
        {
            if (!this.IsLoaded || this.HasFormattedView || string.IsNullOrEmpty(this.StringFormat))
            {
                return;
            }

            var chrome = this.VisualChildren().SingleOrNull<Decorator>();
            var scrollViewer = this.Template?.FindName("PART_ContentHost", this) as ScrollViewer;
            var scrollContentPresenter = scrollViewer?.NestedChildren().SingleOrNull<ScrollContentPresenter>();
            var grid = scrollContentPresenter?.Parent as Grid;
            var textView = scrollContentPresenter?.VisualChildren().SingleOrNull<IScrollInfo>() as FrameworkElement;
            if (scrollViewer == null || scrollContentPresenter == null || grid == null || textView == null || chrome == null)
            {
                {
                    // Falling back to vanilla textbox in runtime
                    return;
                }
            }

            var formattedBox = (TextBlock)grid.FindName(FormattedName);
            if (formattedBox == null)
            {
                this.HasFormattedView = true;
                formattedBox = new TextBlock
                {
                    Name = FormattedName,
                    VerticalAlignment = VerticalAlignment.Center,
                    IsHitTestVisible = false
                };

                formattedBox.Bind(TextBlock.TextProperty)
                    .OneWayTo(this, FormattedTextProperty);

                formattedBox.Bind(MarginProperty)
                    .OneWayTo(scrollContentPresenter, MarginProperty, FormattedTextBlockMarginConverter.Default, scrollContentPresenter);

                formattedBox.Bind(VisibilityProperty)
                    .OneWayTo(this, IsKeyboardFocusWithinProperty, HiddenWhenTrueConverter.Default);

                formattedBox.Bind(TextBlock.BackgroundProperty)
                            .OneWayTo(chrome, BackgroundProperty);

                grid.Children.Add(formattedBox);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == IsVisibleProperty || e.Property == StringFormatProperty)
            {
                if (this.IsVisible && !string.IsNullOrEmpty(this.StringFormat))
                {
                    this.UpdateView();
                }
            }

            base.OnPropertyChanged(e);
        }

        protected virtual void OnLoaded()
        {
            this.UpdateView();
        }

        public override void OnApplyTemplate()
        {
            this.HasFormattedView = false;
            base.OnApplyTemplate();
            this.UpdateView();
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var box = (BaseBox)sender;
            box.OnLoaded();
        }
    }
}
