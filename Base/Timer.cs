using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using MyTimer.Architecture;
using MyTimer.Misc;
using MyTimer.UI;

namespace MyTimer.Base;


internal sealed class AppTimer : IDisposable
{
    private CancellationTokenSource? _cts;
    private CancellationTokenSource? _userCts;

    private readonly Task? _timeDisplayer;
    private readonly Task? _inputHandler;

    private readonly Stopwatch _totalStopWatch = new();
    private readonly Stopwatch _pausableStopWatch = new();

    private volatile bool _stopListener = false;
    private bool _isDisposed = false;
    private bool _isPaused = false;
    private bool _isTaskRunning = false;

    private int _userTime = -1;
    private int _lastTime = -1;
    private int _lastElapsedTimeMinutes = -1;
    private int _lastElapsedTimeSeconds = -1;


    public AppTimer(int time, CancellationTokenSource cts)
    {
        TimerAwait(time, cts);
    }

    private void TimerAwait(int time, CancellationTokenSource cts)
    {
        Console.CursorVisible = false;
        Gui.Work();
        Gui.DrawKeyBinds(1);

        _userCts = cts;
        _userTime = time;
        TimerStart(time, 0, false);
    }

    private void TimerStart(int time, int seconds, bool quit = false)
    {
        CancellationToken token = StartNewTimerSession();

        _ = TimerProcess(time, seconds, token, quit);
    }

    // ----------   TIMER   ---------- //
    private async Task TimerProcess(int time, int seconds, CancellationToken token, bool quit = false)
    {
        _lastTime = time;
        _lastElapsedTimeMinutes = TimeSpan.Zero.Milliseconds;
        _lastElapsedTimeSeconds = TimeSpan.Zero.Milliseconds;

        int oneMinuteInMS = 60000;
        int oneSecondInMs = 1000;


        if (quit == true) _cts?.Cancel();

        try
        {
            await Task.Delay((time * oneMinuteInMS) - (seconds * oneSecondInMs), token);

            ConsoleCommands.PlayMusic();
            PreQuitTimer();
            Logic.CalculateTotalTimeSpent(in _userTime);
            Gui.Done();
        }
        catch (OperationCanceledException)
        {
            if (_isPaused == true && quit == false)
                return;

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
            PreQuitTimer();
            Logic.CalculateTotalTimeSpent(in _userTime, roundedDelta);
            Gui.DrawHistoryList();
            Gui.Canceled();
        }
        catch { Logic.PrintError(); }

        QuitTimer();
    }

    private void TimerPause()
    {
        if (!CheckCorrectness())
            return;

        _isPaused = true;
        Gui.DrawKeyBinds(2);

        {
            _cts?.Cancel();
            _totalStopWatch.Stop();
            _pausableStopWatch.Stop();
            _lastElapsedTimeMinutes = (int)_pausableStopWatch.Elapsed.TotalMinutes;
            _lastElapsedTimeSeconds = (int)_pausableStopWatch.Elapsed.Seconds;
        }
    }

    private void TimerUnpause(bool quit = false)
    {
        if (!CheckCorrectness())
            return;

        _isPaused = false;
        Gui.DrawKeyBinds(1);

        {
            _totalStopWatch.Start();

            _lastTime -= _lastElapsedTimeMinutes;
            if (_lastElapsedTimeMinutes > _lastTime)
            {
                Logic.PrintError();
                _lastTime = _lastElapsedTimeMinutes;
            }

            TimerStart(_lastTime, _lastElapsedTimeSeconds, quit);
        }
    }

    private bool CheckCorrectness()
    {
        if (_lastTime == -1 || _lastElapsedTimeMinutes == -1 || _lastElapsedTimeSeconds == -1)
        {
            Logic.PrintError();
            return false;
        }

        return true;
    }


    private CancellationToken StartNewTimerSession()
    {
        _cts?.Dispose();
        _cts = new CancellationTokenSource();

        if (_isTaskRunning == false)
        {
            StartNewInputHandler();
            StartNewTimerDisplayer();
            _isTaskRunning = true;
        }

        return _cts.Token;
    }

    private void StartNewInputHandler()
    {
        var _inputHandler = Task.Run(() =>
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
                            _stopListener = true;
                            break;
                        }
                    }
                    else if (key.Key == ConsoleKey.Spacebar)
                    {
                        if (_isPaused == false)
                        {
                            WriteTextHere(text: "----------   PAUSED   ---------- ",
                                            left: 9, top: 0, color: ConsoleColor.Red);
                            TimerPause();
                        }
                        else if (_isPaused == true)
                        {
                            WriteTextHere(text: "++++++++++   RESUMED   ++++++++++",
                                            left: 9, top: 0, color: ConsoleColor.Green);
                            TimerUnpause();
                        }

                        continue;
                    }
                }

                Thread.Sleep(150);
            }

            _cts?.CancelAsync();
            _isPaused = false;
        }, _cts!.Token);
    }

    private void StartNewTimerDisplayer()
    {
        var _timeDisplayer = Task.Run(async () =>
        {
            while (_stopListener == false)
            {
                WriteTextHere(text: $"{_totalStopWatch.Elapsed:hh\\:mm\\:ss}", left: 21, top: 23,
                                ConsoleColor.Yellow);
                await Task.Delay(1000);
            }

        });
    }

    private void PreQuitTimer()
    {
        _totalStopWatch.Stop();
        ConsoleCommands.OpenConsole();
        Gui.DrawTotalTimeSpent();
    }

    private void QuitTimer()
    {
        _stopListener = true;
        _isTaskRunning = false;
        _totalStopWatch.Reset();
        _pausableStopWatch.Reset();
        WriteTextHere(text: "                ", left: 20, top: 23);
        WriteTextHere(text: " ", left: 41, top: 10, color: 0);
        WriteTextHere(text: " ", left: 69, top: 10, color: 0);
        Gui.DrawKeyBinds(0);
        Console.CursorVisible = true;
        _userCts?.Cancel();
    }

    public void Dispose()
    {
        if (_isDisposed) return;
        _isDisposed = true;

        _cts?.Dispose();
        _userCts?.Dispose();
    }

}