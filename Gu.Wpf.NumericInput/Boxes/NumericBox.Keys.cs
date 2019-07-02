namespace Gu.Wpf.NumericInput
{
    using System.Runtime.CompilerServices;
    using System.Windows;

    /// <summary>
    /// Resource keys for <see cref="NumericBox"/>.
    /// </summary>
    public static partial class NumericBox
    {
        public static ResourceKey IncreaseGeometryKey { get; } = CreateKey();

        public static ResourceKey DecreaseGeometryKey { get; } = CreateKey();

        public static ResourceKey SpinnerButtonStyleKey { get; } = CreateKey();

        public static ResourceKey SpinnerPathStyleKey { get; } = CreateKey();

        public static ResourceKey ValidationErrorRedBorderTemplateKey { get; } = CreateKey();

        public static ResourceKey ValidationErrorTextUnderTemplateKey { get; } = CreateKey();

        public static ResourceKey ValidationErrorListTemplateKey { get; } = CreateKey();

        public static ResourceKey SimpleValidationErrorTemplateKey { get; } = CreateKey();

        public static ResourceKey SpinnersTemplateKey { get; } = CreateKey();

        private static ComponentResourceKey CreateKey([CallerMemberName] string caller = null)
        {
            // ReSharper disable once AssignNullToNotNullAttribute
            return new ComponentResourceKey(typeof(NumericBox), caller);
        }
    }
}
