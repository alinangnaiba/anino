namespace LibraryManagement.Models;

public class PerformanceIndicator
{
    public string KPIName { get; set; } = string.Empty;
    public double CurrentValue { get; set; }
    public double TargetValue { get; set; }
    public List<KPIComponent> Components { get; set; } = new();
    public KPIStatus Status { get; set; } = new();
}

public class KPIComponent
{
    public string ComponentName { get; set; } = string.Empty;
    public double Weight { get; set; }
    public double ContributionValue { get; set; }
    public List<string> Influences { get; set; } = new();
}

public class KPIStatus
{
    public string Status { get; set; } = string.Empty;
    public List<StatusIndicator> Indicators { get; set; } = new();
}

public class StatusIndicator
{
    public string IndicatorType { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public List<string> Actions { get; set; } = new();
}
