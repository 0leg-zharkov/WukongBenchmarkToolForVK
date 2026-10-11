namespace WukongBenchmarkToolForVK;

public class ReportPrinter
{
    public void PrintResult(string testName, BenchmarkResult? result)
    {
        if (result == null)
        {
            Console.WriteLine($"  [{testName}] Результат недоступен.");
            return;
        }

        Console.WriteLine($"\n========== РЕЗУЛЬТАТ: {testName} ==========");
        Console.WriteLine($"  Средний FPS:     {result.FPSAvg:F1}");
        Console.WriteLine($"  Максимум FPS:    {result.FPSMax:F0}");
        Console.WriteLine($"  Минимум FPS:     {result.FPSMin:F0}");
        Console.WriteLine($"  95-й перцентиль: {result.FPS95:F0}");
        Console.WriteLine($"  Загрузка CPU:    {result.CPUAvg:F0}%");
        Console.WriteLine($"  Загрузка GPU:    {result.GPUAvg:F0}%");
        Console.WriteLine($"  Видеопамять:     {result.VideoMem:F2} GB (из {result.VideoMemSize})");
        Console.WriteLine($"  Разрешение:      {result.ScreenResolution}");
        Console.WriteLine($"  Качество:        {result.QualityLevel}");
        Console.WriteLine($"  RTX:             {(result.Rtx == 1 ? "Вкл" : "Выкл")}");
        Console.WriteLine($"  DLSS:            {(result.Dlss == 1 ? "Вкл" : "Выкл")}");
        Console.WriteLine($"  Генерация кадров:{(result.InsertFrame == 1 ? "Вкл" : "Выкл")}");
        Console.WriteLine($"  DX12:            {(result.Dx12 == 1 ? "Вкл" : "Выкл")}");
        Console.WriteLine($"  Игра:            {result.GameVer}");
    }

    public void PrintSummary(List<(string Name, BenchmarkResult? Result)> results)
    {
        Console.WriteLine("\n\n========================================");
        Console.WriteLine("ИТОГОВЫЙ ОТЧЁТ");
        Console.WriteLine("========================================");

        var first = results.Select(r => r.Result).FirstOrDefault(r => r != null);
        if (first != null)
        {
            Console.WriteLine("\n--- Характеристики ПК ---");
            Console.WriteLine($"  CPU:      {first.CPUModel.Trim()}");
            Console.WriteLine($"  GPU:      {first.GPUModel}");
            Console.WriteLine($"  Драйвер:  {first.GpuDriverVer}");
            Console.WriteLine($"  RAM:      {first.SysMem}");
            Console.WriteLine($"  VRAM:     {first.VideoMemSize}");
            Console.WriteLine($"  ОС:       {first.SysVer}");
            Console.WriteLine($"  Игра:     {first.GameVer}");
        }

        Console.WriteLine("\n--- Сравнение тестов ---");
        foreach (var (name, result) in results)
        {
            if (result == null)
            {
                Console.WriteLine($"  {name}: результат недоступен");
                continue;
            }
            Console.WriteLine(
                $"  {name}: FPS ср. = {result.FPSAvg:F1} | " +
                $"Min = {result.FPSMin:F0} | " +
                $"GPU = {result.GPUAvg:F0}%");
        }
    }
}