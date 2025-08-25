namespace LibraryManagement.Models;

public class SeasonalAnalysis
{
    public List<SeasonalTrend> Trends { get; set; } = new();
    public Dictionary<string, SeasonalMetrics> Seasons { get; set; } = new();
    public IEnumerable<SeasonalRecommendation> Recommendations { get; set; } = new List<SeasonalRecommendation>();
}

public class SeasonalTrend
{
    public string Season { get; set; } = string.Empty;
    public List<TrendMetric> Metrics { get; set; } = new();
    public SeasonalContext Context { get; set; } = new();
}

public class TrendMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public double YearOverYearChange { get; set; }
    public List<string> Factors { get; set; } = new();
}

public class SeasonalContext
{
    public List<ContextualFactor> Factors { get; set; } = new();
    public Dictionary<string, object> ExternalInfluences { get; set; } = new();
}

public class ContextualFactor
{
    public string FactorType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Impact { get; set; }
    public List<string> Manifestations { get; set; } = new();
}

public class SeasonalMetrics
{
    public double AverageActivity { get; set; }
    public List<CategoryPerformance> Categories { get; set; } = new();
    public VariationAnalysis Variation { get; set; } = new();
}

public class CategoryPerformance
{
    public string Category { get; set; } = string.Empty;
    public double PerformanceIndex { get; set; }
    public List<PerformanceDriver> Drivers { get; set; } = new();
}

public class PerformanceDriver
{
    public string DriverName { get; set; } = string.Empty;
    public double Contribution { get; set; }
    public List<string> Mechanisms { get; set; } = new();
}

public class VariationAnalysis
{
    public double Coefficient { get; set; }
    public List<VariationSource> Sources { get; set; } = new();
}

public class VariationSource
{
    public string SourceName { get; set; } = string.Empty;
    public double Variance { get; set; }
    public List<string> Explanations { get; set; } = new();
}

public class SeasonalRecommendation
{
    public string Season { get; set; } = string.Empty;
    public string RecommendationType { get; set; } = string.Empty;
    public List<ActionableInsight> Insights { get; set; } = new();
}

public class ActionableInsight
{
    public string InsightTitle { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<ImplementationStep> Steps { get; set; } = new();
}

public class ImplementationStep
{
    public string StepDescription { get; set; } = string.Empty;
    public int Priority { get; set; }
    public List<StepResource> Resources { get; set; } = new();
}

public class StepResource
{
    public string ResourceType { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<string> AccessMethods { get; set; } = new();
}
