namespace Anino.Models;

public class AnalyzedPropertyInfo
{
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsNullable { get; set; }
    public bool IsCollection { get; set; }
    public bool IsDictionary { get; set; }
    public string? GenericArgument { get; set; }
    public List<AnalyzedPropertyInfo>? NestedProperties { get; set; }
}