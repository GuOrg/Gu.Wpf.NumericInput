namespace Gu.Wpf.NumericInput
{
    using System.Windows.Input;

    public interface IIncrementBox
    {
        ICommand IncreaseCommand { get; }

        ICommand DecreaseCommand { get; }
    }
}