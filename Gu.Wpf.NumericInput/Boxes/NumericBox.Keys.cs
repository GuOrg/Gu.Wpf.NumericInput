namespace Gu.Wpf.NumericInput
{
    using System.Runtime.CompilerServices;
    using System.Windows;

    /// <summary>
    /// Resource keys for <see cref="NumericBox"/>.
    /// </summary>
    public static partial class NumericBox
    {
        /// <summary>Gets the <see cref="ResourceKey"/> for the increase spinner button geometry.</summary>
        public static ResourceKey IncreaseGeometryKey { get; } = CreateKey();

        /// <summary>Gets the <see cref="ResourceKey"/> for the decrease spinner button geometry.</summary>
        public static ResourceKey DecreaseGeometryKey { get; } = CreateKey();

        /// <summary>Gets the <see cref="ResourceKey"/> for the spinner button style.</summary>
        public static ResourceKey SpinnerButtonStyleKey { get; } = CreateKey();

        /// <summary>Gets the <see cref="ResourceKey"/> for the spinner button path style.</summary>
        public static ResourceKey SpinnerPathStyleKey { get; } = CreateKey();

        /// <summary>Gets the <see cref="ResourceKey"/> for the error border template.</summary>
        public static ResourceKey ValidationErrorRedBorderTemplateKey { get; } = CreateKey();

        /// <summary>Gets the <see cref="ResourceKey"/> for the error template.</summary>
        public static ResourceKey ValidationErrorTextUnderTemplateKey { get; } = CreateKey();

        /// <summary>Gets the <see cref="ResourceKey"/> for the list error template.</summary>
        public static ResourceKey ValidationErrorListTemplateKey { get; } = CreateKey();

        /// <summary>Gets the <see cref="ResourceKey"/> for the simple error template.</summary>
        public static ResourceKey SimpleValidationErrorTemplateKey { get; } = CreateKey();

        /// <summary>Gets the <see cref="ResourceKey"/> for the spinner button template.</summary>
        public static ResourceKey SpinnersTemplateKey { get; } = CreateKey();

        private static ComponentResourceKey CreateKey([CallerMemberName] string? caller = null)
        {
            return new ComponentResourceKey(typeof(NumericBox), caller);
        }
    }
}
