using WukongBenchmarkToolForVK.Settings;

namespace WukongBenchmarkToolForVK.Models;

public record TestProfile(
    string DisplayName,
    Action<SettingsManager> ApplySettings,
    int DurationMinutes);
