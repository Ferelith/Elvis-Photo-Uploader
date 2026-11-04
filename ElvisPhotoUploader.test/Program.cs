using System.Diagnostics;
using System.Text;
using ElvisPhotoUploader.test;
using System.Net.NetworkInformation;

IntPtr matchedElvisWindowHandle = IntPtr.Zero;
IntPtr uploadModalHandle = IntPtr.Zero;

Process[] edgeProcess = Process.GetProcessesByName("msedge");

// Loop through the msedge processes again to find a matching window title
foreach (Process process in edgeProcess)
{
    process.Refresh(); 
    if (process.MainWindowTitle.Contains("Elvis Vehicle Recovery Edition", StringComparison.OrdinalIgnoreCase) && process.MainWindowHandle != IntPtr.Zero) 
    {
        matchedElvisWindowHandle = process.MainWindowHandle; 
        Console.WriteLine($"A matching Elvis Vehicle Recovery Edition Window is running with (HWND): {matchedElvisWindowHandle}");
        break;
    }
}
if (matchedElvisWindowHandle == IntPtr.Zero)
{
    Console.WriteLine("No matching Elvis Vehicle Recovery Edition Window is running.");
}
NativeWindows.EnumWindows((hWnd, lParam) =>
{
    StringBuilder titleBuffer = new StringBuilder(512);
    NativeWindows.GetWindowText(hWnd, titleBuffer, titleBuffer.Capacity);
    string windowTitle = titleBuffer.ToString();

    if (windowTitle.Contains("ELVIS DotNet", StringComparison.OrdinalIgnoreCase) && windowTitle.Contains("Webpage Dialog", StringComparison.OrdinalIgnoreCase))
    {
        uploadModalHandle = hWnd; 
    }
    return true; 
}, IntPtr.Zero);

if (uploadModalHandle == IntPtr.Zero)
{
    Console.WriteLine("No photo upload dialog window was found.");
}
else
{
    Console.WriteLine($"Found the upload photo modal window with handle (HWND): {uploadModalHandle}");

    NativeWindows.EnumChildWindows(uploadModalHandle, (childHWnd, lParam) =>
    {
        StringBuilder classTitleBuffer = new StringBuilder(256);
        int charactersCopied = NativeWindows.GetClassName(childHWnd, classTitleBuffer, classTitleBuffer.Capacity);
        string childClassName = classTitleBuffer.ToString();

        if (charactersCopied > 0)
        {
            Console.WriteLine($"Found a child window with class name: {childClassName} and HWND: {childHWnd}");
        }
        else
        {
            Console.WriteLine($"Could not get the class name for child window with HWND: {childHWnd}");
        }

        return true; 
    }, IntPtr.Zero);
}

Console.ReadLine();


