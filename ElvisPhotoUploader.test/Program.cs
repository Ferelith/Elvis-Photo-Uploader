using Accessibility;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using ElvisPhotoUploader.test;

const uint OBJID_CLIENT = 0xFFFFFFFC;

const int CHILDID_SELF = 0;
const int NAVDIR_FIRSTCHILD = 7;

Guid iidIAccessible = typeof(IAccessible).GUID;

IntPtr matchedElvisWindowHandle = IntPtr.Zero;
IntPtr uploadModalHandle = IntPtr.Zero;
IntPtr ieServerHandle = IntPtr.Zero;   
IntPtr msaaPointer = IntPtr.Zero;

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

        if (charactersCopied > 0 && childClassName.Equals("Internet Explorer_Server", StringComparison.OrdinalIgnoreCase))
        {
            ieServerHandle = childHWnd;
            Console.WriteLine($"Found a child window with class name: {childClassName} and HWND: {childHWnd}");
        }
        else if (charactersCopied == 0)
        {
            Console.WriteLine($"Could not get the class name for child window with HWND: {childHWnd}");
        }
        else
        {
            Console.WriteLine($"Child window with HWND: {childHWnd} has class name: {childClassName}, which does not match 'Internet Explorer_Server'.");
        }

        return true;
    }, IntPtr.Zero);

    if (ieServerHandle == IntPtr.Zero)
    {
        Console.WriteLine("No Internet Explorer_Server child window was found.");
    }
    else
    {
        try
        {
            int hresult = NativeWindows.AccessibleObjectFromWindow(ieServerHandle, OBJID_CLIENT, ref iidIAccessible, out msaaPointer);

            if (hresult == 0 && msaaPointer != IntPtr.Zero)
            {
                Console.WriteLine("MSAA IAccessible Interface retrieved successfully!");
                Console.WriteLine($"HRESULT: 0x{hresult:X8}");

                IAccessible accessibleObject = (IAccessible) Marshal.GetObjectForIUnknown( msaaPointer );

                Console.WriteLine($"Get type: {accessibleObject.GetType()}");

                int childCount = accessibleObject.accChildCount;

                Console.WriteLine($"Accessible children: {childCount}");
                Console.WriteLine($"Accessible object name: {accessibleObject.accName[CHILDID_SELF]}");

                object firstChild = accessibleObject.accNavigate(NAVDIR_FIRSTCHILD, CHILDID_SELF);

                if (firstChild is IAccessible accessibleChild)
                {
                    Console.WriteLine($"Child name:{accessibleChild.accName[CHILDID_SELF]}");
                    Console.WriteLine($"Child count: {accessibleChild.accChildCount}");
                }
                else if (firstChild is int childId)
                {
                    Console.WriteLine($"MSAA returned child ID {childId}");
                }
                else
                {
                    Console.WriteLine("No accessible child object was returned.");
                }
            
            }
            else
            {
                Console.WriteLine("Failed to retrieve MSAA Interface");
                Console.WriteLine($"HRESULT: 0x{hresult:X8}");
            }    
        }
        catch (Exception e)
        {
            Console.WriteLine($"Failed to retrieve MSAA Interface due to an error: {e}");
        }
        finally
        {
            if (msaaPointer != IntPtr.Zero)
            {
                Marshal.Release(msaaPointer);
            }
        }
    }
}
Console.ReadLine();

