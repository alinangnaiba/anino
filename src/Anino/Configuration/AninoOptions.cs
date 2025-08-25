using Anino.Models;

namespace Anino.Configuration;

public class AninoOptions
{
    public FileInfo? File { get; set; }
    public int Port { get; set; } = DefaultValueOf.Port;
    public int Latency { get; set; } = DefaultValueOf.Latency;
    public string? GenerateDefinition { get; set; }
    public IEnumerable<string>? ScanFiles { get; set; }
    public string? ScanOutput { get; set; }
    public IEnumerable<string>? ScanTargets { get; set; }
}