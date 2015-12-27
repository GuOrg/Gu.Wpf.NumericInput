namespace Gu.Wpf.NumericInput.UITests
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using Gu.Wpf.NumericInput.Demo;

    public static class Info
    {
        public static ProcessStartInfo ProcessStartInfo
        {
            get
            {
                var assembly = typeof(MainWindow).Assembly;
                var uri = new Uri(assembly.CodeBase, UriKind.Absolute);
                var fileName = uri.AbsolutePath;
                var workingDirectory = System.IO.Path.GetDirectoryName(fileName);
                var processStartInfo = new ProcessStartInfo
                {
                    WorkingDirectory =  workingDirectory,
                    FileName = fileName,
                    //UseShellExecute = false,
                    //CreateNoWindow = false,
                    //RedirectStandardOutput = true,
                    //RedirectStandardError = true
                };
                return processStartInfo;
            }
        }
    }
}