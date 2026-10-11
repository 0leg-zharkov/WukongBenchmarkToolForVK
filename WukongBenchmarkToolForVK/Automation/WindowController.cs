using System.Diagnostics;
using System.Runtime.InteropServices;

namespace WukongBenchmarkToolForVK.Automation;

public static class WindowController
{
    public static void HideConsole() => ShowConsole(Win32.SW_HIDE);
    public static void RestoreConsole() => ShowConsole(Win32.SW_RESTORE);

    private static void ShowConsole(int cmd)
    {
        var h = Win32.GetConsoleWindow();
        if (h != IntPtr.Zero) _ = Win32.ShowWindow(h, cmd);
    }

    public static void Focus(Process p)
    {
        try
        {
            p.Refresh();
            var h = p.MainWindowHandle;
            if (h == IntPtr.Zero) return;
            if (Win32.IsIconic(h)) _ = Win32.ShowWindow(h, Win32.SW_RESTORE);
            _ = Win32.SetForegroundWindow(h);
        }
        catch { }
    }

    public static void ClickCenter(Process p) => ClickAt(p, 0.5, 0.5);

    public static void ClickAt(Process p, double relX, double relY)
    {
        try
        {
            p.Refresh();
            var h = p.MainWindowHandle;
            if (h == IntPtr.Zero) return;

            if (Win32.IsIconic(h)) _ = Win32.ShowWindow(h, Win32.SW_RESTORE);
            _ = Win32.SetForegroundWindow(h);
            Thread.Sleep(500);

            if (!Win32.GetWindowRect(h, out var rect)) return;

            int w = rect.Right - rect.Left;
            int hh = rect.Bottom - rect.Top;
            int x = rect.Left + (int)(w * relX);
            int y = rect.Top + (int)(hh * relY);

            _ = Win32.SetCursorPos(x, y);
            Thread.Sleep(200);

            var inputs = new Win32.INPUT[2];
            inputs[0].type = Win32.INPUT_MOUSE;
            inputs[0].U.mi.dwFlags = Win32.MOUSEEVENTF_LEFTDOWN;
            inputs[1].type = Win32.INPUT_MOUSE;
            inputs[1].U.mi.dwFlags = Win32.MOUSEEVENTF_LEFTUP;
            _ = Win32.SendInput(2, inputs, Marshal.SizeOf<Win32.INPUT>());
        }
        catch { }
    }
}