namespace LibraryManagement.Models;

public class BranchPerformanceAnalysis
{
    public LibraryBranch Branch { get; set; } = new();
    public BranchMetrics Metrics { get; set; } = new();
    public List<ServiceAnalysis> Services { get; set; } = new();
    public IEnumerable<BranchComparison> Comparisons { get; set; } = new List<BranchComparison>();
}

public class BranchMetrics
{
    public Dictionary<string, double> OperationalMetrics { get; set; } = new();
    public List<EfficiencyMeasure> Efficiency { get; set; } = new();
    public ResourceUtilization Resources { get; set; } = new();
}

public class EfficiencyMeasure
{
    public string MeasureName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<EfficiencyDriver> Drivers { get; set; } = new();
}

public class EfficiencyDriver
{
    public string DriverName { get; set; } = string.Empty;
    public double Impact { get; set; }
    public List<string> Mechanisms { get; set; } = new();
}

public class ResourceUtilization
{
    public List<ResourceMetric> Resources { get; set; } = new();
    public UtilizationSummary Summary { get; set; } = new();
}

public class ResourceMetric
{
    public string ResourceType { get; set; } = string.Empty;
    public double UtilizationRate { get; set; }
    public List<UtilizationPeriod> Periods { get; set; } = new();
}

public class UtilizationPeriod
{
    public string Period { get; set; } = string.Empty;
    public double Rate { get; set; }
    public List<string> Factors { get; set; } = new();
}

public class UtilizationSummary
{
    public double OverallUtilization { get; set; }
    public List<UtilizationInsight> Insights { get; set; } = new();
}

public class UtilizationInsight
{
    public string InsightType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<string> Recommendations { get; set; } = new();
}

public class ServiceAnalysis
{
    public string ServiceName { get; set; } = string.Empty;
    public ServiceMetrics Metrics { get; set; } = new();
    public List<ServiceTrend> Trends { get; set; } = new();
}

public class ServiceMetrics
{
    public Dictionary<string, double> Values { get; set; } = new();
    public List<ServiceBenchmark> Benchmarks { get; set; } = new();
}

public class ServiceBenchmark
{
    public string BenchmarkType { get; set; } = string.Empty;
    public double BenchmarkValue { get; set; }
    public double CurrentValue { get; set; }
    public List<string> ComparisonNotes { get; set; } = new();
}

public class ServiceTrend
{
    public string TrendName { get; set; } = string.Empty;
    public List<ServiceDataPoint> DataPoints { get; set; } = new();
}

public class ServiceDataPoint
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public List<string> Context { get; set; } = new();
}

public class BranchComparison
{
    public string ComparisonType { get; set; } = string.Empty;
    public List<LibraryBranch> CompareBranches { get; set; } = new();
    public List<ComparisonMetric> Metrics { get; set; } = new();
}

public class ComparisonMetric
{
    public string MetricName { get; set; } = string.Empty;
    public Dictionary<string, double> BranchValues { get; set; } = new();
    public List<ComparisonInsight> Insights { get; set; } = new();
}

public class ComparisonInsight
{
    public string InsightType { get; set; } = string.Empty;
    public string Finding { get; set; } = string.Empty;
    public List<string> Implications { get; set; } = new();
}
