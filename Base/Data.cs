namespace MyTimer.Base;

internal static class Data
{
    internal static int TotalBreaksSpent = 0;
    internal static int TotalWorksSpent = 0;
    internal static int TotalTimeSpent => TotalBreaksSpent + TotalWorksSpent;

    internal static readonly System.Collections.Generic.List<byte> LONGlist = new(16);
    internal static readonly System.Collections.Generic.List<byte> SHORTlist = new(16);

    internal static byte LONG = 0;
    internal static byte SHORT = 0;

    internal static string? breaksString;
    internal static string? worksString;
    internal static byte arrow;


    internal static void Init()
    {
        Console.Title = "❤️🌟⭐️💫💖";
        Console.SetWindowSize(120, 30);
        Console.SetBufferSize(120, 30);
        Console.Clear();
    }
}