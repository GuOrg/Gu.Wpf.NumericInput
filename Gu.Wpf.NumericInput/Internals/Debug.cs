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
            if (o == null)
            {
                return "null";
            }

            if (o is string text)
            {
                if (string.IsNullOrWhiteSpace(text))
                {
                    return "string.Empty";
                }

                return $"\"{text}\"";
            }

            return o.ToString() ?? "null";
        }
    }
}
