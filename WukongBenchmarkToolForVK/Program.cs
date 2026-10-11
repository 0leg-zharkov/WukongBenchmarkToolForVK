using System.Text;
using WukongBenchmarkToolForVK;
using WukongBenchmarkToolForVK.Automation;

namespace WukongBenchmarkToolForVK;

class Program
{
    private const string DefaultBenchmarkPath =
        @"D:\SteamLibrary\steamapps\common\Black Myth Wukong Benchmark Tool";

    static async Task<int> Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        // справка
        if (args.Contains("--help") || args.Contains("-h"))
        {
            Console.WriteLine("Использование: WukongBenchmarkToolForVK.exe [путь_к_бенчмарку]");
            Console.WriteLine($"По умолчанию: {DefaultBenchmarkPath}");
            return 0;
        }

        string reportPath = Path.Combine(
            AppContext.BaseDirectory,
            $"report_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.txt");

        using var logFile = new StreamWriter(reportPath, append: false, Encoding.UTF8);
        Console.SetOut(new TeeWriter(Console.Out, logFile));

        Console.WriteLine("=== Wukong Benchmark Tool ===");
        Console.WriteLine($"Отчёт: {reportPath}\n");

        string? benchmarkRoot = ResolveBenchmarkPath(args);
        if (benchmarkRoot == null)
        {
            Console.WriteLine("Папка бенчмарка не найдена.");
            return 1;
        }

        Console.WriteLine($"Папка бенчмарка: {benchmarkRoot}\n");

        try
        {
            var app = new BenchmarkApp(benchmarkRoot);
            return await app.RunAsync();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Критическая ошибка: {ex.Message}");
            return 1;
        }
        finally
        {
            WindowController.RestoreConsole();
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
            Console.WriteLine($"\nОтчёт сохранён: {reportPath}");
        }
    }

    private static string? ResolveBenchmarkPath(string[] args)
    {
        if (args.Length > 0)
        {
            string candidate = args[0].Trim('"', '\'');
            if (Directory.Exists(candidate))
                return candidate;

            Console.WriteLine($"Путь из аргумента не найден: {candidate}");
        }

        // путь по умолчанию
        if (Directory.Exists(DefaultBenchmarkPath))
            return DefaultBenchmarkPath;

        // ручной ввод
        Console.WriteLine("Путь по умолчанию не найден. Введите вручную:");
        string? input = Console.ReadLine()?.Trim('"', '\'');
        return !string.IsNullOrWhiteSpace(input) && Directory.Exists(input) ? input : null;
    }
}