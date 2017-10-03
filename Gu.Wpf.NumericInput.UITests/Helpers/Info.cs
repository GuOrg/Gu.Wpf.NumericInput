namespace Gu.Wpf.NumericInput.UITests
{
    using System.Diagnostics;
    using Gu.Wpf.UiAutomation;

    public static class Info
    {
        internal static ProcessStartInfo CreateStartInfo(string args)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = Application.FindExe("Gu.Wpf.NumericInput.Demo.exe"),
                Arguments = args,
                UseShellExecute = false,
            };
            return processStartInfo;
        }
    }
}