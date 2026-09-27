using System.Threading;
using System.Threading.Tasks;
using MyTimer.Architecture;
using MyTimer.Base;
using MyTimer.UI;

namespace MyTimer;

// TO DO!
// Добавить очищение экрана и перерисовку каждые 5 итераций
// поменять вывод времени (на Е) с локального на тоталТаймер
// продебажить что на(вайб)кодил в пятницу ночью 
// XDDD
//

internal static class Program
{
    internal static async Task Main()
    {
        Data.Init();

        Gui.StartScreen();
        bool isExit = false;

        while (!isExit)
        {
            if (Logic.InputLogic(out int time, ref isExit) == false)
                continue;
            Logic.AddListValue((byte)time);

            Gui.DrawTotalRepeats();
            Gui.DrawHistoryList();


            var cts = new CancellationTokenSource();
            var token = cts.Token;
            AppTimer.TimerAwait(time, cts);

            try
            {
                await Task.Delay(Timeout.Infinite, token);
            }
            catch (OperationCanceledException) { }
            catch { Logic.PrintError(); }
        }
    }
}