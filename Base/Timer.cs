using System.Diagnostics;
using MyTimer.Architecture;
using MyTimer.Misc;
using MyTimer.UI;

namespace MyTimer.Base;


internal static class AppTimer
{
    private static CancellationTokenSource? _cts;

    private static volatile bool _stopListener = true;
    private static readonly Stopwatch _totalStopWatch = new();
    private static readonly Stopwatch _pausableStopWatch = new();

    private static bool _isPaused = false;
    private static bool _isTaskRunning = false;

    private static int _userTime = -1;
    private static int _lastTime = -1;
    private static int _lastElapsedTimeMinutes = -1;
    private static int _lastElapsedTimeSeconds = -1;

    private static CancellationTokenSource _userCts;


    internal static void TimerAwait(int time, CancellationTokenSource cts)
    {
        Gui.Work();
        _userCts = cts;
        _userTime = time;
        _totalStopWatch.Restart();
        TimerStart(time, 0, false);
    }

    internal static void TimerStart(int time, int seconds, bool quit = false)
    {
        CancellationToken token = AppTimer.StartNewTimerSession();

        TimerProcess(time, seconds, token, quit);
    }

    // ----------   TIMER   ---------- //
    private static async Task TimerProcess(int time, int seconds, CancellationToken token, bool quit = false)
    {
        _pausableStopWatch.Restart();
        _lastTime = time;
        _lastElapsedTimeMinutes = TimeSpan.Zero.Milliseconds;
        _lastElapsedTimeSeconds = TimeSpan.Zero.Milliseconds;

        int oneMinuteInMS = 60000;
        int oneSecondInMs = 1000;


        if (quit)
        {
            _totalStopWatch.Stop();
            int roundedDelta = (int)Math.Round(_totalStopWatch.Elapsed.TotalMinutes,
                                                MidpointRounding.AwayFromZero);
            if (_userTime > 15)
            {
                Data.LONGlist[0] = (byte)roundedDelta;
            }
            else
            {
                Data.SHORTlist[0] = (byte)roundedDelta;
            }

            Misc.ConsoleCommands.PlaySound();
            Logic.CalculateTotalTimeSpent(in _userTime, roundedDelta);
            Gui.DrawTotalTimeSpent();
            Gui.DrawHistoryList();
            Gui.Canceled();

            _stopListener = true;
            _isTaskRunning = false;
            _totalStopWatch.Reset();
            _pausableStopWatch.Reset();
            WriteTextHere(text: "                ", left: 20, top: 23);
            _userCts.Cancel();

            return;
        }


        try
        {
            await Task.Delay((time * oneMinuteInMS) - (seconds * oneSecondInMs), token);

            _totalStopWatch.Stop();
            ConsoleCommands.PlayMusic();
            ConsoleCommands.OpenConsole();
            Logic.CalculateTotalTimeSpent(in time);
            Gui.DrawTotalTimeSpent();
            Gui.Done();
        }
        catch (OperationCanceledException)
        {
            if (_isPaused == true)
                return;

            _totalStopWatch.Stop();
            int roundedDelta = (int)Math.Round(_totalStopWatch.Elapsed.TotalMinutes,
                                                MidpointRounding.AwayFromZero);
            if (_userTime > 15)
            {
                Data.LONGlist[0] = (byte)roundedDelta;
            }
            else
            {
                Data.SHORTlist[0] = (byte)roundedDelta;
            }

            Misc.ConsoleCommands.PlaySound();
            Logic.CalculateTotalTimeSpent(in _userTime, roundedDelta);
            Gui.DrawTotalTimeSpent();
            Gui.DrawHistoryList();
            Gui.Canceled();
        }
        catch { Logic.PrintError(); }

        _stopListener = true;
        _isTaskRunning = false;
        _totalStopWatch.Reset();
        _pausableStopWatch.Reset();
        WriteTextHere(text: "                ", left: 20, top: 23);
        _userCts.Cancel();
    }

    private static void TimerPause()
    {
        if (!CheckCorrectness())
            return;

        _isPaused = true;

        {
            _cts?.Cancel();
            _totalStopWatch.Stop();
            _pausableStopWatch.Stop();
            _lastElapsedTimeMinutes = (int)_pausableStopWatch.Elapsed.TotalMinutes;
            _lastElapsedTimeSeconds = (int)_pausableStopWatch.Elapsed.Seconds;
        }
    }

    private static void TimerUnpause(bool quit = false)
    {
        if (!CheckCorrectness())
            return;

        _isPaused = false;

        {
            _totalStopWatch.Start();

            _lastTime -= _lastElapsedTimeMinutes;
            if (_lastElapsedTimeMinutes > _lastTime)
                Logic.PrintError();

            TimerStart(_lastTime, _lastElapsedTimeSeconds, quit);
        }
    }

    private static bool CheckCorrectness()
    {
        if (_lastTime == -1 || _lastElapsedTimeMinutes == -1 || _lastElapsedTimeSeconds == -1)
        {
            Logic.PrintError();
            return false;
        }

        return true;
    }


    internal static CancellationToken StartNewTimerSession()
    {
        _stopListener = false;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        if (_isTaskRunning == false)
            StartNewRunTask();

        return _cts.Token;
    }

    private static void StartNewRunTask()
    {
        _isTaskRunning = true;

        _ = Task.Run(() =>
        {
            while (_stopListener == false)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(intercept: true);

                    if (key.Key == ConsoleKey.R)
                    {
                        if (_isPaused == false)
                        {
                            _cts?.Cancel();
                            _stopListener = true;
                            break;
                        }
                        else
                        {
                            TimerUnpause(true);
                            break;
                        }
                    }
                    else if (key.Key == ConsoleKey.E)
                    {
                        WriteTextHere(text: $"{_pausableStopWatch.Elapsed:hh\\:mm\\:ss}", left: 20, top: 23);
                        //total
                        continue;
                    }
                    else if (key.Key == ConsoleKey.Spacebar)
                    {
                        if (_isPaused == false)
                        {
                            WriteTextHere(text: "----------   PAUSED   ---------- ",
                                            left: 9, top: 0, color: ConsoleColor.Green);
                            TimerPause();
                        }
                        else if (_isPaused == true)
                        {
                            WriteTextHere(text: "++++++++++   RESUMED   ++++++++++",
                                            left: 9, top: 0, color: ConsoleColor.Red);
                            TimerUnpause();
                        }
                    }
                }
                Thread.Sleep(150);
            }

            _cts?.CancelAsync();
            _isPaused = false;
        }, _cts!.Token);
    }
}