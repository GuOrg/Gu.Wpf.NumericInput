namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;
    using System.Windows.Threading;

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
        private bool hasFormattedView;

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
            if (this.hasFormattedView)
            {
                return;
            }

            var scrollViewer = this.Template?.FindName("PART_ContentHost", this) as ScrollViewer;
            if (scrollViewer != null && this.IsLoaded && !scrollViewer.IsLoaded)
            {
                // let visual tree build
                this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(this.UpdateView));
                return;
            }

            var whenFocused = scrollViewer?.NestedChildren().OfType<ScrollContentPresenter>().SingleOrDefault();
            var grid = whenFocused?.Parent as Grid;
            if (scrollViewer == null || whenFocused == null || grid == null)
            {
                if (DesignerProperties.GetIsInDesignMode(this))
                {
                    var message = $"The template does not match the expected template.\r\n" +
                                  $"Cannot create formatted view\r\n" +
                                  $"The expected template is (pseudo)\r\n" +
                                  $"{nameof(ScrollViewer)}: {(scrollViewer == null ? "null" : "correct")}\r\n" +
                                  $"  {nameof(Grid)}: {(grid == null ? "null" : "correct")}\r\n" +
                                  $"    {nameof(ScrollContentPresenter)}: {(whenFocused == null ? "null" : "correct")}\r\n" +
                                  $"But was:\r\n" +
                                  $"{this.DumpVisualTree()}";

                    throw new InvalidOperationException(message);
                }
                else
                {
                    // Falling back to vanilla textbox in runtime
                    return;
                }
            }

            var formattedBox = (TextBlock)grid.FindName(FormattedName);
            if (formattedBox == null)
            {
                this.hasFormattedView = true;
                var whenNotFocused = new TextBlock
                {
                    Name = FormattedName,
                    VerticalAlignment = VerticalAlignment.Center
                };

                whenNotFocused.Bind(TextBlock.TextProperty)
                    .OneWayTo(this, FormattedTextProperty);

                whenNotFocused.Bind(MarginProperty)
                    .OneWayTo(whenFocused, MarginProperty, FormattedTextBlockMarginConverter.Default, whenFocused);

                whenNotFocused.Bind(VisibilityProperty)
                    .OneWayTo(this, IsKeyboardFocusWithinProperty, HiddenWhenTrueConverter.Default);

                grid.Children.Add(whenNotFocused);

                whenFocused.Bind(VisibilityProperty)
                    .OneWayTo(this, IsKeyboardFocusWithinProperty, VisibleWhenTrueConverter.Default);
            }
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == VisibilityProperty || e.Property == StringFormatProperty)
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
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var box = (BaseBox)sender;
            box.OnLoaded();
        }
    }
}
