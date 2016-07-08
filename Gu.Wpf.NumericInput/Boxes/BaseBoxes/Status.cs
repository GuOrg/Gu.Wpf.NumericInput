namespace Gu.Wpf.NumericInput
{
#pragma warning disable SA1602 // Enumeration items must be documented

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