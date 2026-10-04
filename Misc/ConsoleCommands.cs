using System.Runtime.InteropServices;

namespace MyTimer.Misc;

internal static class ConsoleCommands
{
    [DllImport("kernel32.dll")] private static extern IntPtr GetConsoleWindow();
    [DllImport("user32.dll")] private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("kernel32.dll")] private static extern IntPtr GetStdHandle(int n);
    [DllImport("kernel32.dll")] private static extern bool GetConsoleMode(IntPtr h, out uint m);
    [DllImport("kernel32.dll")] private static extern bool SetConsoleMode(IntPtr h, uint m);

    private const int STD_INPUT_HANDLE = -10;
    private const uint QUICK_EDIT = 0x0040;


    // ----------   MUSIC   ---------- //
    internal static void PlayMusic()
    {
        // ♩ ♪ ♪ ♪ | ♩ ♪ ♪ ♪ | финал (медленный диско)
        Console.Beep(262, 300); System.Threading.Thread.Sleep(40);  // тун
        Console.Beep(294, 250); System.Threading.Thread.Sleep(40);  // ту  
        Console.Beep(330, 250); System.Threading.Thread.Sleep(80);  // ту

        Console.Beep(262, 300); System.Threading.Thread.Sleep(40);  // тун
        Console.Beep(294, 250); System.Threading.Thread.Sleep(40);  // ту
        Console.Beep(330, 250); System.Threading.Thread.Sleep(80);  // ту

        Console.Beep(262, 300); System.Threading.Thread.Sleep(40);  // тун
        Console.Beep(262, 300); System.Threading.Thread.Sleep(40);  // тун
        Console.Beep(220, 400); System.Threading.Thread.Sleep(100); // тин (завершение)
    }

    internal static void PlaySound()
    {
        Console.Beep(330, 150);
    }

    // ----------   OPEN   ---------- //
    internal static void OpenConsole()
    {
        ShowWindow(GetConsoleWindow(), 9);
    }

    internal static void DisabelQuickEditConsoleMode()
    {
        var h = GetStdHandle(STD_INPUT_HANDLE);
        GetConsoleMode(h, out uint m);
        SetConsoleMode(h, m & ~QUICK_EDIT);
    }

}
