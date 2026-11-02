using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ElvisPhotoUploader.test
{
    internal static class NativeWindows
    {
        internal delegate bool WindowCallback(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        internal static extern bool EnumWindows(WindowCallback callback, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetWindowTextW", ExactSpelling = true)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder stringBuilder, int maxCount);
    }
}
