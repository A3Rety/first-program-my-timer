using MyTimer.Base;
using MyTimer.Misc;
using MyTimer.UI;

namespace MyTimer.Architecture;

internal static class Logic
{
    private const int _notDiffValue = -1;


    internal static bool InputLogic(out int time, ref bool isExit)
    {
        WriteTextHere(text: "Minutes: " + new string(' ', 230), left: 0, top: 0);

        while (Console.KeyAvailable)
        {
            Console.ReadKey(intercept: true);
        }

        Console.ForegroundColor = Resources.GetRandomColor(); Console.SetCursorPosition(9, 0);
        string input = Console.ReadLine() ?? "";

        if (input == "q")
        {
            isExit = true;
        }

        if (!int.TryParse(input, out time)) return false;
        if (time <= -1 || time >= 256) return false;

        return true;
    }

    internal static void AddListValue(byte time)
    {
        if (time > 15)
        {
            Data.LONG++; Data.worksString = "(+1)"; Data.breaksString = "";

            Data.LONGlist.Insert(0, time);

            if (Data.LONGlist.Count > 15)
            {
                Data.LONGlist.RemoveAt(Data.LONGlist.Count - 1);
                Data.LONGlist.Capacity = 15;
            }
        }
        else
        {
            Data.SHORT++; Data.worksString = ""; Data.breaksString = "(+1)";

            Data.SHORTlist.Insert(0, time);

            if (Data.SHORTlist.Count > 15)
            {
                Data.SHORTlist.RemoveAt(Data.SHORTlist.Count - 1);
                Data.SHORTlist.Capacity = 15;
            }
        }
    }


    // ----------   TIMESPENT   ---------- //
    internal static void CalculateTotalTimeSpent(in int time, in int diffTime = _notDiffValue)
    {
        int addValue;
        if (diffTime > _notDiffValue)
        {
            addValue = diffTime;
        }
        else
        {
            addValue = time;
        }


        if (time > 15) // long | work
        {
            Data.TotalWorksSpent += addValue;
        }
        else // short | break
        {
            Data.TotalBreaksSpent += addValue;
        }
    }

    internal static void PrintError()
    {
        WriteTextHere("ERROR!", 8, 15, ConsoleColor.Red);
    }

}
