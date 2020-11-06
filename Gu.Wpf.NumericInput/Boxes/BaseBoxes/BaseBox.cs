namespace Gu.Wpf.NumericInput
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;
    using System.Windows.Input;

    /// <summary>
    /// Base class that adds a couple of dependency properties to TextBox.
    /// </summary>
    public abstract partial class BaseBox : TextBox
    {
        private static readonly RoutedEventHandler LoadedHandler = OnLoaded;

        // this is only used to create the binding expression needed for validaton
        private static readonly Binding ValidationBinding = new Binding { Mode = BindingMode.OneTime, Source = string.Empty, NotifyOnValidationError = true };

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseBox"/> class.
        /// </summary>
        protected BaseBox()
        {
            this.AddHandler(LoadedEvent, LoadedHandler);
            this.TextBindingExpression = (BindingExpression)BindingOperations.SetBinding(this, NumericBox.TextProperty, ValidationBinding);
            this.FormattedView = new FormattedView(this);
        }

        internal BindingExpression TextBindingExpression { get; }

        internal FormattedView FormattedView { get; }

        /// <summary>
        /// Update the validation.
        /// </summary>
        public abstract void UpdateValidation();

        /// <inheritdoc/>
        public override void OnApplyTemplate()
        {
            this.HasFormattedView = false;
            base.OnApplyTemplate();
            this.FormattedView.UpdateView();
        }

        /// <inheritdoc/>
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

        /// <summary>
        /// This is called when a new value comes from the user.
        /// </summary>
        /// <param name="text">The new text.</param>
        protected virtual void SetTextAndCreateUndoAction(string text)
        {
            this.TextSource = TextSource.UserInput;
            this.BeginChange();
            this.SetCurrentValue(TextProperty, text);
            this.EndChange();
        }

        /// <summary>
        /// This is called when a new value comes from the binding.
        /// </summary>
        /// <param name="text">The new text.</param>
        protected virtual void SetTextClearUndo(string text)
        {
            var isUndoEnabled = this.GetValue(IsUndoEnabledProperty);
            this.SetCurrentValue(IsUndoEnabledProperty, false);
            this.SetCurrentValue(TextProperty, text);
            this.SetCurrentValue(IsUndoEnabledProperty, isUndoEnabled);
        }

        /// <summary>
        /// Called when <see cref="StringFormatProperty"/> changes.
        /// </summary>
        /// <param name="oldFormat">The old format.</param>
        /// <param name="newFormat">The new format.</param>
        protected virtual void OnStringFormatChanged(string? oldFormat, string? newFormat)
        {
        }

        /// <summary>
        /// Called when <see cref="CultureProperty"/> changes.
        /// </summary>
        /// <param name="oldCulture">The old <see cref="IFormatProvider"/>.</param>
        /// <param name="newCulture">The new <see cref="IFormatProvider"/>.</param>
        protected virtual void OnCultureChanged(IFormatProvider oldCulture, IFormatProvider newCulture)
        {
        }

        /// <inheritdoc/>
        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == IsVisibleProperty || e.Property == StringFormatProperty)
            {
                if (this.IsVisible && !string.IsNullOrEmpty(this.StringFormat))
                {
                    this.FormattedView.UpdateView();
                }
            }

            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// Called when the control is loaded.
        /// </summary>
        protected virtual void OnLoaded()
        {
            this.FormattedView.UpdateView();
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var box = (BaseBox)sender;
            box.OnLoaded();
        }
    }
}
