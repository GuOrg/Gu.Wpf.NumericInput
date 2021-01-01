namespace Gu.Wpf.NumericInput
{
    internal enum Status
    {
        Idle,
        UpdatingFromUserInput,
        UpdatingFromValueBinding,
        Formatting,
        ResettingValue,
        Validating,
    }
}
