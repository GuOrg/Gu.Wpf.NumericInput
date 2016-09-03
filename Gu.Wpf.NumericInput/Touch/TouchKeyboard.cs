namespace Gu.Wpf.NumericInput.Touch
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    public static class TouchKeyboard
    {
        private static TouchProcessInfo touchProcessInfo = TouchProcessInfo.Default;

        public static string TouchKeyboardPath
        {
            get { return touchProcessInfo?.ProcessStartInfo.FileName; }
            set { touchProcessInfo = TouchProcessInfo.Create(value); }
        }

        public static void Show()
        {
            if (touchProcessInfo?.ProcessStartInfo == null)
            {
                return;
            }

            NumericInput.Debug.WriteLine("show");
            Process.Start(touchProcessInfo.ProcessStartInfo);
        }

        public static void Hide()
        {
            if (touchProcessInfo?.ProcessStartInfo == null)
            {
                return;
            }

            // http://mheironimus.blogspot.se/2015/05/adding-touch-keyboard-support-to-wpf.html
            var keyboardWnd = NativeMethods.FindWindow("IPTip_Main_Window", null);
            var nullIntPtr = new IntPtr(0);
            const uint WmSyscommand = 0x0112;
            var scClose = new IntPtr(0xF060);

            if (keyboardWnd != nullIntPtr)
            {
                NumericInput.Debug.WriteLine("hide");
                NativeMethods.SendMessage(keyboardWnd, WmSyscommand, scClose, nullIntPtr);
            }
        }

        ////private static bool HasTouchInput()
        ////{
        ////    return Tablet.TabletDevices.Cast<TabletDevice>().Any(tabletDevice => tabletDevice.Type == TabletDeviceType.Touch);
        ////}

        private static class NativeMethods
        {
            [DllImport("user32.dll", CharSet = CharSet.Unicode)]
            internal static extern IntPtr FindWindow(string sClassName, string sAppName);

            [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
            internal static extern IntPtr SendMessage(IntPtr hWnd, uint uMsg, IntPtr wParam, IntPtr lParam);
        }
    }
}
