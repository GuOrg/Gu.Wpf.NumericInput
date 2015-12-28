namespace Gu.Wpf.NumericInput
{
    using System.Runtime.CompilerServices;
    using System.Windows;

    public static partial class NumericBox
    {
        public static ResourceKey IncreaseGeometryKey { get; } = CreateKey();

        public static ResourceKey DecreaseGeometryKey { get; } = CreateKey();

        public static ResourceKey SpinnerButtonStyleKey { get; } = CreateKey();

        public static ResourceKey SpinnerPathStyleKey { get; } = CreateKey();

        public static ResourceKey TextBoxStyleKey { get; } = CreateKey();

        public static ResourceKey BaseBoxStyleKey { get; } = CreateKey();

        public static ResourceKey SuffixTextBoxStyleKey { get; } = CreateKey();

        public static ResourceKey ValidationErrorListTemplateKey { get; } = CreateKey();

        public static ResourceKey SimpleValidationErrorTemplateKey { get; } = CreateKey();

        public static ResourceKey WithSpinnersTemplateKey { get; } = CreateKey();

        public static ResourceKey WithSuffixTemplateKey { get; } = CreateKey();

        public static ResourceKey WithSpinnersAndSuffixTemplateKey { get; } = CreateKey();

        private static ComponentResourceKey CreateKey([CallerMemberName] string caller = null)
        {
            return new ComponentResourceKey(typeof(NumericBox), caller);
        }
    }
}
