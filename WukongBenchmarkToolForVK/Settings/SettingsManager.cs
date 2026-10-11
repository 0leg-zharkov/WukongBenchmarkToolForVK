using System.Text.RegularExpressions;

namespace WukongBenchmarkToolForVK.Settings
{
    public class SettingsManager
    {
        private readonly string _configPath;

        public SettingsManager(string benchmarkPath)
        {
            _configPath = Path.Combine(benchmarkPath, @"b1\Saved\Config\Windows\GameUserSettings.ini");
        }

        // для обновления простых строк (ResolutionSizeX и т.д.)
        private void UpdateSimpleSetting(ref string[] lines, string key, string value)
        {
            bool found = false;
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].TrimStart().StartsWith(key + "="))
                {
                    lines[i] = $"{key}={value}";
                    found = true;
                    break;
                }
            }
            if (!found)
            {
                Array.Resize(ref lines, lines.Length + 1);
                lines[lines.Length - 1] = $"{key}={value}";
            }
        }

        // для обновления значений внутри строки UISettingData
        private void UpdateUiSettingData(ref string[] lines, string key, string value)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].StartsWith("UISettingData="))
                {
                    // замена значения конкретного ключа
                    // замена шаблона ("Ключ", "Значение") ("Ключ", "НовоеЗначение")
                    string pattern = $@"\(""{key}"",\s*""[^""]*""\)";
                    string replacement = $@"(""{key}"", ""{value}"")";

                    lines[i] = Regex.Replace(lines[i], pattern, replacement);
                    return;
                }
            }
        }

        //TODO: DRY через цикл оформить
        public void ApplyCpuTestSettings()
        {
            if (!File.Exists(_configPath)) return;

            string[] lines = File.ReadAllLines(_configPath);

            // 1. замена базового разрешения
            UpdateSimpleSetting(ref lines, "ResolutionSizeX", "1280");
            UpdateSimpleSetting(ref lines, "ResolutionSizeY", "720");
            UpdateSimpleSetting(ref lines, "DesiredScreenWidth", "1280");
            UpdateSimpleSetting(ref lines, "DesiredScreenHeight", "720");

            // 2. замена настройки внутри UISettingData
            // 0 - Low, 1 - Medium и тп
            UpdateUiSettingData(ref lines, "QualityLevel", "0");
            UpdateUiSettingData(ref lines, "ImageQuality", "720");
            UpdateUiSettingData(ref lines, "Rtx", "0");

            // отключить тяжелые эффекты
            UpdateUiSettingData(ref lines, "ViewDistance", "0");
            UpdateUiSettingData(ref lines, "ShadowQuality", "0");
            UpdateUiSettingData(ref lines, "PostProcessing", "0");
            UpdateUiSettingData(ref lines, "TextureQuality", "0");
            UpdateUiSettingData(ref lines, "FxQuality", "0");
            UpdateUiSettingData(ref lines, "VegetationQuality", "0");
            UpdateUiSettingData(ref lines, "GlobalIllumination", "0");
            UpdateUiSettingData(ref lines, "ReflectionQuality", "0");

            File.WriteAllLines(_configPath, lines);
            Console.WriteLine("[SettingsManager] Настройки для CPU-теста применены (1280x720, Low, RTX Off).");
        }

        public void ApplyGpuTestSettings()
        {
            if (!File.Exists(_configPath)) return;

            string[] lines = File.ReadAllLines(_configPath);

            // замена базового разрешение на 1920x1080 
            UpdateSimpleSetting(ref lines, "ResolutionSizeX", "1920");
            UpdateSimpleSetting(ref lines, "ResolutionSizeY", "1080");
            UpdateSimpleSetting(ref lines, "DesiredScreenWidth", "1920");
            UpdateSimpleSetting(ref lines, "DesiredScreenHeight", "1080");

            // замена настройки внутри UISettingData
            // максимальное качество
            UpdateUiSettingData(ref lines, "QualityLevel", "6");
            UpdateUiSettingData(ref lines, "ImageQuality", "1080");
            UpdateUiSettingData(ref lines, "Rtx", "0"); // у меня RX 570 :_(

            //тяжелые эффекты на максимум
            UpdateUiSettingData(ref lines, "ViewDistance", "3");
            UpdateUiSettingData(ref lines, "ShadowQuality", "3");
            UpdateUiSettingData(ref lines, "PostProcessing", "3");
            UpdateUiSettingData(ref lines, "TextureQuality", "3");
            UpdateUiSettingData(ref lines, "FxQuality", "3");
            UpdateUiSettingData(ref lines, "VegetationQuality", "3");
            UpdateUiSettingData(ref lines, "GlobalIllumination", "3");
            UpdateUiSettingData(ref lines, "ReflectionQuality", "3");

            File.WriteAllLines(_configPath, lines);
            Console.WriteLine("[SettingsManager] Настройки для GPU-теста применены (1920x1080, Max Quality).");
        }
    }
}