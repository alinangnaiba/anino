namespace LibraryManagement.Models;

public class CollectionInsight
{
    public string CollectionType { get; set; } = string.Empty;
    public CollectionAnalysisData Analysis { get; set; } = new();
    public List<CollectionTrend> Trends { get; set; } = new();
    public IEnumerable<CollectionRecommendation> Recommendations { get; set; } = new List<CollectionRecommendation>();
}

public class CollectionAnalysisData
{
    public int TotalItems { get; set; }
    public List<CategoryBreakdown> Categories { get; set; } = new();
    public CollectionHealth Health { get; set; } = new();
}

public class CategoryBreakdown
{
    public string Category { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public List<CategoryMetric> Metrics { get; set; } = new();
}

public class CategoryMetric
{
    public string MetricName { get; set; } = string.Empty;
    public double Value { get; set; }
    public List<string> Observations { get; set; } = new();
}

public class CollectionHealth
{
    public double HealthScore { get; set; }
    public List<HealthIndicator> Indicators { get; set; } = new();
}

public class HealthIndicator
{
    public string IndicatorName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<string> Factors { get; set; } = new();
}

public class CollectionTrend
{
    public string TrendType { get; set; } = string.Empty;
    public List<TrendPoint> Points { get; set; } = new();
}

public class TrendPoint
{
    public DateTime Date { get; set; }
    public double Value { get; set; }
    public List<string> Notes { get; set; } = new();
}

public class CollectionRecommendation
{
    public string RecommendationType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<RecommendationAction> Actions { get; set; } = new();
}

public class RecommendationAction
{
    public string ActionType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Priority { get; set; }
    public List<string> Requirements { get; set; } = new();
}
