using WukongBenchmarkToolForVK.Automation;
using WukongBenchmarkToolForVK.Models;
using WukongBenchmarkToolForVK.Processes;
using WukongBenchmarkToolForVK.Results;
using WukongBenchmarkToolForVK.Settings;

namespace WukongBenchmarkToolForVK;

public class BenchmarkApp
{
    // время между тестами, чтобы стим успел закрыть игру
    private const int DelayBetweenTestsMs = 25000;

    private readonly SettingsManager _settings;
    private readonly BenchmarkProcessRunner _processRunner;
    private readonly BenchmarkRunner _uiRunner;
    private readonly BenchmarkResultParser _parser;
    private readonly ReportPrinter _printer;

    public BenchmarkApp(string benchmarkRoot)
    {
        _settings = new SettingsManager(benchmarkRoot);
        _processRunner = new BenchmarkProcessRunner(benchmarkRoot);
        _uiRunner = new BenchmarkRunner();
        _parser = new BenchmarkResultParser();
        _printer = new ReportPrinter();
    }

    public async Task<int> RunAsync()
    {
        var profiles = new[]
        {
            // 3 минуты на прогон теста
            new TestProfile("CPU-ТЕСТ", s => s.ApplyCpuTestSettings(), 3),
            new TestProfile("GPU-ТЕСТ", s => s.ApplyGpuTestSettings(), 3),
        };

        var results = new List<(string Name, BenchmarkResult? Result)>();

        for (int i = 0; i < profiles.Length; i++)
        {
            var profile = profiles[i];
            var result = await RunSingleTestAsync(profile);
            results.Add((profile.DisplayName, result));

            // пауза между тестами
            if (i < profiles.Length - 1)
            {
                Console.WriteLine($"[App] Пауза {DelayBetweenTestsMs / 1000} сек перед следующим тестом...");
                await Task.Delay(DelayBetweenTestsMs);
            }
        }

        _printer.PrintSummary(results);
        return 0;
    }

    private async Task<BenchmarkResult?> RunSingleTestAsync(TestProfile profile)
    {
        Console.WriteLine("\n========================================");
        Console.WriteLine($"ЭТАП: {profile.DisplayName}");
        Console.WriteLine("========================================");

        profile.ApplySettings(_settings);

        var game = await _processRunner.LaunchAndWaitForWindowAsync();
        if (game == null)
        {
            Console.WriteLine("[App] Процесс бенчмарка не появился");
            return null;
        }

        await _uiRunner.AutoStartTestAsync(game);

        Console.WriteLine($"[App] Ждём завершения теста ({profile.DurationMinutes} мин)...");
        await Task.Delay(TimeSpan.FromMinutes(profile.DurationMinutes));

        await _uiRunner.FinishTestAndExitMenuAsync(game);

        Console.WriteLine("[App] Ждём 5 сек для сохранения результата...");
        await Task.Delay(5000);

        await _processRunner.CloseGracefullyAsync(game);

        // вернуть консольку
        WindowController.RestoreConsole();
        await Task.Delay(500);

        var result = _parser.GetLatestResult();
        _printer.PrintResult(profile.DisplayName, result);

        return result;
    }
}