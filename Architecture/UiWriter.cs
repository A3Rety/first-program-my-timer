using MyTimer.Misc;

namespace MyTimer.Architecture;

internal static class UiWriter
{
    private const int _dontMove = -1;

    // ----------   DRAW TEXT HERE!!!   ----------//
    internal static void WriteTextHere(string text, int left = _dontMove, int top = _dontMove)
    {
        Console.ForegroundColor = Resources.GetRandomColor();

        SetPosition(left, top);
        Console.Write(text);
    }

    internal static void WriteTextHere(string text, int left = _dontMove, int top = _dontMove, bool randomColorPerChar = false)
    {
        if (randomColorPerChar == false)
        {
            WriteTextHere(text, left, top);
            return;
        }

        SetPosition(left, top);
        for (int i = 0; i < text.Length; i++)
        {
            Console.ForegroundColor = Resources.GetRandomColor(); Console.Write(text[i]);
        }
    }

    internal static void WriteTextHere(string text, int left = _dontMove, int top = _dontMove, ConsoleColor color = 0)
    {
        if (color != 0)
        {
            Console.ForegroundColor = color;
        }

        SetPosition(left, top);
        Console.Write(text);
    }

    private static void SetPosition(int left, int top)
    {
        if (left >= 0 && top >= 0)
            Console.SetCursorPosition(left, top);
    }
    
}