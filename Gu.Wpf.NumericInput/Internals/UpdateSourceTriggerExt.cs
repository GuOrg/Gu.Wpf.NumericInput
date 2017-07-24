namespace Gu.Wpf.NumericInput
{
    using System.Windows.Data;

    internal static class UpdateSourceTriggerExt
    {
        internal static bool IsEither(this UpdateSourceTrigger self, UpdateSourceTrigger x, UpdateSourceTrigger y) => self == x || self == y;
    }
}