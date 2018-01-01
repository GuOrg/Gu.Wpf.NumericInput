namespace Gu.Wpf.NumericInput.UITests
{
    using System.Diagnostics;

    public static class Info
    {
        internal static ProcessStartInfo CreateStartInfo(string args)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "Gu.Wpf.NumericInput.Demo.exe",
                Arguments = args,
                UseShellExecute = false,
            };
            return processStartInfo;
        }
    }
}