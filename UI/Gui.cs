using MyTimer.Base;

namespace MyTimer.UI;

internal static class Gui
{
    internal static void StartScreen()
    {
        WriteTextHere(text: "q - exit    e - time", left: 8, top: 25, color: 0);
        WriteTextHere(text: "r - stop", left: 20, top: 26, color: 0);
        WriteTextHere(text: "space - pause/resume", left: 8, top: 28, color: 0);
        Gui.DrawBorder();
    }

    internal static void DrawTotalTimeSpent()
    {
        WriteTextHere(text: "TIME SPENT", left: 91, top: 14, color: ConsoleColor.Gray);
        WriteTextHere(text: "Breaks - ", left: 85, top: 15, color: ConsoleColor.Yellow);
        WriteTextHere(text: "Works - ", left: 91, top: 16, color: 0);
        WriteTextHere(text: "Total - ", left: 96, top: 17, color: 0);

        WriteTextHere(text: $"{Data.TotalBreaksSpent}", left: 94, top: 15);
        WriteTextHere(text: $"{Data.TotalWorksSpent}", left: 99, top: 16);
        WriteTextHere(text: $"{Data.TotalTimeSpent}", left: 104, top: 17);
    }

    internal static void DrawTotalRepeats()
    {
        WriteTextHere(text: "TIMES", left: 29, top: 2, color: ConsoleColor.Gray);

        WriteTextHere(text: "Breaks (1-15): ", left: 10, top: 3, color: ConsoleColor.Yellow);
        WriteTextHere(text: $"{Data.SHORT + Data.breaksString}", randomColorPerChar: false);
        WriteTextHere(text: " / Works (15+): ", color: ConsoleColor.Yellow);
        WriteTextHere(text: $"{Data.LONG + Data.worksString}", randomColorPerChar: false);
    }

    internal static void DrawHistoryList()
    {
        WriteTextHere(text: "HISTORY", left: 52, top: 9, color: ConsoleColor.Gray);

        WriteTextHere(text: "Breaks: ", left: 43, top: 10, color: ConsoleColor.Yellow);
        WriteTextHere(text: "|", left: 55, top: 10, color: 0);
        WriteTextHere(text: "Works: ", left: 58, top: 10, color: 0);

        WriteTextHere(text: "[]", left: 51, top: 25, color: 0);
        WriteTextHere(text: "[]", left: 65, top: 25, color: 0);


        for (int i = 0; i < Data.SHORTlist.Count; i++)
        {
            WriteTextHere(text: "   ", left: 51, top: 10 + i, color: 0);
            WriteTextHere(text: $"{Data.SHORTlist[i]}", left: 51, top: 10 + i);
        }

        for (int k = 0; k < Data.LONGlist.Count; k++)
        {
            WriteTextHere(text: "   ", left: 65, top: 10 + k, color: 0);
            WriteTextHere(text: $"{Data.LONGlist[k]}", left: 65, top: 10 + k);
        }

    }


    // ----------   DRAW   ---------- //
    internal static void DrawBorder()
    {
        Console.ForegroundColor = ConsoleColor.Yellow; // yellow

        for (byte i = 83; i <= 108; i++)
        {
            WriteTextHere(text: "-", left: i, top: 13, color: 0);
            WriteTextHere(text: "-", left: i, top: 18, color: 0);
        }
        for (byte i = 14; i <= 17; i++) // -1
        {
            WriteTextHere(text: "|", left: 83, top: i, color: 0);
            WriteTextHere(text: "|", left: 108, top: i, color: 0);
        }

        WriteTextHere(text: "+", left: 83, top: 13, color: 0);
        WriteTextHere(text: "+", left: 108, top: 13, color: 0);
        WriteTextHere(text: "+", left: 83, top: 18, color: 0);
        WriteTextHere(text: "+", left: 108, top: 18, color: 0);
        //Console.WriteLine("═══════════════════");
    }

    // ----------   DONE   ---------- //
    internal static void Done()
    {
        WriteTextHere(text: "DONE!        ", left: 10, top: 5, randomColorPerChar: true);
    }

    // ----------   CANCELED   ---------- //
    internal static void Canceled()
    {
        WriteTextHere(text: "CANCELED!        ", left: 10, top: 5, randomColorPerChar: true);
    }

    // ----------   WORK   ---------- //
    internal static void Work()
    {
        WriteTextHere(text: "PROCESSING...", left: 10, top: 5, randomColorPerChar: true);
    }

}
