namespace Gu.Wpf.NumericInput
{
    using System.Windows;

    /// <summary>
    /// The reason for having this stuff here is enabling a shared style
    /// </summary>
    public abstract partial class BaseBox
    {
        protected BaseBox()
        {
            this.IncreaseCommand = new ManualRelayCommand(this.Increase, this.CanIncrease);
            this.DecreaseCommand = new ManualRelayCommand(this.Decrease, this.CanDecrease);
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
                // Not nice to cast like this but want to have ManualRelayCommand as internal
                ((ManualRelayCommand)this.IncreaseCommand).RaiseCanExecuteChanged();
                ((ManualRelayCommand)this.DecreaseCommand).RaiseCanExecuteChanged();
            }
        }

        protected override void OnIsKeyboardFocusWithinChanged(DependencyPropertyChangedEventArgs e)
        {
            if (Equals(e.NewValue, false))
            {
                // this is needed because the inner textbox gets focus
                this.RaiseEvent(new RoutedEventArgs(LostFocusEvent));
            }

            base.OnIsKeyboardFocusWithinChanged(e);
        }
    }
}
