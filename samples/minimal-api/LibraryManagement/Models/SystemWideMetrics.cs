namespace LibraryManagement.Models;

public class SystemWideMetrics
{
    public CirculationMetrics Circulation { get; set; } = new();
    public List<UsagePattern> UsagePatterns { get; set; } = new();
    public MembershipAnalytics Membership { get; set; } = new();
    public IEnumerable<PerformanceIndicator> KPIs { get; set; } = new List<PerformanceIndicator>();
}
