using System.Diagnostics;

namespace WukongBenchmarkToolForVK.Processes;

public class BenchmarkProcessRunner
{
    private readonly string _executablePath;
    private readonly string _workingDirectory;

    public BenchmarkProcessRunner(string benchmarkRoot)
    {
        _executablePath = Path.Combine(benchmarkRoot, @"b1\Binaries\Win64\b1-Win64-Shipping.exe");
        _workingDirectory = Path.Combine(benchmarkRoot, @"b1\Binaries\Win64");
    }

    public async Task<Process?> LaunchAndWaitForWindowAsync(int maxWaitSeconds = 60)
    {
        var knownPids = Process.GetProcesses().Select(p => p.Id).ToHashSet();

        // запуск лаунчера
        using (var launcher = Process.Start(new ProcessStartInfo
        {
            FileName = _executablePath,
            WorkingDirectory = _workingDirectory,
            UseShellExecute = false
        }))
        {
            if (launcher == null) return null;
            _ = launcher.WaitForExit(15000);
        }

        var start = DateTime.Now;

        while ((DateTime.Now - start).TotalSeconds < maxWaitSeconds)
        {
            await Task.Delay(1000);

            var proc = Process.GetProcesses()
                .Where(p => p.ProcessName.Contains("b1-Win64", StringComparison.OrdinalIgnoreCase))
                .Where(p => !knownPids.Contains(p.Id))
                .OrderByDescending(p => p.WorkingSet64)
                .FirstOrDefault();

            if (proc != null)
            {
                // подождать до появления окна
                for (int i = 0; i < 30; i++)
                {
                    await Task.Delay(1000);
                    proc.Refresh();
                    if (proc.MainWindowHandle != IntPtr.Zero) return proc;
                }
                return proc;
            }
        }
        return null;
    }

    public async Task CloseGracefullyAsync(Process proc)
    {
        try
        {
            if (proc.HasExited) return;

            _ = proc.CloseMainWindow();
            await Task.Delay(3000);

            if (!proc.HasExited)
            {
                proc.Kill(entireProcessTree: true);
                await Task.Delay(2000);
            }
        }
        catch { }
        finally { proc.Dispose(); }
    }
}