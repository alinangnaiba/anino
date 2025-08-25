namespace Anino.Models;

public class DiscoveredEndpoint
{
    public string Path { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public string ReturnType { get; set; } = string.Empty;
    public bool IsAsync { get; set; }
    public int StatusCode { get; set; } = 200;
    public AnalyzedTypeInfo? ReturnTypeInfo { get; set; }
}