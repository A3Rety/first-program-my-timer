namespace MyTimer.Misc;

internal static class Resources
{
    // ----------   COLOR   ---------- //
    internal static ConsoleColor GetRandomColor()
    {
        return Colors[Random.Shared.Next(Colors.Length)];
    }

    internal static readonly ConsoleColor[] Colors =
    [
            ConsoleColor.Red, ConsoleColor.Yellow, ConsoleColor.Green,
                ConsoleColor.Cyan, ConsoleColor.Magenta, ConsoleColor.Blue,
                ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.DarkCyan,
                ConsoleColor.DarkGray, ConsoleColor.DarkGreen, ConsoleColor.DarkMagenta,
                ConsoleColor.DarkRed, ConsoleColor.DarkYellow
    ];

}
