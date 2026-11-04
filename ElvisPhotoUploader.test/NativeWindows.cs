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
        // Import the EnumWindows function from user32.dll to enumerate all top-level windows on the screen
        internal static extern bool EnumWindows(WindowCallback callback, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        // Import the EnumChildWindows function from user32.dll to enumerate child windows of a specified parent window
        internal static extern bool EnumChildWindows(IntPtr hWndParent, WindowCallback callback, IntPtr lParam);

        // Import the GetClassName function from user32.dll to retrieve the class name of a window
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetClassNameW", ExactSpelling = true)]
        internal static extern int GetClassName(IntPtr hWnd, StringBuilder className, int maxCount);

        // Import the GetWindowText function from user32.dll
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetWindowTextW", ExactSpelling = true)]
        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder stringBuilder, int maxCount);
    }
}
