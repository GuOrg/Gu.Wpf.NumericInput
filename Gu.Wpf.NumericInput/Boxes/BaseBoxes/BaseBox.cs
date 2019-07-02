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

        // this is only used to create the binding expression needed for Validator
        private static readonly Binding ValidationBinding = new Binding { Mode = BindingMode.OneTime, Source = string.Empty, NotifyOnValidationError = true };

        protected BaseBox()
        {
            this.AddHandler(LoadedEvent, LoadedHandler);
            this.TextBindingExpression = (BindingExpression)BindingOperations.SetBinding(this, NumericBox.TextProperty, ValidationBinding);
            this.FormattedView = new FormattedView(this);
        }

        internal BindingExpression TextBindingExpression { get; }

        internal FormattedView FormattedView { get; }

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
            var isUndoEnabled = this.GetValue(IsUndoEnabledProperty);
            this.SetCurrentValue(IsUndoEnabledProperty, false);
            this.SetCurrentValue(TextProperty, text);
            this.SetCurrentValue(IsUndoEnabledProperty, isUndoEnabled);
        }

        //// ReSharper disable UnusedParameter.Global
        protected virtual void OnStringFormatChanged(string oldFormat, string newFormat)
        //// ReSharper restore UnusedParameter.Global
        {
        }

        protected virtual void OnCultureChanged(IFormatProvider oldCulture, IFormatProvider newCulture)
        {
        }

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

        protected virtual void OnLoaded()
        {
            this.FormattedView.UpdateView();
        }

        public override void OnApplyTemplate()
        {
            this.HasFormattedView = false;
            base.OnApplyTemplate();
            this.FormattedView.UpdateView();
        }

        private static void OnLoaded(object sender, RoutedEventArgs e)
        {
            var box = (BaseBox)sender;
            box.OnLoaded();
        }
    }
}
