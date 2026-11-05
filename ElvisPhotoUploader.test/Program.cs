using System.Diagnostics;
using System.Text;
using System.Windows.Automation;
using ElvisPhotoUploader.test;


IntPtr matchedElvisWindowHandle = IntPtr.Zero;
IntPtr uploadModalHandle = IntPtr.Zero;
IntPtr ieServerHandle = IntPtr.Zero;   

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
        Console.WriteLine($"Found the Internet Explorer_Server child window with handle (HWND): {ieServerHandle}");

        AutomationElement browserElement = AutomationElement.FromHandle(ieServerHandle);

        if (browserElement == null)
        {
            Console.WriteLine("No UI Automation Element was returned");
        }
        else
        {
            Console.WriteLine("UI Automation Element was found!");
            Console.WriteLine($"Element name: {browserElement.Current.Name}");
            Console.WriteLine($"Control type: {browserElement.Current.ControlType.ProgrammaticName}");

            AutomationElementCollection children = browserElement.FindAll(TreeScope.Children, Condition.TrueCondition);
            AutomationElement rawFirstChild = TreeWalker.RawViewWalker.GetFirstChild(browserElement);

            Console.WriteLine($"Found {children.Count} child elements!");

            foreach (AutomationElement child in children)
            {
                Console.WriteLine($"Element name: {child.Current.Name}");
                Console.WriteLine($"Control type: {child.Current.ControlType.ProgrammaticName}");   
            }
            if (rawFirstChild == null)
            {
                Console.WriteLine("NULL");
            }
            
        }
    }

    Console.ReadLine();
}


