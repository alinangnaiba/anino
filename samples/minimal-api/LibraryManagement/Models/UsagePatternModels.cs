namespace LibraryManagement.Models;

public class UsagePattern
{
    public string PatternType { get; set; } = string.Empty;
    public PatternMetrics Metrics { get; set; } = new();
    public List<PatternComponent> Components { get; set; } = new();
    public IEnumerable<PatternImplication> Implications { get; set; } = new List<PatternImplication>();
}

public class PatternMetrics
{
    public Dictionary<string, double> Values { get; set; } = new();
    public List<MetricTrend> Trends { get; set; } = new();
}

public class MetricTrend
{
    public string MetricName { get; set; } = string.Empty;
    public string TrendDirection { get; set; } = string.Empty;
    public double Magnitude { get; set; }
    public List<string> Drivers { get; set; } = new();
}

public class PatternComponent
{
    public string ComponentName { get; set; } = string.Empty;
    public Dictionary<string, object> Properties { get; set; } = new();
    public List<ComponentRelationship> Relationships { get; set; } = new();
}

public class ComponentRelationship
{
    public string RelationshipType { get; set; } = string.Empty;
    public string TargetComponent { get; set; } = string.Empty;
    public double Strength { get; set; }
    public List<string> Mechanisms { get; set; } = new();
}

public class PatternImplication
{
    public string ImplicationType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<BusinessImpact> Impacts { get; set; } = new();
}

public class BusinessImpact
{
    public string ImpactArea { get; set; } = string.Empty;
    public double Magnitude { get; set; }
    public List<string> Consequences { get; set; } = new();
}
