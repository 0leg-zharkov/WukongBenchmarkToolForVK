using System.Text.Json;
using System.Text.Json.Serialization;

namespace WukongBenchmarkToolForVK.Results;

public class BenchmarkResultParser
{
    private readonly string _historyPath;

    public BenchmarkResultParser()
    {
        string tempPath = Path.GetTempPath();
        _historyPath = Path.Combine(tempPath, "b1", "BenchMarkHistory", "Tool");
    }

    public string? GetLatestResultFile()
    {
        if (!Directory.Exists(_historyPath)) return null;

        return Directory.GetFiles(_historyPath)
            .Where(f => Path.GetFileNameWithoutExtension(f).All(char.IsDigit))
            .OrderByDescending(f => File.GetLastWriteTime(f))
            .FirstOrDefault();
    }

    public BenchmarkResult? GetLatestResult()
    {
        string? file = GetLatestResultFile();
        if (file == null) return null;

        string json = File.ReadAllText(file);

        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString
        };

        try
        {
            return JsonSerializer.Deserialize<BenchmarkResult>(json, options);
        }
        catch (JsonException)
        {
            return null;
        }
    }
}