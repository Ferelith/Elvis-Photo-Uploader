using System.Diagnostics;
using System.Net.NetworkInformation;

// Create a variable to hold the matching window's handle (HWND)
IntPtr matchedElvisWindowHandle = IntPtr.Zero;

Process[] edgeProcess = Process.GetProcessesByName("msedge");

Console.WriteLine("Elvis Photo Uploader Test is ready");
// Display the number of msedge processes running
Console.WriteLine("There are currently " + edgeProcess.Length + " msedge processes running.\n");
Console.WriteLine($"Here are the details of the {edgeProcess.Length} msedge processes:\n");
// Loop through the msedge processes and print their details
foreach (Process process in edgeProcess)
{
    // Print the process name, ID, and main window title
    Console.WriteLine($"Process Name: {process.ProcessName}, Process ID: {process.Id}, Process Main Window Title: {(string.IsNullOrEmpty(process.MainWindowTitle) ? "N/A" : process.MainWindowTitle)}");
}

// Loop through the msedge processes again to find a matching window title
foreach (Process process in edgeProcess)
{
    process.Refresh(); // Refresh the process information to get the latest data
    if (process.MainWindowTitle.Contains("Elvis Vehicle Recovery Edition", StringComparison.OrdinalIgnoreCase) && process.MainWindowHandle != IntPtr.Zero) 
    {
        matchedElvisWindowHandle = process.MainWindowHandle; // Capture the matching window's handle (HWND)
        Console.WriteLine("\nA matching Elvis Vehicle Recovery Edition Window is running.");
        Console.WriteLine($"The window handle (HWND) is: {matchedElvisWindowHandle}");
        break;
    }

}
// If no matching window was found, inform the user
if (matchedElvisWindowHandle == IntPtr.Zero)
{
    Console.WriteLine("\nNo matching Elvis Vehicle Recovery Edition Window is running.");
}

Console.WriteLine("Press Enter to exit...");
Console.ReadLine();


