namespace LibraryManagement.Models;

public class ComprehensiveLibraryAnalytics
{
    public Guid LibrarySystemId { get; set; }
    public DateTime GeneratedAt { get; set; }
    public SystemWideMetrics SystemMetrics { get; set; } = new();
    public List<BranchPerformanceAnalysis> BranchAnalytics { get; set; } = new();
    public IEnumerable<CollectionInsight> CollectionAnalysis { get; set; } = new List<CollectionInsight>();
    public Dictionary<string, double> CustomKPIs { get; set; } = new();
    public AdvancedReportingData ReportingData { get; set; } = new();
}
