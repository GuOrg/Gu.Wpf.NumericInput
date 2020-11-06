namespace Gu.Wpf.NumericInput.Touch
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    /// <summary>
    /// Helper class for showing the windows on screen keyboard.
    /// </summary>
    public static class TouchKeyboard
    {
        private static TouchProcessInfo? touchProcessInfo = TouchProcessInfo.Default;
        private static Process? process;

        /// <summary>
        /// Gets or sets the path to the exe.
        /// </summary>
        public static string? TouchKeyboardPath
        {
            get => touchProcessInfo?.ProcessStartInfo.FileName;
            set => touchProcessInfo = TouchProcessInfo.Create(value);
        }

        /// <summary>
        /// Show the on screen keyboard.
        /// </summary>
        public static void Show()
        {
            if (touchProcessInfo?.ProcessStartInfo is null)
            {
                return;
            }

            process?.Dispose();
            process = Process.Start(touchProcessInfo.ProcessStartInfo);
        }

        /// <summary>
        /// Hide the on screen keyboard.
        /// </summary>
        public static void Hide()
        {
            if (touchProcessInfo?.ProcessStartInfo is null)
            {
                return;
            }

            // http://mheironimus.blogspot.se/2015/05/adding-touch-keyboard-support-to-wpf.html
            var keyboardWnd = NativeMethods.FindWindow("IPTip_Main_Window", null);
            var nullIntPtr = new IntPtr(0);
            const uint wmSysCommand = 0x0112;
            var scClose = new IntPtr(0xF060);

            if (keyboardWnd != nullIntPtr)
            {
                _ = NativeMethods.SendMessage(keyboardWnd, wmSysCommand, scClose, nullIntPtr);
            }

            process?.Dispose();
        }

        ////private static bool HasTouchInput()
        ////{
        ////    return Tablet.TabletDevices.Cast<TabletDevice>().Any(tabletDevice => tabletDevice.Type == TabletDeviceType.Touch);
        ////}

        private static class NativeMethods
        {
            [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern IntPtr FindWindow(string sClassName, string? sAppName);

            [DefaultDllImportSearchPaths(DllImportSearchPath.System32)]
            [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        }
    }
}
