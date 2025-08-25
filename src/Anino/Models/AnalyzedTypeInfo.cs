namespace Anino.Models;

public class AnalyzedTypeInfo
{
    public string TypeName { get; set; } = string.Empty;
    public bool IsCollection { get; set; }
    public bool IsGeneric { get; set; }
    public string? GenericArgument { get; set; }
    public List<AnalyzedPropertyInfo> Properties { get; set; } = new();
}
