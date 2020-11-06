namespace Gu.Wpf.NumericInput
{
    using System.Windows.Input;

    /// <summary>
    /// For <see cref="SpinnerDecorator"/>.
    /// </summary>
    public interface ISpinnerBox
    {
        /// <summary>
        /// Gets the command that increases the value.
        /// </summary>
        ICommand? IncreaseCommand { get; }

        /// <summary>
        /// Gets the command that decreases the value.
        /// </summary>
        ICommand? DecreaseCommand { get; }

        /// <summary>
        /// Gets a value indicating whether buttons should be visible.
        /// </summary>
        bool AllowSpinners { get; }
    }
}
