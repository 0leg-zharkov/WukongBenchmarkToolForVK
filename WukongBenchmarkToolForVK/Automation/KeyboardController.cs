using System.Runtime.InteropServices;

namespace WukongBenchmarkToolForVK.Automation;

public static class KeyboardController
{
    public const ushort VK_ESC = 0x01;
    public const ushort VK_TAB = 0x0F;
    public const ushort VK_ENTER = 0x1C;
    public const ushort VK_SPACE = 0x39;
    public const ushort VK_DOWN = 0x50;

    public static void SendScancode(ushort scan)
    {
        var inputs = new Win32.INPUT[2];

        inputs[0].type = Win32.INPUT_KEYBOARD;
        inputs[0].U.ki.wScan = scan;
        inputs[0].U.ki.dwFlags = Win32.KEYEVENTF_SCANCODE;

        inputs[1].type = Win32.INPUT_KEYBOARD;
        inputs[1].U.ki.wScan = scan;
        inputs[1].U.ki.dwFlags = Win32.KEYEVENTF_SCANCODE | Win32.KEYEVENTF_KEYUP;

        _ = Win32.SendInput((uint)inputs.Length, inputs, Marshal.SizeOf<Win32.INPUT>());
    }
}