using System.Text.Json.Serialization;

public class BenchmarkResult
{
    [JsonPropertyName("TimeStamp")] 
    public long TimeStamp { get; set; }

    [JsonPropertyName("FPSAvg")] 
    public double FPSAvg { get; set; }

    [JsonPropertyName("FPSMax")]
    public double FPSMax { get; set; }

    [JsonPropertyName("FPSMin")] 
    public double FPSMin { get; set; }

    [JsonPropertyName("FPS95")] 
    public double FPS95 { get; set; }

    [JsonPropertyName("CPUAvg")]
    public double CPUAvg { get; set; }

    [JsonPropertyName("GPUAvg")]
    public double GPUAvg { get; set; }

    [JsonPropertyName("VideoMem")]
    public double VideoMem { get; set; }

    [JsonPropertyName("GameVer")]
    public string GameVer { get; set; }

    [JsonPropertyName("SysVer")]
    public string SysVer { get; set; }

    [JsonPropertyName("CPUModel")]
    public string CPUModel { get; set; }

    [JsonPropertyName("GPUModel")] 
    public string GPUModel { get; set; }

    [JsonPropertyName("GpuDriverVer")]
    public string GpuDriverVer { get; set; }

    [JsonPropertyName("VideoMemSize")]
    public string VideoMemSize { get; set; }

    [JsonPropertyName("SysMem")]
    public string SysMem { get; set; }

    [JsonPropertyName("ScreenResolution")]
    public string ScreenResolution { get; set; }

    [JsonPropertyName("QualityLevel")] 
    public int QualityLevel { get; set; }

    [JsonPropertyName("Rtx")]
    public int Rtx { get; set; }

    [JsonPropertyName("Dlss")]
    public int Dlss { get; set; }

    [JsonPropertyName("Dx12")] 
    public int Dx12 { get; set; }

    [JsonPropertyName("InsertFrame")] 
    public int InsertFrame { get; set; }
}