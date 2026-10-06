using System.Threading;
using System.Threading.Tasks;
using MyTimer.Architecture;
using MyTimer.Base;
using MyTimer.UI;

namespace MyTimer;


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
            AppTimer timer = new AppTimer(time, cts);

            try
            {
                await Task.Delay(Timeout.Infinite, token);
            }
            catch (OperationCanceledException) { }
            catch { Logic.PrintError(); }

            timer.Dispose();
        }
        
    }
}