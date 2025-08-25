namespace CollectionTest;

public class SimpleItem
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class CollectionContainer
{
    public List<SimpleItem> Items { get; set; } = new();
    public IEnumerable<string> Tags { get; set; } = new List<string>();
    public Dictionary<string, double> Metrics { get; set; } = new();
    public string[] Categories { get; set; } = Array.Empty<string>();
}