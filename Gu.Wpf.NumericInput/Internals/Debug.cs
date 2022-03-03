// ReSharper disable ExplicitCallerInfoArgument
namespace Gu.Wpf.NumericInput
{
    using System.Runtime.CompilerServices;
    using System.Windows;

    internal static class Debug
    {
        [System.Diagnostics.Conditional("DEBUG")]
        internal static void WriteLine(string message, [CallerMemberName] string? caller = null)
        {
            System.Diagnostics.Debug.WriteLine($"{caller}: {message}");
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void WriteLine(DependencyPropertyChangedEventArgs args, [CallerMemberName] string? caller = null)
        {
            WriteLine(args.OldValue, args.NewValue, caller);
        }

        [System.Diagnostics.Conditional("DEBUG")]
        internal static void WriteLine(object from, object to, [CallerMemberName] string? caller = null)
        {
            System.Diagnostics.Debug.WriteLine($"{caller}: From: {from.Formatted()} To: {to.Formatted()}");
        }

        private static string Formatted(this object o)
        {
            return o switch
            {
                null => "null",
                string text when string.IsNullOrWhiteSpace(text) => "string.Empty",
                string text => $"\"{text}\"",
                _ => o.ToString() ?? "(null)",
            };
        }
    }
}
