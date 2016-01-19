namespace Gu.Wpf.NumericInput
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Controls.Primitives;
    using System.Windows.Threading;

    /// <summary>
    /// The reason for having this stuff here is enabling a shared style
    /// </summary>
    [TemplatePart(Name = IncreaseButtonName, Type = typeof(RepeatButton))]
    [TemplatePart(Name = DecreaseButtonName, Type = typeof(RepeatButton))]
    [TemplatePart(Name = FormattedName, Type = typeof(TextBox))]
    [TemplatePart(Name = SuffixBoxName, Type = typeof(TextBox))]
    public abstract partial class BaseBox : TextBox
    {
        public const string DecreaseButtonName = "PART_DecreaseButton";
        public const string IncreaseButtonName = "PART_IncreaseButton";
        public const string EditBoxName = "PART_EditText";
        public const string FormattedName = "PART_FormattedText";
        public const string SuffixBoxName = "PART_SuffixText";

        protected BaseBox()
        {
            this.IncreaseCommand = new ManualRelayCommand(this.Increase, this.CanIncrease);
            this.DecreaseCommand = new ManualRelayCommand(this.Decrease, this.CanDecrease);
            this.Bind(TextProxyProperty).OneWayTo(this, TextProperty);
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.UpdateView();
        }

        protected virtual void SetTextAndCreateUndoAction(string text)
        {
            var canUndo = this.CanUndo;
            this.TextSource = TextSource.UserInput;
            this.BeginChange();
            this.SetCurrentValue(TextProperty, text);
            this.EndChange();
            Debug.WriteLine(canUndo, this.CanUndo);
        }

        protected virtual void SetTextClearUndo(string text)
        {
            var canUndo = this.CanUndo;
            var isUndoEnabled = this.IsUndoEnabled;
            this.IsUndoEnabled = false;
            this.SetCurrentValue(TextProperty, text);
            this.IsUndoEnabled = isUndoEnabled;
            Debug.WriteLine(canUndo, this.CanUndo);
        }

        /// <summary>
        /// Invoked when IncreaseCommand.CanExecute() is executed
        /// </summary>
        /// <param name="parameter">The inner <see cref="System.Windows.Controls.TextBox"/> showing the value in the controltemplate</param>
        /// <returns>True if the value can be increased</returns>
        protected abstract bool CanIncrease(object parameter);

        /// <summary>
        /// Invoked when IncreaseCommand.Execute() is executed
        /// </summary>
        /// <param name="parameter">The inner <see cref="System.Windows.Controls.TextBox"/> showing the value in the controltemplate</param>
        protected abstract void Increase(object parameter);

        /// <summary>
        /// Invoked when DecreaseCommand.CanExecute() is executed
        /// </summary>
        /// <param name="parameter">The inner <see cref="System.Windows.Controls.TextBox"/> showing the value in the controltemplate</param>
        /// <returns>True if the value can be decreased</returns>
        protected abstract bool CanDecrease(object parameter);

        /// <summary>
        /// Invoked when DecreaseCommand.Execute() is executed
        /// </summary>
        /// <param name="parameter">The inner <see cref="System.Windows.Controls.TextBox"/> showing the value in the controltemplate</param>
        protected abstract void Decrease(object parameter);

        protected virtual void CheckSpinners()
        {
            if (this.AllowSpinners)
            {
                (this.IncreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
                (this.DecreaseCommand as ManualRelayCommand)?.RaiseCanExecuteChanged();
            }
        }

        protected virtual void OnStringFormatChanged(string oldFormat, string newFormat)
        {
        }

        protected virtual void OnCultureChanged(IFormatProvider oldCulture, IFormatProvider newCulture)
        {
        }

        protected void UpdateView()
        {
            var scrollViewer = this.GetTemplateChild("PART_ContentHost") as ScrollViewer;
            var whenFocused = scrollViewer?.NestedChildren().OfType<ScrollContentPresenter>().SingleOrDefault();
            var grid = whenFocused?.Parent as Grid;
            if (scrollViewer == null || whenFocused == null || grid == null)
            {
                if (!this.IsLoaded)
                {
                    this.Loaded += OnLoaded;
                    return;
                }

                if (this.IsArrangeValid == false)
                {
                    // retry after arrange
                    // using the Loaded event does not work if template is changed in runtime.
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Loaded, new Action(this.UpdateView));
                    return;
                }

                if (DesignerProperties.GetIsInDesignMode(this))
                {
                    var message = $"The template does not match the expected template. Cannot use formatting\r\n" +
                                  $"The expected template is (pseudo)\r\n" +
                                  $"{nameof(ScrollViewer)}: {(scrollViewer == null ? "null" : string.Empty)}\r\n" +
                                  $"  {nameof(Grid)}: {(grid == null ? "null" : string.Empty)}\r\n" +
                                  $"    {nameof(ScrollContentPresenter)}: {(whenFocused == null ? "null" : string.Empty)}";
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

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var baseBox = (BaseBox)sender;
            baseBox.Loaded -= OnLoaded;
            baseBox.UpdateView();
        }
    }
}
