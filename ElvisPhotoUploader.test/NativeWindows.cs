using System;
using System.Runtime.InteropServices;
using System.Text;

namespace ElvisPhotoUploader.test
{
    internal static class NativeWindows
    {
        // Define a delegate type for the callback function used in EnumWindows and EnumChildWindows
        internal delegate bool WindowCallback(IntPtr hWnd, IntPtr lParam);
        // Import the necessary functions from user32.dll for window enumeration and text retrieval
        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]

        // Import the EnumChildWindows function from user32.dll
        internal static extern bool EnumChildWindows(IntPtr hWndParent, WindowCallback callback, IntPtr lParam);
        
        // Import the EnumWindows function from user32.dll
        internal static extern bool EnumWindows(WindowCallback callback, IntPtr lParam);

        // Import the GetWindowText function from user32.dll to retrieve the title of a window
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode, EntryPoint = "GetWindowTextW", ExactSpelling = true)]

        internal static extern int GetWindowText(IntPtr hWnd, StringBuilder stringBuilder, int maxCount);
    }
}
